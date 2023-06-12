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
            curTargetPlayer.DamageEffect(originDamage);

            SettleManager.Instance.DamageSourceId = cardManager.Owner.ID;

            AfterDamage();
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
        TargetsManager.Instance.Targets.RemoveAt(0);
        if (TargetsManager.Instance.Targets.Count == 0)
        {
            GlobalSettings.Instance.Table.ClearCards();
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
        }
        else
        {
            GameObject card = GlobalSettings.Instance.Table.CardsOnTable[0];
            OneCardManager cardManager = card.GetComponent<OneCardManager>();

            Player nextTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0]);
            PlayCardManager.Instance.ActiveEffect(cardManager);
        }
    }

    //TODO 需要将伤害方法分离出来，并且统计伤害来源
}
