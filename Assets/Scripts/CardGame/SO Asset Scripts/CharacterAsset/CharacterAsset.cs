using UnityEngine;
using System.Collections;

public enum CharClass { Elf, Monk, Warrior }

public enum CharSex { Male, Female }

public enum PlayerWarrior
{
    Caocao,
    Zhaoyun,
    Zhenji,
    Sunshangxiang,
    Liubei,
    Ganfuren,
    Huatuo,
    Liuchan,
    Zhoutai,
    Chengong,
    KuailiangKuaiyue,
    Zhangsong
}

public enum Nation
{
    Wei,
    Shu,
    Wu,
    Qun
}

public class CharacterAsset : ScriptableObject
{
    public CharClass Class;
    public string ClassName;
    public int MaxHealth = 30;
    public CharSex CharSex;
    public PlayerWarrior PlayerWarrior;
    public Nation PlayerNation;
    public string HeroPowerName;
    public Sprite AvatarImage;
    public Sprite HeroPowerIconImage;
    public Sprite AvatarBGImage;
    public Sprite HeroPowerBGImage;
    public Color32 AvatarBGTint;
    public Color32 HeroPowerBGTint;
    public Color32 ClassCardTint;
    public Color32 ClassRibbonsTint;
}
