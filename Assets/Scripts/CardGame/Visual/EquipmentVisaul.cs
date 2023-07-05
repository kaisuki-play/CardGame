using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;

public class EquipmentVisaul : MonoBehaviour
{
    public SameDistanceChildren Slots;
    private List<GameObject> _cardsInEquipment = new List<GameObject>();

    // add a new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        _cardsInEquipment.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(Slots.transform);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        // remove a card from the list
        _cardsInEquipment.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    /// <summary>
    /// 装备上装备
    /// </summary>
    /// <param name="cardId"></param>
    public async Task EquipWithCard(int cardId, Player player)
    {
        var tcs = new TaskCompletionSource<bool>();

        GameObject card = IDHolder.GetGameObjectWithID(cardId);
        OneCardManager cardManager = card.GetComponent<OneCardManager>();

        AddCard(card);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(player.PArea.HandVisual.PlayPreviewSpot.position, 1f));
        s.Insert(0f, card.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(2f);
        s.Append(card.transform.DOLocalMove(player.PArea.EquipmentVisaul.Slots.Children[0].transform.localPosition, 1f));
        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });

        await tcs.Task;

        card.transform.SetParent(player.PArea.EquipmentVisaul.Slots.transform);

        Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // 设置为 (2, 2, 2) 的比例
        card.transform.localScale = newScale;

        cardManager.CanBePlayedNow = false;

        await cardManager.ChangeOwnerAndLocation(player, CardLocation.Table);

        //到了装备区
        await cardManager.ChangeOwnerAndLocation(player, CardLocation.Equipment);
    }

    /// <summary>
    /// 移除卡牌到弃牌堆
    /// </summary>
    /// <param name="CardID"></param>
    public async Task DisCardFromEquipment(int CardID)
    {
        var tcs = new TaskCompletionSource<bool>();

        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        RemoveCard(card);

        card.transform.SetParent(null);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });
        await tcs.Task;
        card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        cardManager.CanBePlayedNow = false;

        //到弃牌区
        await cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
    }

    // gives player a new card from a given position
    //public void GivePlayerAJudgementCard(CardAsset c, int UniqueID)
    //{
    //    // Instantiate a card depending on its type
    //    GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, Slots.Children[0].transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
    //    card.SetActive(false);

    //    // apply the look of the card based on the info from CardAsset
    //    OneCardManager manager = card.GetComponent<OneCardManager>();
    //    manager.SetCardAssetA(c);
    //    manager.ReadCardFromAsset();


    //    card.transform.SetParent(Slots.Children[0].transform);

    //    // pass this card to HandVisual class
    //    AddCard(card);

    //    // Bring card to front while it travels from draw spot to hand
    //    WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
    //    w.BringToFront();
    //    w.Slot = 0;

    //    // pass a unique ID to this card.
    //    IDHolder id = card.AddComponent<IDHolder>();
    //    id.UniqueID = UniqueID;

    //    ChangeLastCardStatusToInHand(card, w);
    //}

    // this method will be called when the card arrived to hand 
    void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCardOrCreature w)
    {
        card.SetActive(true);

        // set correct sorting order
        w.SetHandSortingOrder();

        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();

        // end command execution for DrawACArdCommand
        Command.CommandExecutionComplete();
    }

    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (_cardsInEquipment.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[_cardsInEquipment.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // shift all cards to their new Slots
    public void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in _cardsInEquipment)
        {
            try
            {
                // tween this card to a new Slot
                g.transform.DOLocalMoveX(Slots.Children[_cardsInEquipment.IndexOf(g)].transform.localPosition.x, 0.3f);
            }
            catch (Exception ex)
            {
                Debug.LogError("other error is:" + ex.Message);
            }

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = _cardsInEquipment.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }
}
