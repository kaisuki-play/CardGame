using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HeroSKillStateKey
{
    OsirisSkill1State,//手牌当五谷丰登
    OsirisSkill2State,//木乃伊
}
public class HeroSkillState : MonoBehaviour
{
    //存储boolean类型的技能,限定一次
    public static Dictionary<HeroSKillStateKey, bool> HeroSkillBooleanDic_Once = new Dictionary<HeroSKillStateKey, bool>();

}
