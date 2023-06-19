using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

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
                    ShowCardsSelection(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]), TargetCardsPanelType.Shunshouqianyang);

                    //ShowTargetAllCards(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]), TargetCardsPanelType.Shunshouqianyang);
                    break;
                case SubTypeOfCards.Guohechaiqiao:
                    ShowCardsSelection(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]), TargetCardsPanelType.GuoheChaiqiao);

                    //ShowTargetAllCards(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]), TargetCardsPanelType.GuoheChaiqiao);
                    break;
                case SubTypeOfCards.Wugufengdeng:
                    {
                        if (GlobalSettings.Instance.CardSelectVisual.CardsOnHand.Count != 0)
                        {
                            GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
                        }
                        else
                        {
                            int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                            ShowCardsFromDeck(TargetsManager.Instance.Targets[cardIndex].Count, TargetCardsPanelType.Wugufengdeng);
                        }
                    }
                    break;
                case SubTypeOfCards.Taoyuanjieyi:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        if (curTargetPlayer.Health < curTargetPlayer.PArea.Portrait.TotalHealth)
                        {
                            HealthManager.Instance.HealingEffect(1, curTargetPlayer);
                        }
                        UseCardManager.Instance.FinishSettle();
                    }
                    break;
                case SubTypeOfCards.Huogong:
                    {
                        if (cardManager.ShownCard)
                        {
                            Debug.Log("需要火攻");
                            HighlightManager.DisACards(cardManager.Owner);
                            cardManager.Owner.ShowOp1Button = true;
                            cardManager.Owner.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                            cardManager.Owner.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                            {
                                cardManager.Owner.ShowOp1Button = false;
                                UseCardManager.Instance.FinishSettle();
                            });
                        }
                        else
                        {
                            int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                            Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                            HighlightManager.ShowACards(curTargetPlayer);
                        }
                    }
                    break;
                case SubTypeOfCards.Tiesuolianhuan:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        curTargetPlayer.IsInIronChain = !curTargetPlayer.IsInIronChain;
                        UseCardManager.Instance.FinishSettle();
                    }
                    break;
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

    /// <summary>
    /// 从堆顶展示牌
    /// </summary>
    /// <param name="cardsNumber"></param>
    /// <param name="targetCardsPanelType"></param>
    public void ShowCardsFromDeck(int cardsNumber, TargetCardsPanelType targetCardsPanelType)
    {
        if (!GlobalSettings.Instance.CardSelectVisual.gameObject.activeSelf)
        {
            GlobalSettings.Instance.CardSelectVisual.PanelType = targetCardsPanelType;
            GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);

            for (int i = cardsNumber - 1; i >= 0; i--)
            {
                GameObject card = GlobalSettings.Instance.PDeck.DeckCards[i];
                OneCardManager cardManager = card.GetComponent<OneCardManager>();
                GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
            }
        }
    }

    /// <summary>
    /// 展示目标的牌供选择
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="targetCardsPanelType"></param>
    public void ShowCardsSelection(Player targetPlayer, TargetCardsPanelType targetCardsPanelType)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = targetCardsPanelType;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = targetPlayer.JudgementLogic.CardsInJudgement.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.JudgementLogic.CardsInJudgement[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = targetPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }
}
