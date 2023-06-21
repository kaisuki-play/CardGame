using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SettleManager : MonoBehaviour
{
    public static SettleManager Instance;
    public int DamageSourceId;
    private void Awake()
    {
        Instance = this;
    }

    //开始结算
    public void StartSettle(OneCardManager cardManager = null, Player targetPlayer = null, int originalDamage = 0, bool isFromIronChain = false)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (cardManager != null)
        {
            Debug.Log("伤害属性" + cardManager.CardAsset.SpellAttribute);
        }
        BeforeDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
    }

    //伤害前
    public async void BeforeDamage(OneCardManager cardManager = null, Player targetPlayer = null, int originalDamage = 0, bool isFromIronChain = false)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (cardManager != null)
        {
            if (targetPlayer == null)
            {
                targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
            }
            switch (cardManager.CardAsset.TypeOfCard)
            {
                case TypesOfCards.Base:
                    {
                        switch (cardManager.CardAsset.SubTypeOfCard)
                        {
                            case SubTypeOfCards.Slash:
                            case SubTypeOfCards.FireSlash:
                            case SubTypeOfCards.ThunderSlash:
                                try
                                {
                                    await SkillManager.BeforeCalculateDamage(cardManager, targetPlayer);
                                }
                                catch (Exception ex)
                                {
                                    Debug.Log("异常为：" + ex);
                                    return;
                                }
                                Debug.Log("-----------------------------------------slash settle before calculate damage--------------------------");
                                CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case TypesOfCards.Tips:
                    {
                        switch (cardManager.CardAsset.SubTypeOfCard)
                        {
                            case SubTypeOfCards.Nanmanruqin:
                            case SubTypeOfCards.Wanjianqifa:
                                CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
                                break;
                            case SubTypeOfCards.Juedou:
                                if (TipCardManager.Instance.PlayCardOwner == null)
                                {
                                    CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
                                }
                                else
                                {
                                    Player player1 = cardManager.Owner;
                                    Player player2 = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
                                    if (TipCardManager.Instance.PlayCardOwner.ID == player1.ID)
                                    {
                                        CalculateDamage(cardManager, player2, originalDamage, isFromIronChain);
                                    }
                                    else
                                    {
                                        CalculateDamage(cardManager, player1, originalDamage, isFromIronChain);
                                    }
                                }
                                break;
                            case SubTypeOfCards.Huogong:
                                CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case TypesOfCards.DelayTips:
                    if (cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Thunder)
                    {
                        CalculateDamage(cardManager, TurnManager.Instance.whoseTurn, 3, isFromIronChain);
                    }
                    break;
            }

        }
    }


    //计算伤害
    public async void CalculateDamage(OneCardManager cardManager = null, Player curTargetPlayer = null, int originalDamage = 1, bool isFromIronChain = false)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (cardManager != null)
        {

            if (originalDamage == 0)
            {
                originalDamage = cardManager.CardAsset.SpecialSpellAmount;
            }

            if (curTargetPlayer == null)
            {
                curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
            }

            //酒杀钩子
            originalDamage = AnalepticHook(originalDamage);

            //触发的计算伤害的
            originalDamage = SkillManager.StartCalculateDamage(cardManager, curTargetPlayer, originalDamage);

            //结算伤害
            await HealthManager.Instance.DamageEffect(originalDamage, curTargetPlayer);

            SettleManager.Instance.DamageSourceId = cardManager.Owner.ID;

            //是否进入濒死流程
            if (curTargetPlayer.Health <= 0)
            {
                Debug.Log("中断流程，进入濒死流程");
                DyingManager.Instance.EnterDying(curTargetPlayer);
                //阻塞不往下执行
                await TaskManager.Instance.BlockTask(TaskType.DyingTask);
            }

            Debug.Log("没有濒死继续往下执行");
            AfterDamage(cardManager, curTargetPlayer, originalDamage, isFromIronChain);

        }
    }

    //伤害后
    public async void AfterDamage(OneCardManager cardManager = null, Player targetPlayer = null, int originalDamage = 1, bool isFromIronChain = false)
    {
        await SkillManager.StartAfterDamage(cardManager, targetPlayer, isFromIronChain);
        Debug.Log("-----------------------------------------slash settle after damage--------------------------");
        //TODO 判断是否死亡，如果死亡则继续铁索
        //TODO 无属性的话不需要响应铁索连环
        HandleIronChain(cardManager, targetPlayer, originalDamage, isFromIronChain);
    }

    //铁索连环结算
    public void HandleIronChain(OneCardManager cardManager = null, Player targetPlayer = null, int originalDamage = 1, bool isFromIronChain = false)
    {
        //TODO 铁索连环结算
        //TODO 自己回合死亡，回合需要传到下一个玩家手里
        if (cardManager.CardAsset.TypeOfCard == TypesOfCards.DelayTips)
        {
            TaskManager.Instance.DelayTipTask.SetResult(true);
            //DelayTipManager.HandleDelayTip(TurnManager.Instance.whoseTurn);
        }
        else
        {
            UseCardManager.Instance.FinishSettle();
        }
    }

    //酒杀的钩子
    public int AnalepticHook(int originDamage)
    {
        if (GlobalSettings.Instance.Table.CardsOnTable.Count == 0)
        {
            return originDamage;
        }
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if ((cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
            || cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash
            || cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash)
            && CounterManager.Instance.UsedAnalepticThisTurn
            && CounterManager.Instance.AnalepticWorkCount == 0)
        {
            CounterManager.Instance.AnalepticWorkCount++;
            return originDamage + 1;
        }
        return originDamage;
    }
}
