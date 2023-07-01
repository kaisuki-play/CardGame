using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public enum HeroSkillActivePhase
{
    Hook1,
    Hook2,
    Hook3,
    Hook4,
    Hook5,
    Hook6,
    Hook7,
    Hook8,
    Hook9,
    Hook10,
    Hook11,
    Hook12,
    Hook13,
    Hook14,
    Hook15,
    Hook16,
    Hook17,
    Hook18,
    Hook19,
    Hook20,
    Hook21,
    Hook22,
    Hook23,
    Hook24,
    Hook25,
    Hook26,
    Hook27,
    Hook28,
    Hook29,
    Hook30,
    Hook31,
    Hook32,
}

public enum HeroSkillType
{
    AthenaSkill1,
    AthenaSkill2,
    MattSkill1,
    MattSkill2,
    FenrirSkill1,
    FenrirSkill2,
    AnubisSkill1
}

public struct HeroSkillInfo
{
    public HeroSkillType SkillType;
    public List<HeroSkillActivePhase> PhaseList;
    public HeroSkillInfo(HeroSkillType skillType, List<HeroSkillActivePhase> list)
    {
        SkillType = skillType;
        PhaseList = list;
    }

}

public class HeroSkillRegister : MonoBehaviour
{
    //技能注册表
    public static Dictionary<int, List<HeroSkillInfo>> SkillRegister = new Dictionary<int, List<HeroSkillInfo>>();

    public static void HandlePlayersSkill()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            HeroSkillRegister.HandlePlayerSkill(player);
        }
    }

    public static void HandlePlayerSkill(Player player)
    {
        List<HeroSkillInfo> skillList = new List<HeroSkillInfo>();
        Debug.Log(player.PArea.Owner + "~~~~~~~~~~~~~~~~~~~~~~~~~~" + player.ID);
        switch (player.CharAsset.PlayerWarrior)
        {
            case PlayerWarrior.Athena:
                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook1);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.AthenaSkill1, skill1PhaseList));

                    List<HeroSkillActivePhase> skill2PhaseList = new List<HeroSkillActivePhase>();
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook11);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.AthenaSkill1, skill2PhaseList));
                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                }
                break;
            case PlayerWarrior.Maat:
                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook10);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.MattSkill1, skill1PhaseList));

                    List<HeroSkillActivePhase> skill2PhaseList = new List<HeroSkillActivePhase>();
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook14);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.MattSkill2, skill2PhaseList));
                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                }
                break;
            case PlayerWarrior.Fenrir:

                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook9);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.FenrirSkill1, skill1PhaseList));

                    List<HeroSkillActivePhase> skill2PhaseList = new List<HeroSkillActivePhase>();
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook4);
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook15);
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook16);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.FenrirSkill2, skill2PhaseList));
                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                }

                break;
            case PlayerWarrior.Anubis:

                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook10);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.AnubisSkill1, skill1PhaseList));
                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                }

                break;
        }

    }

    public static bool NeedToActiveSkillForPlayer(Player player, HeroSkillActivePhase heroSkillActivePhase)
    {
        if (!HeroSkillRegister.SkillRegister.ContainsKey(player.ID))
        {
            return false;
        }
        //先获得技能信息列表
        List<HeroSkillInfo> skillList = HeroSkillRegister.SkillRegister[player.ID];
        //如果有技能则返回T，否则返回F
        foreach (HeroSkillInfo heroSkillInfo in skillList)
        {
            if (heroSkillInfo.PhaseList.Contains(heroSkillActivePhase))
            {
                return true;
            }
        }
        return false;
    }

    public static async Task PriorityHeroSkill(OneCardManager playedCard, HeroSkillActivePhase heroSkillActivePhase, int targetID)
    {
        Player player1 = TurnManager.Instance.whoseTurn;
        Player player2 = player1.OtherDontIgnoreDeadPlayer;
        Player player3 = player2.OtherDontIgnoreDeadPlayer;
        Player player4 = player3.OtherDontIgnoreDeadPlayer;
        Player player5 = player4.OtherDontIgnoreDeadPlayer;
        Player player6 = player5.OtherDontIgnoreDeadPlayer;

        Queue<Task> eventQueue = new Queue<Task>();

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player1, heroSkillActivePhase))
        {
            await player1.InvokeHeroSkillEvent(playedCard, heroSkillActivePhase, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player2, heroSkillActivePhase))
        {
            await player2.InvokeHeroSkillEvent(playedCard, heroSkillActivePhase, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player3, heroSkillActivePhase))
        {
            await player3.InvokeHeroSkillEvent(playedCard, heroSkillActivePhase, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player4, heroSkillActivePhase))
        {
            await player4.InvokeHeroSkillEvent(playedCard, heroSkillActivePhase, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player5, heroSkillActivePhase))
        {
            await player5.InvokeHeroSkillEvent(playedCard, heroSkillActivePhase, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player6, heroSkillActivePhase))
        {
            await player6.InvokeHeroSkillEvent(playedCard, heroSkillActivePhase, targetID);
        }
        await TaskManager.Instance.DontAwait();
    }

}

