using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetCardsPanelType
{
    GuoheChaiqiao,
    Wugufengdeng,
    Shunshouqianyang,
    DisHandCard
}

public class TargetCardsPanel : MonoBehaviour
{
    public TargetCardsPanelType PanelType;

    public SameDistanceChildren HandSlots;
    public SameDistanceChildren JudgementSlots;
    public SameDistanceChildren EquipmentSlots;

    public List<GameObject> CardsOnHand = new List<GameObject>();
    public List<GameObject> CardsOnJudgement = new List<GameObject>();
    public List<GameObject> CardsOnEquipment = new List<GameObject>();

    public void Dismiss()
    {
        DisAllCards();
        this.gameObject.SetActive(false);
    }

    // 顺手牵羊、过河拆桥、寒冰剑 加牌
    public void AddHandCardsAtIndex(OneCardManager cardManager, int index)
    {
        SameDistanceChildren slots;
        List<GameObject> cardlist;
        switch (cardManager.CardLocation)
        {
            case CardLocation.Hand:
                slots = HandSlots;
                cardlist = CardsOnHand;
                break;
            case CardLocation.Judgement:
                slots = JudgementSlots;
                cardlist = CardsOnJudgement;
                break;
            case CardLocation.Equipment:
                slots = EquipmentSlots;
                cardlist = CardsOnEquipment;
                break;
            default:
                slots = HandSlots;
                cardlist = CardsOnHand;
                break;
        }
        // create a new card from prefab
        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.TargetAllCardsPrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CardAsset = cardManager.CardAsset;
        manager.ReadCardFromAsset();
        manager.UniqueCardID = cardManager.UniqueCardID;
        manager.Owner = cardManager.Owner;

        // parent a new creature gameObject to table slots
        card.transform.SetParent(slots.transform);

        // add a new creature to the list
        cardlist.Insert(index, card);

        // let this card know about its position
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
        w.BringToFront();

        // add our unique ID to this creature
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = -1;

        // after a new creature is added update placing of all the other creatures
        CardUtils.Instance.ShiftSlotsGameObjectAccordingToNumberOfCreatures(slots, cardlist);
        CardUtils.Instance.PlaceCreaturesOnNewSlots(slots, cardlist);

        // end command execution
        Command.CommandExecutionComplete();
    }

    public void DisAllCards()
    {
        foreach (GameObject card in CardsOnHand)
        {
            Destroy(card);
        }
        foreach (GameObject card in CardsOnJudgement)
        {
            Destroy(card);
        }
        foreach (GameObject card in CardsOnEquipment)
        {
            Destroy(card);
        }
        CardsOnHand.Clear();
        CardsOnJudgement.Clear();
        CardsOnEquipment.Clear();
    }
}
