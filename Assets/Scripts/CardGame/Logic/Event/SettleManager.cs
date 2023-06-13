using System.Collections;
using System.Collections.Generic;
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
        OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();
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
                            default:
                                break;
                        }
                    }
                    break;
            }

        }
    }

    //计算伤害
    public void CalculateDamage()
    {
        OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();
        if (cardManager != null)
        {

            int originDamage = cardManager.CardAsset.SpecialSpellAmount;

            Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0]);

            //结算伤害
            HealthManager.Instance.DamageEffect(originDamage, curTargetPlayer);

            SettleManager.Instance.DamageSourceId = cardManager.Owner.ID;

            //移除已经结算完伤害的玩家
            if (TargetsManager.Instance.Targets.Count > 0)
            {
                TargetsManager.Instance.Targets.RemoveAt(0);
            }

            //是否进入濒死流程
            if (curTargetPlayer.Health <= 0)
            {
                Debug.Log("中断流程，进入濒死流程");
                DyingManager.Instance.EnterDying(curTargetPlayer);
            }
            else
            {
                AfterDamage();
            }
        }
    }

    //伤害后
    public void AfterDamage()
    {
        //TODO 无属性的话不需要响应铁索连环
        HandleIronChain();
    }

    //铁索连环结算
    public void HandleIronChain()
    {
        //TODO 铁索连环结算
        FinishSettle();
    }

    //完成结算
    public void FinishSettle()
    {
        if (TargetsManager.Instance.Targets.Count == 0)
        {
            //去除所有目标高亮
            HighlightManager.DisableAllTargetsGlow();
            //移除卡
            GlobalSettings.Instance.Table.ClearCards();
            //高亮当前回合人
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
        }
        else
        {
            OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();

            Player nextTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0]);
            PlayCardManager.Instance.ActiveEffect(cardManager);
        }
    }

    //TODO 需要将伤害方法分离出来，并且统计伤害来源
}
