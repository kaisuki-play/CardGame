using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

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
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        _cardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(Slots.transform);

        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        cardManager.ChangeOwnerAndLocation(GlobalSettings.Instance.Players[this.Owner], CardLocation.Hand);

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

    public void DrawACard(GameObject card)
    {
        AddCard(card);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();
        w.Slot = 0;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        // displace the card so that we can select it in the scene easier.
        s.Append(card.transform.DOLocalMove(Slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTime));

        s.OnComplete(() =>
        {
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();

            Command.CommandExecutionComplete();
        });
    }


    // PLAYING SPELLS

    public void DisCardFromHand(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        Command.CommandExecutionComplete();
        RemoveCard(card);

        card.transform.SetParent(null);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(DisCardPreviewSpot.position, 1f));
        s.Insert(0f, card.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(2f);
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            //Command.CommandExecutionComplete();
            //Destroy(card);
            card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            cardManager.CanBePlayedNow = false;
            cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
        });
    }

    // 2 Overloaded method to show a spell played from hand
    public void PlayASpellFromHand(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        PlayASpellFromHand(card);
    }

    public void UseASpellFromHand(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        PlayASpellFromHand(card);
    }

    public void PlayASpellFromHand(GameObject CardVisual)
    {
        Command.CommandExecutionComplete();
        CardAsset cardAsset = CardVisual.GetComponent<OneCardManager>().CardAsset;

        switch (cardAsset.TypeOfCard)
        {
            case TypesOfCards.Equipment:
                {
                    RemoveCard(CardVisual);

                    CardVisual.transform.SetParent(null);

                    Player player = GlobalSettings.Instance.Players[Owner];

                    player.PArea.EquipmentVisaul.AddCard(CardVisual);

                    Sequence s = DOTween.Sequence();
                    s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
                    s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
                    s.AppendInterval(2f);
                    s.Append(CardVisual.transform.DOLocalMove(player.PArea.EquipmentVisaul.Slots.Children[0].transform.localPosition, 1f));
                    s.OnComplete(() =>
                    {
                        //Command.CommandExecutionComplete();
                        //Destroy(CardVisual);

                        CardVisual.transform.SetParent(player.PArea.EquipmentVisaul.Slots.transform);

                        OneCardManager cardManager = CardVisual.GetComponent<OneCardManager>();
                        cardManager.CanBePlayedNow = false;
                        cardManager.ChangeOwnerAndLocation(player, CardLocation.Equipment);
                    });
                }
                break;
            case TypesOfCards.Base:
            case TypesOfCards.Tips:
                {
                    RemoveCard(CardVisual);

                    CardVisual.transform.SetParent(null);

                    Sequence s = DOTween.Sequence();
                    s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
                    s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
                    s.AppendInterval(2f);
                    s.Append(CardVisual.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
                    s.OnComplete(() =>
                    {
                        //Command.CommandExecutionComplete();
                        //Destroy(CardVisual);
                        CardVisual.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

                        OneCardManager cardManager = CardVisual.GetComponent<OneCardManager>();
                        cardManager.CanBePlayedNow = false;
                        cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
                    });
                }
                break;
        }


    }


}
