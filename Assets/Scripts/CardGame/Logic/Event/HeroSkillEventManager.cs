using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
//事件自定义传参
public class SkillEventArgs : EventArgs
{
    public HeroSkillActivePhase SkillPhase { get; }

    public OneCardManager PlayedCard { get; }
    public int TargetID { get; }

    public SkillEventArgs(OneCardManager playedCard, HeroSkillActivePhase skillPhase, int targetID)
    {
        SkillPhase = skillPhase;
        PlayedCard = playedCard;
        TargetID = targetID;
    }
}

public class HeroSkillEventManager : MonoBehaviour
{
    public static void RegisterSkillEvent(Player player)
    {
        player.HeroSkillEvent += HandleHeroSkillActiveEvent;
    }

    public static async Task HandleHeroSkillActiveEvent(object sender, SkillEventArgs e)
    {
        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner + "-----------------------------------------------" + player.CharAsset.PlayerWarrior);
        HeroSkillActivePhase skillPhase = e.SkillPhase;
        List<HeroSkillInfo> skillList = HeroSkillRegister.SkillRegister[player.ID];
        switch (player.CharAsset.PlayerWarrior)
        {
            case PlayerWarrior.Athena:
                {
                    Debug.Log("-----------------------------------------------Athena 有技能需要触发" + e.SkillPhase);
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.AthenaSkill1:
                            await HeroSkillManager.ActiveAthenaSkill1(player, playedCard);
                            break;
                        case HeroSkillType.AthenaSkill2:
                            await HeroSkillManager.ActiveAthenaSkill2(player, playedCard, targetID);
                            break;
                    }

                }
                break;
            case PlayerWarrior.Maat:
                {
                    Debug.Log("-----------------------------------------------Maat 有技能需要触发" + e.SkillPhase);
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.MattSkill1:
                            await HeroSkillManager.ActiveMaatSkill1(player, playedCard, targetID);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Fenrir:

                {
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.FenrirSkill2:
                            //await HeroSkillManager.ActiveFenrirSkill2(player);
                            break;
                    }
                }

