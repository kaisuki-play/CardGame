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
    public async Task StartSettle(OneCardManager cardManager = null, SpellAttribute spellAttribute = SpellAttribute.None, Player targetPlayer = null, int originalDamage = 0, bool isFromIronChain = false)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
            spellAttribute = cardManager.CardAsset.SpellAttribute;
        }
        if (cardManager != null)
        {
            Debug.Log("伤害属性" + cardManager.CardAsset.SpellAttribute);
        }
        await BeforeDamage(cardManager, spellAttribute, targetPlayer, originalDamage, isFromIronChain);
    }

    //伤害前
    public async Task BeforeDamage(OneCardManager cardManager = null, SpellAttribute spellAttribute = SpellAttribute.None, Player targetPlayer = null, int originalDamage = 0, bool isFromIronChain = false)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (cardManager != null)
        {
            if (targetPlayer == null)
            {
                targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0]);
            }
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
            await CalculateDamage(cardManager, spellAttribute, targetPlayer, originalDamage, isFromIronChain);

            //switch (cardManager.CardAsset.TypeOfCard)
            //{
            //    case TypesOfCards.Base:
            //        {
            //            switch (cardManager.CardAsset.SubTypeOfCard)
            //            {
            //                case SubTypeOfCards.Slash:
            //                case SubTypeOfCards.FireSlash:
            //                case SubTypeOfCards.ThunderSlash:

            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        break;
            //    case TypesOfCards.Tips:
            //        {
            //            switch (cardManager.CardAsset.SubTypeOfCard)
            //            {
            //                case SubTypeOfCards.Nanmanruqin:
            //                case SubTypeOfCards.Wanjianqifa:
            //                    CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
            //                    break;
            //                case SubTypeOfCards.Juedou:
            //                    //TODO 决斗的这些放在第六步记录
            //                    if (TipCardManager.Instance.PlayCardOwner == null)
            //                    {
            //                        CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
            //                    }
            //                    else
            //                    {
            //                        Player player1 = cardManager.Owner;
            //                        Player player2 = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
            //                        if (TipCardManager.Instance.PlayCardOwner.ID == player1.ID)
            //                        {
            //                            CalculateDamage(cardManager, player2, originalDamage, isFromIronChain);
            //                        }
            //                        else
            //                        {
            //                            CalculateDamage(cardManager, player1, originalDamage, isFromIronChain);
            //                        }
            //                    }
            //                    break;
            //                case SubTypeOfCards.Huogong:
            //                    CalculateDamage(cardManager, targetPlayer, originalDamage, isFromIronChain);
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        break;
            //    case TypesOfCards.DelayTips:
            //        if (cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Thunder)
            //        {
            //            CalculateDamage(cardManager, TurnManager.Instance.whoseTurn, 3, isFromIronChain);
            //        }
            //        break;
            //}

        }
    }


    //计算伤害
    public async Task CalculateDamage(OneCardManager cardManager = null, SpellAttribute spellAttribute = SpellAttribute.None, Player curTargetPlayer = null, int originalDamage = 1, bool isFromIronChain = false)
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
                curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0]);
            }

            //计算最终伤害
            originalDamage = await DamageCalManager.FinalDamage(originalDamage, cardManager, spellAttribute, curTargetPlayer);

            //结算伤害
            await HealthManager.Instance.DamageEffect(originalDamage, curTargetPlayer);

            SettleManager.Instance.DamageSourceId = cardManager.Owner.ID;

            //是否进入濒死流程
            if (curTargetPlayer.Health <= 0)
            {
                Debug.Log("中断流程，进入濒死流程");
                DyingManager.Instance.EnterDying(cardManager.Owner, curTargetPlayer);
                //阻塞不往下执行
                await TaskManager.Instance.BlockTask(TaskType.DyingTask);
            }

            Debug.Log("没有濒死继续往下执行");

            await AfterDamage(cardManager, spellAttribute, curTargetPlayer, originalDamage, isFromIronChain);
        }
    }

    //伤害后
    public async Task AfterDamage(OneCardManager cardManager = null, SpellAttribute spellAttribute = SpellAttribute.None, Player targetPlayer = null, int originalDamage = 1, bool isFromIronChain = false)
    {
        //判断是否死亡，如果死亡则继续铁索
        if (!targetPlayer.IsDead)
        {
            await SkillManager.StartAfterDamage(cardManager, targetPlayer, isFromIronChain);
            Debug.Log("-----------------------------------------slash settle after damage--------------------------");
        }

        //TODO 无属性的话不需要响应铁索连环
        await HandleIronChain(cardManager, spellAttribute, targetPlayer, originalDamage, isFromIronChain);
    }

    //铁索连环结算
    public async Task HandleIronChain(OneCardManager cardManager = null, SpellAttribute spellAttribute = SpellAttribute.None, Player targetPlayer = null, int originalDamage = 1, bool isFromIronChain = false)
    {
        //TODO 铁索连环结算
        if (targetPlayer.IsInIronChain && spellAttribute != SpellAttribute.None)
        {
            targetPlayer.IsInIronChain = false;
            if (targetPlayer.IsThereAnyOneIsInIronChain())
            {
                await SettleManager.Instance.StartSettle(cardManager, spellAttribute, targetPlayer.OtherIronChainPlayer, originalDamage, true);
                return;
            }
        }
        Debug.Log("没铁索了，继续往下结算");
        //TODO 自己回合死亡，回合需要传到下一个玩家手里
        if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips && cardManager.CardAsset.SubTypeOfTip == TypesOfTip.DelayTips)
        {
            TaskManager.Instance.DelayTipTask.SetResult(true);
            //DelayTipManager.HandleDelayTip(TurnManager.Instance.whoseTurn);
        }
        else
        {
            if (cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.SkillDamage)
            {
                Destroy(cardManager.gameObject);
            }
            else
            {
                await UseCardManager.Instance.FinishSettle();
            }
        }
    }

}
