using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class HandVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition Owner;
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren Slots;

    [Header("Transform References")]
    public Transform DrawPreviewSpot;
    public Transform DeckTransform;
    public Transform OtherCardDrawSourceTransform;
    public Transform PlayPreviewSpot;
    public Transform DisCardPreviewSpot;

    public int CardsOnHand
    {
        get
        {
            return _cardsInHand.Count;
        }
    }

    // PRIVATE : a list of all card visual representations as GameObjects
    private List<GameObject> _cardsInHand = new List<GameObject>();

    public void ActiveAllCards()
    {
        foreach (GameObject gameObject in _cardsInHand)
        {
            gameObject.SetActive(true);
        }
    }

    // ADDING OR REMOVING CARDS FROM HAND

    // add a new card GameObject to hand
    public async Task AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        _cardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(Slots.transform);

        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        await cardManager.ChangeOwnerAndLocation(GlobalSettings.Instance.Players[this.Owner], CardLocation.Hand);


        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        // remove a card from the list
        _cardsInHand.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();

    }

    // remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        _cardsInHand.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return _cardsInHand[index];
    }

    // MANAGING CARDS AND SLOTS

    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (_cardsInHand.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[_cardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // shift all cards to their new Slots
    public void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in _cardsInHand)
        {
            try
            {
                // tween this card to a new Slot
                g.transform.DOLocalMoveX(Slots.Children[_cardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);
            }
            catch (Exception ex)
            {
                Debug.LogError("other error is:" + ex.Message);
            }

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = _cardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    public async Task DrawACard(GameObject card)
    {
        await AddACardToHand(card);
    }

    public async Task GetACardFromOther(GameObject card, Player otherPlayer)
    {
        otherPlayer.PArea.HandVisual.PlaceCardsOnNewSlots();
        otherPlayer.PArea.HandVisual.UpdatePlacementOfSlots();
        await AddACardToHand(card);
    }

    public async Task AddACardToHand(GameObject card)
    {
        await AddCard(card);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();
        w.Slot = 0;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        // displace the card so that we can select it in the scene easier.
        s.Append(card.transform.DOLocalMove(Slots.Children[0].transform.localPosition, 1f));
        s.OnComplete(() =>
        {
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
        });
    }


    /// <summary>
    /// 移除卡牌到弃牌堆
    /// </summary>
    /// <param name="CardID"></param>
    public async Task DisCardFromHand(int CardID)
    {
        var tcs = new TaskCompletionSource<bool>();

        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        RemoveCard(card);

        card.transform.SetParent(null);


        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });
        await tcs.Task;

        cardManager.CanBePlayedNow = false;
        card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

        //位置改为弃牌堆
        await cardManager.ChangeOwnerAndLocation(cardManager.Owner, CardLocation.DisDeck);
    }

    // 2 Overloaded method to show a spell played from hand
    public async Task PlayASpellFromHand(int CardID)
    {
        GameObject CardVisual = IDHolder.GetGameObjectWithID(CardID);
        OneCardManager playedCard = CardVisual.GetComponent<OneCardManager>();

        if (playedCard.IsDisguisedCard)
        {
            foreach (int relationCardId in playedCard.RelationRealCardIds)
            {
                GameObject relationCard = IDHolder.GetGameObjectWithID(relationCardId);
                OneCardManager relationCardManager = relationCard.GetComponent<OneCardManager>();
                await relationCardManager.Owner.DisACardFromHand(relationCardId);
            }
            PlayCardManager.Instance.ActivateEffect(playedCard);
            Destroy(CardVisual);
            return;
        }

        CardAsset cardAsset = playedCard.CardAsset;

        RemoveCard(CardVisual);

        CardVisual.transform.SetParent(null);

        Player player = GlobalSettings.Instance.Players[Owner];
        int index = GlobalSettings.Instance.Table.CardsOnTable.Count;

        var tcs = new TaskCompletionSource<bool>();

        Sequence s = DOTween.Sequence();
        s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
        s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(1f);
        s.Append(CardVisual.transform.DOMove(GlobalSettings.Instance.Table.Slots.Children[index].transform.position, 1f));

        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });

        await tcs.Task;

        CardVisual.transform.SetParent(GlobalSettings.Instance.Table.Slots.transform);
        GlobalSettings.Instance.Table.CardsOnTable.Add(CardVisual);

        OneCardManager cardManager = CardVisual.GetComponent<OneCardManager>();
        cardManager.CanBePlayedNow = false;

        //先变到pending
        await cardManager.ChangeOwnerAndLocation(player, CardLocation.Table);

        //TODO 打出一张牌，到pending后,

        CardVisual.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

        var tcs1 = new TaskCompletionSource<bool>();

        Sequence s1 = DOTween.Sequence();
        s1.AppendInterval(0.1f);
        s1.Append(CardVisual.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s1.OnComplete(() =>
        {
            tcs1.SetResult(true);
        });

        await tcs1.Task;

        //再变到弃牌堆
        await cardManager.ChangeOwnerAndLocation(cardManager.Owner, CardLocation.DisDeck);
        PlayCardManager.Instance.ActivateEffect(playedCard);
    }


    public async Task UseASpellFromHand(int CardID)
    {
        GameObject CardVisual = IDHolder.GetGameObjectWithID(CardID);
        OneCardManager playedCard = CardVisual.GetComponent<OneCardManager>();
        CardAsset cardAsset = playedCard.CardAsset;

        switch (cardAsset.TypeOfCard)
        {
            case TypesOfCards.DelayTips:
                {
                    var tcs = new TaskCompletionSource<bool>();

                    RemoveCard(CardVisual);

                    CardVisual.transform.SetParent(null);

                    Player player = GlobalSettings.Instance.FindPlayerByID(playedCard.TargetsPlayerIDs[0]);

                    //可视化加卡
                    player.PArea.JudgementVisual.AddCard(CardVisual);
                    //逻辑加卡
                    player.JudgementLogic.AddCard(playedCard.UniqueCardID);

                    Sequence s = DOTween.Sequence();
                    s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
                    s.Insert(0f, CardVisual.transform.DORotate(new Vector3(0, 0, -90), 1f));
                    s.AppendInterval(2f);
                    s.Append(CardVisual.transform.DOLocalMove(player.PArea.JudgementVisual.Slots.Children[0].transform.localPosition, 1f));
                    s.OnComplete(() =>
                    {
                        tcs.SetResult(true);
                    });
                    await tcs.Task;

                    CardVisual.transform.SetParent(player.PArea.JudgementVisual.Slots.transform);

                    playedCard.CanBePlayedNow = false;

                    await playedCard.ChangeOwnerAndLocation(player, CardLocation.Judgement);
                }
                break;
            case TypesOfCards.Equipment:
                {
                    //获取卡牌脚本
                    OneCardManager cardManager = CardVisual.GetComponent<OneCardManager>();

                    RemoveCard(CardVisual);

                    CardVisual.transform.SetParent(null);

                    Player player = GlobalSettings.Instance.Players[Owner];

                    await EquipmentManager.Instance.AddOrReplaceEquipment(player, CardVisual.GetComponent<OneCardManager>());
                }
                break;
            case TypesOfCards.Base:
            case TypesOfCards.Tips:
                {
                    var tcs = new TaskCompletionSource<bool>();

                    RemoveCard(CardVisual);

                    CardVisual.transform.SetParent(null);

                    Player player = GlobalSettings.Instance.Players[Owner];
                    int index = GlobalSettings.Instance.Table.CardsOnTable.Count;

                    Sequence s = DOTween.Sequence();
                    s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
                    s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
                    s.AppendInterval(1f);
                    s.Append(CardVisual.transform.DOMove(GlobalSettings.Instance.Table.Slots.Children[index].transform.position, 1f));
                    s.OnComplete(() =>
                    {
                        tcs.SetResult(true);
                    });

                    await tcs.Task;

                    CardVisual.transform.SetParent(GlobalSettings.Instance.Table.Slots.transform);
                    GlobalSettings.Instance.Table.CardsOnTable.Add(CardVisual);

                    OneCardManager cardManager = CardVisual.GetComponent<OneCardManager>();
                    cardManager.CanBePlayedNow = false;
                    //改为pending状态
                    await cardManager.ChangeOwnerAndLocation(player, CardLocation.Table);
                }
                break;
        }


    }
}
