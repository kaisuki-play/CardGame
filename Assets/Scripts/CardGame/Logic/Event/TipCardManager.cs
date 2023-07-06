using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class TipCardManager : MonoBehaviour
{
    public static TipCardManager Instance;
    public Player PlayCardOwner;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 无懈可击询问完毕，跳过锦囊效果
    /// </summary>
    public void SkipTipCard()
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if (cardManager != null)
        {
            //TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].RemoveAt(0);
            //if (TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count != 0)
            //{
            //    UseCardManager.Instance.HandleImpeccable(cardManager);
            //}
            //else
            //{
            //    UseCardManager.Instance.BackToWhoseTurn();
            //}
            TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID].RemoveAt(0);
            if (TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID].Count != 0)
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
    public async void ActiveTipCard()
    {
        HighlightManager.DisableAllCards();
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0]);
        if (cardManager != null)
        {
            switch (cardManager.CardAsset.SubTypeOfCard)
            {
                //南蛮入侵
                case SubTypeOfCards.Nanmanruqin:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        //Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        UseCardManager.Instance.NeedToPlaySlash(EventEnum.NeedToPlaySlash);
                    }
                    break;
                //万箭齐发
                case SubTypeOfCards.Wanjianqifa:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        //Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        UseCardManager.Instance.NeedToPlayJinkNew(EventEnum.SlashNeedToPlayJink);
                    }
                    break;
                //决斗
                case SubTypeOfCards.Juedou:
                    {
                        if (this.PlayCardOwner != null)
                        {
                            //Debug.Log("进来决斗了");
                            //if (this.PlayCardOwner.ID == TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0])
                            if (this.PlayCardOwner.ID == TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0])
                            {
                                //Debug.Log("出牌是目标，高亮决斗出牌人");
                                UseCardManager.Instance.NeedToPlaySlash(EventEnum.JuedouNeedToPlaySlash, cardManager.Owner);
                                //FixOpButton1Listener(cardManager.Owner);
                            }
                            else
                            {
                                //Debug.Log("出牌是决斗出牌人，高亮目标");
                                UseCardManager.Instance.NeedToPlaySlash(EventEnum.JuedouNeedToPlaySlash);
                                //FixOpButton1Listener(curTargetPlayer);
                            }
                        }
                        else
                        {
                            //Debug.Log("出牌是决斗出牌人，高亮目标");
                            UseCardManager.Instance.NeedToPlaySlash(EventEnum.JuedouNeedToPlaySlash);
                            //FixOpButton1Listener(curTargetPlayer);
                        }
                    }
                    break;
                //借刀杀人
                case SubTypeOfCards.Jiedaosharen:
                    UseCardManager.Instance.NeedToPlaySlash(EventEnum.JiedaoSharenNeedToPlaySlash, null, true);
                    break;
                //顺手牵羊
                case SubTypeOfCards.Shunshouqianyang:
                    ShowCardsSelection(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0]), CardSelectPanelType.Shunshouqianyang);
                    break;
                //过河拆桥
                case SubTypeOfCards.Guohechaiqiao:
                    ShowCardsSelection(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0]), CardSelectPanelType.GuoheChaiqiao);
                    break;
                //五谷丰登
                case SubTypeOfCards.Wugufengdeng:
                    {
                        if (GlobalSettings.Instance.CardSelectVisual.CardsOnHand.Count != 0)
                        {
                            GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
                        }
                        else
                        {
                            int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                            ShowCardsFromDeck(cardManager.TargetsPlayerIDs.Count, CardSelectPanelType.Wugufengdeng);
                        }
                    }
                    break;
                //桃园结义
                case SubTypeOfCards.Taoyuanjieyi:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        //Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        if (curTargetPlayer.Health < curTargetPlayer.PArea.Portrait.TotalHealth)
                        {
                            HealthManager.Instance.HealingEffect(1, curTargetPlayer);
                        }
                        await UseCardManager.Instance.FinishSettle();
                    }
                    break;
                //火攻
                case SubTypeOfCards.Huogong:
                    {
                        if (cardManager.ShownCard)
                        {
                            HighlightManager.DisACards(cardManager.Owner);
                            cardManager.Owner.ShowOp1Button = true;
                            cardManager.Owner.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                            cardManager.Owner.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                            {
                                cardManager.Owner.ShowOp1Button = false;
                                await UseCardManager.Instance.FinishSettle();
                            });
                        }
                        else
                        {
                            int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                            //Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                            HighlightManager.ShowACards(curTargetPlayer);
                        }
                    }
                    break;
                //铁索连环
                case SubTypeOfCards.Tiesuolianhuan:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        //Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        curTargetPlayer.IsInIronChain = !curTargetPlayer.IsInIronChain;
                        await UseCardManager.Instance.FinishSettle();
                    }
                    break;
                case SubTypeOfCards.Wuzhongshengyou:
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);
                        //Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        await curTargetPlayer.DrawSomeCards(2);
                        await UseCardManager.Instance.FinishSettle();
                    }
                    break;
                default:
                    Debug.Log("锦囊");
                    break;
            }
        }
        await TaskManager.Instance.DontAwait();
    }

    //public void FixOpButton1Listener(Player targetPlayer)
    //{
    //    targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
    //    targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
    //    {
    //        targetPlayer.ShowOp1Button = false;
    //        SettleManager.Instance.StartSettle(null, SpellAttribute.None, targetPlayer);
    //    });
    //}

    //出杀后借刀杀人取消当前目标
    public async Task RemoveJiedaoSharenTarget(OneCardManager cardManager)
    {
        (bool hasJiedaosharen, OneCardManager jiedaosharenCard) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
        if (hasJiedaosharen)
        {
            if (GlobalSettings.Instance.Table.CardsOnTable.Count > 0)
            {

                //结算完毕了借刀杀人的当前目标，需要移除
                TargetsManager.Instance.TargetsDic[jiedaosharenCard.UniqueCardID].RemoveAt(0);
                await GlobalSettings.Instance.Table.ClearAllCardsWithNoTargets();
            }
        }
    }

    /// <summary>
    /// 借刀杀人多个目标的时候
    /// </summary>
    public async Task JiedaoSharenNextTarget()
    {
        (bool hasJiedaosharen, OneCardManager jiedaosharenCard) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
        if (hasJiedaosharen)
        {
            if (GlobalSettings.Instance.Table.CardsOnTable.Count > 0)
            {
                //结算完毕了借刀杀人的当前目标，需要移除
                Debug.Log("借刀杀人还有没有目标" + TargetsManager.Instance.TargetsDic.ContainsKey(jiedaosharenCard.UniqueCardID));
                if (TargetsManager.Instance.TargetsDic.ContainsKey(jiedaosharenCard.UniqueCardID))
                {
                    if (TargetsManager.Instance.TargetsDic[jiedaosharenCard.UniqueCardID].Count > 0)
                    {
                        TargetsManager.Instance.TargetsDic[jiedaosharenCard.UniqueCardID].RemoveAt(0);
                    }
                }
                if (TargetsManager.Instance.TargetsDic[jiedaosharenCard.UniqueCardID].Count > 0)
                {
                    UseCardManager.Instance.HandleImpeccable(jiedaosharenCard);
                }
                else
                {
                    Debug.Log("回到玩家回合");
                    UseCardManager.Instance.BackToWhoseTurn();
                }
            }
            else
            {
                //UseCardManager.Instance.BackToWhoseTurn();
                Debug.Log("回到玩家回合");
                UseCardManager.Instance.BackToWhoseTurn();
            }
            await TaskManager.Instance.ReturnException("走借刀杀人分支");
        }
        else
        {
            await TaskManager.Instance.DontAwait();
        }


    }

    public async void GiveJiedaoSharenWeapon()
    {
        (bool hasJiedaosharen, OneCardManager jiedaosharenCard) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
        //int targetId = TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)][0];
        int targetId = TargetsManager.Instance.TargetsDic[jiedaosharenCard.UniqueCardID][0];
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);
        await targetPlayer.GiveWeaponToTargetWithCardType(jiedaosharenCard.Owner);
        //下一个目标
        try
        {
            await JiedaoSharenNextTarget();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// 从堆顶展示牌
    /// </summary>
    /// <param name="cardsNumber"></param>
    /// <param name="targetCardsPanelType"></param>
    public void ShowCardsFromDeck(int cardsNumber, CardSelectPanelType targetCardsPanelType)
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

    /// <summary>
    /// 展示目标的牌供选择
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="targetCardsPanelType"></param>
    public void ShowCardsSelection(Player targetPlayer, CardSelectPanelType targetCardsPanelType)
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
