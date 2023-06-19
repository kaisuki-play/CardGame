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
    public void StartSettle(OneCardManager cardManager = null)
    {

        BeforeDamage(cardManager);
    }

    //伤害前
    public void BeforeDamage(OneCardManager cardManager = null)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (cardManager != null)
        {
            switch (cardManager.CardAsset.TypeOfCard)
            {
                case TypesOfCards.Base:
                    {
                        switch (cardManager.CardAsset.SubTypeOfCard)
                        {
                            case SubTypeOfCards.Slash:
                            case SubTypeOfCards.FireSlash:
                            case SubTypeOfCards.ThunderSlash:
                                CalculateDamage();
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
                                CalculateDamage();
                                break;
                            case SubTypeOfCards.Juedou:
                                if (TipCardManager.Instance.PlayCardOwner == null)
                                {
                                    CalculateDamage();
                                }
                                else
                                {
                                    Player player1 = cardManager.Owner;
                                    Player player2 = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
                                    if (TipCardManager.Instance.PlayCardOwner.ID == player1.ID)
                                    {
                                        CalculateDamage(player2);
                                    }
                                    else
                                    {
                                        CalculateDamage(player1);
                                    }
                                }
                                break;
                            case SubTypeOfCards.Huogong:
                                CalculateDamage();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case TypesOfCards.DelayTips:
                    if (cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Thunder)
                    {
                        CalculateDamage(TurnManager.Instance.whoseTurn, cardManager, 3);
                    }
                    break;
            }

        }
    }


    //计算伤害
    public async void CalculateDamage(Player curTargetPlayer = null, OneCardManager cardManager = null, int originDamage = 0)
    {
        if (cardManager == null)
        {
            cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (cardManager != null)
        {

            if (originDamage == 0)
            {
                originDamage = cardManager.CardAsset.SpecialSpellAmount;
            }

            if (curTargetPlayer == null)
            {
                curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
            }

            //酒杀钩子
            originDamage = AnalepticHook(originDamage);

            //结算伤害
            HealthManager.Instance.DamageEffect(originDamage, curTargetPlayer);

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
            AfterDamage(cardManager);

        }
    }

    //伤害后
    public void AfterDamage(OneCardManager cardManager = null)
    {
        //TODO 判断是否死亡，如果死亡则继续铁索
        //TODO 无属性的话不需要响应铁索连环
        HandleIronChain(cardManager);
    }

    //铁索连环结算
    public void HandleIronChain(OneCardManager cardManager = null)
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
