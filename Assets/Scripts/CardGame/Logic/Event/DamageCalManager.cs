using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalManager : MonoBehaviour
{
    public static int FinalDamage(int originalDamage, OneCardManager playedCard = null, Player targetPlayer = null)
    {
        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        //酒杀钩子
        originalDamage = AnalepticHook(originalDamage);

        //触发的计算伤害的
        if (playedCard != null)
        {
            Debug.Log("1$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            //伤害来源技能、装备等
            originalDamage = SkillManager.StartCalculateDamageForSource(playedCard, targetPlayer, originalDamage);
            //伤害目标的技能、装备等
            originalDamage = SkillManager.StartCalculateDamageForTarget(playedCard, targetPlayer, originalDamage);
        }

        Debug.Log("12$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        return originalDamage;
    }

    //酒杀的钩子
    public static int AnalepticHook(int originDamage)
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