                break;
            case PlayerWarrior.Anubis:
                {
                    Debug.Log("-----------------------------------------------Anubis 有技能需要触发");
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.AnubisSkill1:
                            await HeroSkillManager.ActiveAnubisSkill1(player, playedCard, targetID);
                            break;
                    }
                }

                break;
            case PlayerWarrior.Osiris:
                {
                    Debug.Log("-----------------------------------------------Osiris 有技能需要触发" + e.SkillPhase);
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.OsirisSkill1:
                            await HeroSkillManager.ActiveOsirisSkill1(player, playedCard);
                            break;
                        case HeroSkillType.OsirisSkill2:
                            await HeroSkillManager.ActiveOsirisSkill2(player);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Nephthys:
                {
                    Debug.Log("-----------------------------------------------Nephthys 有技能需要触发" + e.SkillPhase);
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.NephthysSkill1:
                            await HeroSkillManager.ActiveNephthysSkill1(player, playedCard);
                            break;
                        case HeroSkillType.NephthysSkill2:
                            await HeroSkillManager.ActiveNephthysSkill2(player, playedCard, TurnManager.Instance.whoseTurn.ID);
                            break;
                        case HeroSkillType.NephthysSkill3:
                            await HeroSkillManager.ActiveNephthysSkill3(player, playedCard);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Prometheus:
                {
                    Debug.Log("-----------------------------------------------Prometheus 有技能需要触发" + e.SkillPhase);
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    if (skillList[0].PhaseList.Contains(skillPhase))
                    {
                        await HeroSkillManager.ActivePrometheusSkill1(player, playedCard, skillPhase);
                    }
                    if (skillList[1].PhaseList.Contains(skillPhase))
                    {
                        await HeroSkillManager.ActivePrometheusSkill2(player, playedCard, skillPhase);
                    }
                }
                break;
            case PlayerWarrior.Liufeng:
                {
                    Debug.Log("-----------------------------------------------刘封 有技能需要触发");
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.LiuFengSkill1:
                            await HeroSkillManager.ActiveLiufengSkill1(player, playedCard, targetID, skillPhase);
                            break;
                        case HeroSkillType.LiuFengSkill2:
                            await HeroSkillManager.ActiveLiufengSkill2(player);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Yangxiu:
                {
                    Debug.Log("-----------------------------------------------杨修 有技能需要触发");
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.YangxiuSkill1:
                            await HeroSkillManager.ActiveYangxiuSkill1(player, playedCard);
                            break;
                        case HeroSkillType.YangxiuSkill2:
                            await HeroSkillManager.ActiveYangxiuSkill2(player, playedCard);
                            break;
                        case HeroSkillType.YangxiuSkill3:
                            await HeroSkillManager.ActiveYangxiuSkill3(player, targetID);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Freyj:

                {
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.FreyjSkill2:
                            await HeroSkillManager.ActiveFreyjSkill2(player);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Liru:
                {
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    OneCardManager playedCard = e.PlayedCard;
                    int targetID = e.TargetID;
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    //switch (skillInfo.SkillType)
                    //{
                    //    case HeroSkillType.LiruSkill1:
                    //        await HeroSkillManager.ActiveLiruSkill1(player, playedCard, targetID);
                    //        break;
                    //    case HeroSkillType.LiruSkill2:
                    //        await HeroSkillManager.ActiveLiruSkill2(player, playedCard, targetID);
                    //        break;
                    //    case HeroSkillType.LiruSkill3:
                    //        await HeroSkillManager.ActiveLiruSkill3(player, playedCard, targetID);
                    //        break;
                    //}
                    if (skillList[0].PhaseList.Contains(skillPhase))
                    {
                        await HeroSkillManager.ActiveLiruSkill1(player, playedCard, targetID);
                    }
                    if (skillList[1].PhaseList.Contains(skillPhase))
                    {
                        await HeroSkillManager.ActiveLiruSkill2(player, playedCard, targetID);
                    }
                    if (skillList[2].PhaseList.Contains(skillPhase))
                    {
                        await HeroSkillManager.ActiveLiruSkill3(player, playedCard, targetID);
                    }
                }
                break;
        }
        await TaskManager.Instance.DontAwait();
    }
}

/// <summary>
/// 计算伤害
/// </summary>
public class SkillDamageEventArgs : EventArgs
{
    public HeroSkillActivePhase SkillPhase { get; }

    public OneCardManager PlayedCard { get; }
    public int TargetID { get; }
    public int OriginalDamage;

    public SkillDamageEventArgs(OneCardManager playedCard, HeroSkillActivePhase skillPhase, int targetID, int originalDamage)
    {
        SkillPhase = skillPhase;
        PlayedCard = playedCard;
        TargetID = targetID;
        OriginalDamage = originalDamage;
    }
}

public class HeroDamageEventManager : MonoBehaviour
{
    public static void RegisterDamageEvent(Player player)
    {
        player.DamageCalculateHandler += HandleHeroDamageCalculateEvent;
    }

    public static async Task<int> HandleHeroDamageCalculateEvent(object sender, SkillDamageEventArgs e)
    {
        OneCardManager playedCard = e.PlayedCard;
        int targetID = e.TargetID;
        HeroSkillActivePhase skillPhase = e.SkillPhase;

        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
        List<HeroSkillInfo> skillList = HeroSkillRegister.SkillRegister[player.ID];
        switch (player.CharAsset.PlayerWarrior)
        {
            case PlayerWarrior.Fenrir:

                {
                    if (!((playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
                        || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash
                        || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash)
                        && playedCard.CardAsset.CardColor == CardColor.Black))
                    {
                        return e.OriginalDamage;
                    }
                    Debug.Log("-----------------------------------------------Fenrir 有技能需要触发");
                    bool activeSkill = false;
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                    HighlightManager.DisableAllCards();
                    HighlightManager.DisableAllOpButtons();
                    player.ShowOp2Button = true;
                    player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp2ButtonText("发动Fenrir技能1");
                    player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        activeSkill = true;
                        tcs.SetResult(true);
                    });

                    player.ShowOp3Button = true;
                    player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp3Button2Text("不发动Fenrir技能1");
                    player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        activeSkill = false;
                        tcs.SetResult(true);
                    });
                    await tcs.Task;
                    return e.OriginalDamage + (activeSkill ? 1 : 0);
                }
            default:
                {
                    return e.OriginalDamage;
                }

        }
    }


}

public class SkillCardABSwitchEventArgs : EventArgs
{
    public HeroSkillActivePhase SkillPhase { get; }

    public OneCardManager PlayedCard { get; }

    public Player NewOwner { get; }

    public SkillCardABSwitchEventArgs(OneCardManager playedCard, Player newOwner, HeroSkillActivePhase skillPhase)
    {
        SkillPhase = skillPhase;
        PlayedCard = playedCard;
        NewOwner = newOwner;
    }
}

public class HeroCardABSwitchEventManager : MonoBehaviour
{
    public static void RegisterABSwitchEvent(Player player)
    {
        player.HeroCardABSwitchEvent += HandleHeroSwitchABActiveEvent;
    }

    public static async Task HandleHeroSwitchABActiveEvent(object sender, SkillCardABSwitchEventArgs e)
    {
        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
        HeroSkillActivePhase skillPhase = e.SkillPhase;
        List<HeroSkillInfo> skillList = HeroSkillRegister.SkillRegister[player.ID];
        OneCardManager playedCard = e.PlayedCard;
        switch (player.CharAsset.PlayerWarrior)
        {
            case PlayerWarrior.Fenrir:

                {
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.FenrirSkill2:
                            await HeroSkillManager.ActiveFenrirSkill2(player, playedCard);
                            break;
                    }
                }
                break;
            case PlayerWarrior.Freyj:

                {
                    HeroSkillInfo skillInfo = new HeroSkillInfo();
                    foreach (HeroSkillInfo skill in skillList)
                    {
                        if (skill.PhaseList.Contains(skillPhase))
                        {
                            skillInfo = skill;
                            break;
                        }
                    }
                    switch (skillInfo.SkillType)
                    {
                        case HeroSkillType.FreyjSkill1:
                            await HeroSkillManager.ActiveFreyjSkill1(player, playedCard);
                            break;
                    }
                }
                break;
        }
        await TaskManager.Instance.DontAwait();
    }
}
