using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCardManager : MonoBehaviour
{
    public static TipCardManager Instance;
    public Player PlayCardOwner;
    private void Awake()
    {
        Instance = this;
    }

    public void SkipTipCard()
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if (cardManager != null)
        {
            TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].RemoveAt(0);
            //cardManager.TargetsPlayerIDs.RemoveAt(0);
            Debug.Log("现在还有几张牌的目标~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + TargetsManager.Instance.Targets.Count);
            if (TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count != 0)
            {
                UseCardManager.Instance.HandleImpeccable(cardManager);
            }
            else
            {
                UseCardManager.Instance.BackToWhoseTurn();
            }
        }
    }

    /// <summary>
    /// 无懈可击询问完毕，触发锦囊效果阶段
    /// </summary>
    public void ActiveTipCard()
    {
        HighlightManager.DisableAllCards();
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if (cardManager != null)
        {
            switch (cardManager.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Nanmanruqin:
                    UseCardManager.Instance.NeedToPlaySlash();
                    break;
                case SubTypeOfCards.Wanjianqifa:
                    UseCardManager.Instance.NeedToPlayJink();
                    break;
                case SubTypeOfCards.Juedou:
                    if (this.PlayCardOwner != null)
                    {
                        Debug.Log("进来决斗了");
                        if (this.PlayCardOwner.ID == TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0])
                        {
                            Debug.Log("出牌是目标，高亮决斗出牌人");
                            UseCardManager.Instance.NeedToPlaySlash(cardManager.Owner);
                        }
                        else
                        {
                            Debug.Log("出牌是决斗出牌人，高亮目标");
                            UseCardManager.Instance.NeedToPlaySlash();
                        }
                    }
                    else
                    {
                        Debug.Log("出牌是决斗出牌人，高亮目标");
                        UseCardManager.Instance.NeedToPlaySlash();
                    }
                    break;
                case SubTypeOfCards.Jiedaosharen:
                    Debug.Log("借刀杀人出杀设定");
                    UseCardManager.Instance.NeedToPlaySlash(null, true);
                    break;
                case SubTypeOfCards.Shunshouqianyang:
                    ShowTargetAllCards(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]), TargetCardsPanelType.Shunshouqianyang);
                    break;
                case SubTypeOfCards.Guohechaiqiao:
                    ShowTargetAllCards(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]), TargetCardsPanelType.GuoheChaiqiao);
                    break;
                case SubTypeOfCards.Wugufengdeng:
                default:
                    Debug.Log("锦囊");
                    //SkipTipCard();
                    break;
            }
        }
    }

    /// <summary>
    /// 借刀杀人多个目标的时候
    /// </summary>
    public void JiedaoSharenNextTarget()
    {
        OneCardManager jiedaosharenCard = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen).Item2;
        if (GlobalSettings.Instance.Table.CardsOnTable.Count > 0)
        {
            TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)].RemoveAt(0);
            if (TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)].Count > 0)
            {
                UseCardManager.Instance.HandleImpeccable(jiedaosharenCard);
            }
            else
            {
                UseCardManager.Instance.BackToWhoseTurn();
            }
        }
        else
        {
            UseCardManager.Instance.BackToWhoseTurn();
        }

    }

    public void GiveJiedaoSharenWeapon()
    {
        (bool hasJiedaosharen, OneCardManager jiedaosharenCard) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
        int targetId = TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)][0];
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);
        targetPlayer.GiveWeaponToTargetWithCardType(jiedaosharenCard.Owner);
        //下一个目标
        JiedaoSharenNextTarget();
    }

    public void ShowTargetAllCards(Player targetPlayer, TargetCardsPanelType targetCardsPanelType)
    {
        GlobalSettings.Instance.TargetCardsPanel.PanelType = targetCardsPanelType;
        GlobalSettings.Instance.TargetCardsPanel.gameObject.SetActive(true);
        for (int i = 0; i < targetPlayer.Hand.CardsInHand.Count; i++)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.TargetCardsPanel.AddHandCardsAtIndex(cardManager, i);
        }
        for (int i = 0; i < targetPlayer.JudgementLogic.CardsInJudgement.Count; i++)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.JudgementLogic.CardsInJudgement[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.TargetCardsPanel.AddHandCardsAtIndex(cardManager, i);
        }
        for (int i = 0; i < targetPlayer.EquipmentLogic.CardsInEquipment.Count; i++)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.TargetCardsPanel.AddHandCardsAtIndex(cardManager, i);
        }
    }
}
