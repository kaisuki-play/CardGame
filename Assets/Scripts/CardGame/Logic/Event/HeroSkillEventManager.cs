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
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
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
                            await HeroSkillManager.ActiveFenrirSkill2(player);
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
                            await HeroSkillManager.ActiveOsirisSkill1(player, playedCard, targetID);
                            break;
                        case HeroSkillType.OsirisSkill2:
                            await HeroSkillManager.ActiveOsirisSkill2(player);
                            break;
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
                    player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        activeSkill = true;
                        tcs.SetResult(true);
                    });

                    player.ShowOp3Button = true;
                    player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp3Button2Text("不发动Fenrir技能1");
                    player.PArea.Portrait.OpButton3.onClick.AddListener(async () =>
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
