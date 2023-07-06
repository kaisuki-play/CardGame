using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TreasureVisual : MonoBehaviour
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

    // PRIVATE : a list of all card visual representations as GameObjects
    private List<GameObject> _cardsInTreasure = new List<GameObject>();

    // are we hovering over this table`s collider with a mouse
    private bool _cursorOverTreasure = false;

    // A 3D collider attached to this game object
    private BoxCollider _col;

    // PROPERTIES

    // returns true if we are hovering over any player`s table collider
    public static bool CursorOverSomeTreasure
    {
        get
        {
            TreasureVisual[] bothTables = GameObject.FindObjectsOfType<TreasureVisual>();
            if (bothTables.Length > 0)
            {
                return (bothTables[0].CursorOverTreasure);
            }
            else
            {
                return false;
            }

        }
    }

    // returns true only if we are hovering over this table`s collider
    public bool CursorOverTreasure
    {
        get { return _cursorOverTreasure; }
    }

    public int CardsOnTreasure
    {
        get
        {
            return _cardsInTreasure.Count;
        }
    }

    // MONOBEHAVIOUR METHODS (mouse over collider detection)
    void Awake()
    {
        _col = GetComponent<BoxCollider>();
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + _col);
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        // create an array of RaycastHits
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == _col)
                passedThroughTableCollider = true;
        }
        _cursorOverTreasure = passedThroughTableCollider;
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + _cursorOverTreasure);
    }

    public void ActiveAllCards()
    {
        foreach (GameObject gameObject in _cardsInTreasure)
        {
            gameObject.SetActive(true);
        }
    }

    // ADDING OR REMOVING CARDS FROM HAND

    // add a new card GameObject to hand
    public async void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        _cardsInTreasure.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(Slots.transform);

        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        await cardManager.ChangeOwnerAndLocation(GlobalSettings.Instance.Players[this.Owner], CardLocation.UnderCart);


        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        // remove a card from the list
        _cardsInTreasure.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();

    }

    // remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        _cardsInTreasure.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return _cardsInTreasure[index];
    }

    // MANAGING CARDS AND SLOTS

    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (_cardsInTreasure.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[_cardsInTreasure.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // shift all cards to their new Slots
    public void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in _cardsInTreasure)
        {
            try
            {
                // tween this card to a new Slot
                g.transform.DOLocalMoveX(Slots.Children[_cardsInTreasure.IndexOf(g)].transform.localPosition.x, 0.3f);
            }
            catch (Exception ex)
            {
                Debug.LogError("other error is:" + ex.Message);
            }

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = _cardsInTreasure.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    public void DrawACard(GameObject card)
    {
        AddACardToTreasure(card);
    }

    public void GetACardFromOther(GameObject card, Player otherPlayer)
    {
        otherPlayer.PArea.TreasureVisual.PlaceCardsOnNewSlots();
        otherPlayer.PArea.TreasureVisual.UpdatePlacementOfSlots();
        AddACardToTreasure(card);
    }

    public void AddACardToTreasure(GameObject card)
    {
        AddCard(card);

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


    // 2 Overloaded method to show a spell played from hand
    public async Task PlayASpellFromTreasure(int CardID)
    {
        GameObject CardVisual = IDHolder.GetGameObjectWithID(CardID);
        OneCardManager playedCard = CardVisual.GetComponent<OneCardManager>();

        if (playedCard.IsDisguisedCard)
        {
            foreach (int relationCardId in playedCard.RelationRealCardIds)
            {
                GameObject relationCard = IDHolder.GetGameObjectWithID(relationCardId);
                OneCardManager relationCardManager = relationCard.GetComponent<OneCardManager>();
                await relationCardManager.Owner.DisACardFromTreasure(relationCardId);
            }
            await PlayCardManager.Instance.ActivateEffect(playedCard);
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
        //先到pending状态
        await cardManager.ChangeOwnerAndLocation(player, CardLocation.Table);

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

        //改变位置为弃牌堆
        await cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
        //卡牌生效
        await PlayCardManager.Instance.ActivateEffect(playedCard);
    }


    public async Task UseASpellFromTreasure(int CardID)
    {
        GameObject CardVisual = IDHolder.GetGameObjectWithID(CardID);
        OneCardManager playedCard = CardVisual.GetComponent<OneCardManager>();
        CardAsset cardAsset = playedCard.CardAsset;

        switch (cardAsset.TypeOfCard)
        {
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
                    switch (cardAsset.SubTypeOfTip)
                    {
                        case TypesOfTip.DelayTips:
                            {
                                RemoveCard(CardVisual);

                                CardVisual.transform.SetParent(null);

                                Player player = GlobalSettings.Instance.FindPlayerByID(playedCard.TargetsPlayerIDs[0]);

                                //可视化加卡
                                player.PArea.JudgementVisual.AddCard(CardVisual);
                                //逻辑加卡
                                player.JudgementLogic.AddCard(playedCard.UniqueCardID);

                                var tcs = new TaskCompletionSource<bool>();
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
                                //先落到pending
                                await playedCard.ChangeOwnerAndLocation(player, CardLocation.Table);
                                //改到判定区
                                await playedCard.ChangeOwnerAndLocation(player, CardLocation.Judgement);
                            }
                            break;
                        case TypesOfTip.Default:
                            {
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
                                //到pending状态
                                await cardManager.ChangeOwnerAndLocation(player, CardLocation.Table);
                            }
                            break;
                    }

                }
                break;
        }

    }

    /// <summary>
    /// 移除卡牌到弃牌堆
    /// </summary>
    /// <param name="CardID"></param>
    public async Task DisCardFromTreasure(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        RemoveCard(card);

        card.transform.SetParent(null);

        var tcs = new TaskCompletionSource<bool>();
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            tcs.SetResult(true);

        });
        await tcs.Task;
        card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

        cardManager.CanBePlayedNow = false;
        await cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
    }
}
