using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum TargetingOptions
{
    NoTarget,
    AllCharacters,
    EnemyCharacters,
    YourCharacters,
    EnemyNoDistanceLimit
}

public enum TypesOfCards
{
    Base, Tips, DelayTips, Equipment, None
}

public enum SubTypeOfCards
{
    Jink, Slash, ThunderSlash, FireSlash, Peach, Analeptic,
    Wuzhongshengyou, Shunshouqianyang, Guohechaiqiao, Juedou, Wanjianqifa, Nanmanruqin, Taoyuanjieyi, Wugufengdeng, Tiesuolianhuan, Jiedaosharen, Huogong,
    Impeccable,
    Thunder, Binliangcunduan, Lebusishu,
    FrostBlade, //寒冰剑
    Zhugeliannu, //诸葛连弩
    Gudiandao,//古锭刀
    SilverLion,//白银狮子
    Zhuqueyushan,//朱雀羽扇
    CixiongDoubleSwards,//雌雄双股剑
    Baguazhen,//八卦阵
    Tengjia,//藤甲
    Renwangdun,//仁王盾
    Qinglongyanyuedao,//青龙偃月刀
    Qilingong,//麒麟弓
    Jueying,//绝影
    Chitu,//赤兔
    Dilu,//的卢
    Guanshifu,//贯石斧
    Qinghongjian,//青虹剑
    Zhangbashemao,//丈八蛇矛
    Fangtianhuaji,//方天画戟
    Zhuahuangfeidian,//爪黄飞电
    Zixing,//紫骍
    Dawan,//大宛
    Hualiu,//骅骝
    SilverMoon,//银月枪
    Cart,//木流牛马,
    ThunderHarmer,//雷神之锤
    VictorySword,//胜利之剑
}

public enum TypeOfEquipment
{
    None,
    Weapons,//武器
    Armor,//防具
    AddAHorse,//加一马
    MinusAHorse,//减一马
    Treasure//宝物
}

public enum CardSuits
{
    Spades, Hearts, Clubs, Diamonds, None
}

public enum CardRank
{
    Rank_A, Rank_2, Rank_3, Rank_4, Rank_5, Rank_6, Rank_7, Rank_8, Rank_9, Rank_10, Rank_J, Rank_Q, Rank_K, Rank_0
}

public enum CardColor
{
    Red, Black, None
}

public class CardAsset : ScriptableObject
{
    [TextArea(2, 3)]
    public string Description;  // Description for spell or character

    public Sprite CardImage;

    public TypesOfCards TypeOfCard;

    // sub type of card,e.g. Jink or Slash and so on.
    public SubTypeOfCards SubTypeOfCard;

    public TypeOfEquipment TypeOfEquipment;

    [Header("SpellInfo")]
    public string SpellScriptName;
    public int SpecialSpellAmount;
    public TargetingOptions Targets;
    public SpellAttribute SpellAttribute;

    [Header("Suit")]
    public CardSuits Suits;
    public CardRank CardRank;
    public CardColor CardColor;

    [Header("Weapon Attack Distance")]
    public int WeaponAttackDistance;

    public void ReadFromAsset(CardAsset cardAsset)
    {
        this.Description = cardAsset.Description;
        this.CardImage = cardAsset.CardImage;
        this.TypeOfCard = cardAsset.TypeOfCard;
        this.SubTypeOfCard = cardAsset.SubTypeOfCard;
        this.SpellScriptName = cardAsset.SpellScriptName;
        this.Targets = cardAsset.Targets;
        this.SpellAttribute = cardAsset.SpellAttribute;
        this.SpecialSpellAmount = cardAsset.SpecialSpellAmount;
    }

}
