using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
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
        OneCardManager playedCard = e.PlayedCard;
        int targetID = e.TargetID;
        HeroSkillActivePhase skillPhase = e.SkillPhase;

        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
        List<HeroSkillInfo> skillList = HeroSkillRegister.SkillRegister[player.ID];
        switch (player.CharAsset.PlayerWarrior)
        {
            case PlayerWarrior.Athena:
                {
                    Debug.Log("-----------------------------------------------Athena 有技能需要触发");
                }
                break;
            case PlayerWarrior.Maat:
                {
                    Debug.Log("-----------------------------------------------Maat 有技能需要触发");
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
                    Debug.Log("-----------------------------------------------Fenrir 有技能需要触发");
                }

                break;
            case PlayerWarrior.Anubis:

                {
                    Debug.Log("-----------------------------------------------Anubis 有技能需要触发");
                }

                break;
        }
        await TaskManager.Instance.DontAwait();
    }
}
