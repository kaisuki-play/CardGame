using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class HeroHeadVisual : MonoBehaviour
{
    public SameDistanceChildren Slots;
    private List<GameObject> _cardsInHeroHead = new List<GameObject>();

    // add a new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        _cardsInHeroHead.Insert(0, card);

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
        _cardsInHeroHead.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    /// <summary>
    /// 移除卡牌到弃牌堆
    /// </summary>
    /// <param name="CardID"></param>
    public async Task DisCardFromHeroHead(int CardID)
    {

        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        RemoveCard(card);

        card.transform.SetParent(null);

        var tcs = new TaskCompletionSource<bool>();

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.Insert(0f, card.transform.DORotate(new Vector3(0, 0, 0), 1f));
        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });
        await tcs.Task;

        card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        cardManager.CanBePlayedNow = false;

        //牌到弃牌堆
        await cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
    }


    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (_cardsInHeroHead.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[_cardsInHeroHead.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // shift all cards to their new Slots
    public void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in _cardsInHeroHead)
        {
            try
            {
                // tween this card to a new Slot
                g.transform.DOLocalMoveX(Slots.Children[_cardsInHeroHead.IndexOf(g)].transform.localPosition.x, 0.3f);
            }
            catch (Exception ex)
            {
                Debug.LogError("other error is:" + ex.Message);
            }

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = _cardsInHeroHead.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    public void PutCardOnHero(GameObject card)
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
}
