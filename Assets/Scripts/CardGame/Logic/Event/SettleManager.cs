using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SettleManager : MonoBehaviour
{
    public static SettleManager Instance;
    public int DamageSourceId;
    private void Awake()
    {
        Instance = this;
    }

    //开始结算
    public void StartSettle()
    {

        BeforeDamage();
    }

    //伤害前
    public void BeforeDamage()
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
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
                            default:
                                break;
                        }
                    }
                    break;
            }

        }
    }


    //计算伤害
    public async void CalculateDamage(Player curTargetPlayer = null)
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if (cardManager != null)
        {

            int originDamage = cardManager.CardAsset.SpecialSpellAmount;

            if (curTargetPlayer == null)
            {
                curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
            }

            //结算伤害
            HealthManager.Instance.DamageEffect(originDamage, curTargetPlayer);

            SettleManager.Instance.DamageSourceId = cardManager.Owner.ID;

            //是否进入濒死流程
            if (curTargetPlayer.Health <= 0)
            {
                Debug.Log("中断流程，进入濒死流程");
                DyingManager.Instance.EnterDying(curTargetPlayer);
                //阻塞不往下执行
                await TaskManager.Instance.BlockTask();
            }

            Debug.Log("没有濒死继续往下执行");
            AfterDamage();

        }
    }

    //伤害后
    public void AfterDamage()
    {
        //TODO 判断是否死亡，如果死亡则继续铁索
        //TODO 无属性的话不需要响应铁索连环
        HandleIronChain();
    }

    //铁索连环结算
    public void HandleIronChain()
    {
        //TODO 铁索连环结算
        UseCardManager.Instance.FinishSettle();
    }

    ////完成结算
    //public void FinishSettle()
    //{
    //    if (TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count == 0)
    //    {
    //        //去除所有目标高亮
    //        HighlightManager.DisableAllTargetsGlow();
    //        //移除卡
    //        GlobalSettings.Instance.Table.ClearCardsFromLast();
    //        if (TargetsManager.Instance.Targets.Count == 0)
    //        {
    //            //高亮当前回合人
    //            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
    //        }
    //        else
    //        {
    //            ActiveLastOneCardOnTable();
    //        }
    //    }
    //    else
    //    {
    //        ActiveLastOneCardOnTable();
    //    }
    //}

    //public void ActiveLastOneCardOnTable()
    //{
    //    OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();

    //    if (cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Jiedaosharen)
    //    {
    //        TipCardManager.Instance.JiedaoSharenNextTarget();
    //    }
    //    else
    //    {
    //        Player nextTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
    //        UseCardManager.Instance.HandleImpeccable(cardManager);
    //    }
    //}

    //TODO 需要将伤害方法分离出来，并且统计伤害来源
}
