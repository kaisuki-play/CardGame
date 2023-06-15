using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnCard : ClickActions
{
    private int _savedHandSlot;
    private WhereIsTheCardOrCreature _whereIsCard;
    private OneCardManager _manager;

    void Awake()
    {
        _whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        _manager = GetComponent<OneCardManager>();
    }

    public override bool CanClick
    {
        get
        {
            return true;
        }
    }

    private void Update()
    {

    }

    public override void OnCardClick()
    {
        Debug.Log("On Card Click");
        GameObject originCard = IDHolder.GetGameObjectWithID(_manager.UniqueCardID);
        OneCardManager originCardManager = originCard.GetComponent<OneCardManager>();
        switch (GlobalSettings.Instance.TargetCardsPanel.PanelType)
        {
            //顺手牵羊
            case TargetCardsPanelType.Shunshouqianyang:
                {
                    (bool hasShunshouqianyang, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Shunshouqianyang);
                    if (hasShunshouqianyang)
                    {
                        originCardManager.Owner.GiveCardToTarget(cardManager.Owner, originCardManager);
                        GlobalSettings.Instance.Table.ClearCardsFromLast();
                        GlobalSettings.Instance.TargetCardsPanel.Dismiss();
                        HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                    }
                }
                break;
            case TargetCardsPanelType.GuoheChaiqiao:
                {
                    (bool hasGuohechaiqiao, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Guohechaiqiao);
                    if (hasGuohechaiqiao)
                    {
                        originCardManager.Owner.PArea.HandVisual.DisCardFromHand(originCardManager.UniqueCardID);
                        GlobalSettings.Instance.Table.ClearCardsFromLast();
                        GlobalSettings.Instance.TargetCardsPanel.Dismiss();
                        HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                    }
                }
                break;
        }
    }
}
