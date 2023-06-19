using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class JudgementVisual : MonoBehaviour
{
    public SameDistanceChildren Slots;
    private List<GameObject> _cardsInJudgement = new List<GameObject>();

    // add a new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        _cardsInJudgement.Insert(0, card);

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
        _cardsInJudgement.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    /// <summary>
    /// 移除卡牌到弃牌堆
    /// </summary>
    /// <param name="CardID"></param>
    public void DisCardFromJudgement(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        RemoveCard(card);

        card.transform.SetParent(null);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.Insert(0f, card.transform.DORotate(new Vector3(0, 0, 0), 1f));
        s.OnComplete(() =>
        {
            card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            cardManager.CanBePlayedNow = false;
            cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
        });
    }


    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (_cardsInJudgement.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[_cardsInJudgement.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // shift all cards to their new Slots
    public void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in _cardsInJudgement)
        {
            try
            {
                // tween this card to a new Slot
                g.transform.DOLocalMoveX(Slots.Children[_cardsInJudgement.IndexOf(g)].transform.localPosition.x, 0.3f);
            }
            catch (Exception ex)
            {
                Debug.LogError("other error is:" + ex.Message);
            }

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = _cardsInJudgement.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }
}
