using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HeroSKillStateKey
{
    OsirisSkill1State,//手牌当五谷丰登
    OsirisSkill2State,//木乃伊
    PrometheusSkill1Card,
    PrometheusSkill2Card,
    EnteredDying,
    YangxiuSkill1State,
    YangxiuSkill2State,
    YangxiuSkill3State
}
public class HeroSkillState : MonoBehaviour
{
    //存储boolean类型的技能,回合结束后清除
    public static Dictionary<HeroSKillStateKey, bool> HeroSkillBooleanDic_Once = new Dictionary<HeroSKillStateKey, bool>();

    ///存储卡牌类型的技能,回合结束后清除
    public static Dictionary<HeroSKillStateKey, OneCardManager> HeroSkillCardDic_Once = new Dictionary<HeroSKillStateKey, OneCardManager>();

    ///存储卡牌类型的技能,回合结束后清除
    public static Dictionary<HeroSKillStateKey, TypesOfCards> HeroSkillCardTypeDic_Once = new Dictionary<HeroSKillStateKey, TypesOfCards>();

    //清除一次性的值
    public static void ClearOnceValue()
    {
        HeroSkillBooleanDic_Once.Clear();
        HeroSkillCardDic_Once.Clear();
        HeroSkillCardTypeDic_Once.Clear();
    }
}
