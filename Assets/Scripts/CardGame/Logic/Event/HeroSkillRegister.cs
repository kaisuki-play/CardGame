using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    Hook33
}

public enum HeroSkillType
{
    AthenaSkill1,
    AthenaSkill2,
    MattSkill1,
    MattSkill2,
    FenrirSkill1,
    FenrirSkill2,
    AnubisSkill1,
    OsirisSkill1,
    OsirisSkill2,
    NephthysSkill1,
    NephthysSkill2,
    NephthysSkill3,
    PrometheusSkill1,
    PrometheusSkill2,
    LiuFengSkill1,
    LiuFengSkill2
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
                    skillList.Add(new HeroSkillInfo(HeroSkillType.AthenaSkill2, skill2PhaseList));

                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                    HeroDamageEventManager.RegisterDamageEvent(player);
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
                    HeroDamageEventManager.RegisterDamageEvent(player);
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
                    HeroDamageEventManager.RegisterDamageEvent(player);
                }

                break;
            case PlayerWarrior.Anubis:

                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook10);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.AnubisSkill1, skill1PhaseList));
                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                    HeroDamageEventManager.RegisterDamageEvent(player);
                }

                break;
            case PlayerWarrior.Osiris:
                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook27);
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook16);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.OsirisSkill1, skill1PhaseList));

                    List<HeroSkillActivePhase> skill2PhaseList = new List<HeroSkillActivePhase>();
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook13);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.OsirisSkill2, skill2PhaseList));

                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                    HeroDamageEventManager.RegisterDamageEvent(player);
                }
                break;
            case PlayerWarrior.Nephthys:
                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook23);
                    skillList.Add(new HeroSkillInfo(HeroSkillType.NephthysSkill1, skill1PhaseList));

                    List<HeroSkillActivePhase> skill2PhaseList = new List<HeroSkillActivePhase>();
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook33);
                    skillList.Add(new HeroSkillInfo(HeroSkillType.NephthysSkill2, skill2PhaseList));

                    List<HeroSkillActivePhase> skill3PhaseList = new List<HeroSkillActivePhase>();
                    skill3PhaseList.Add(HeroSkillActivePhase.Hook1);
                    skillList.Add(new HeroSkillInfo(HeroSkillType.NephthysSkill3, skill3PhaseList));

                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                    HeroDamageEventManager.RegisterDamageEvent(player);
                }
                break;
            case PlayerWarrior.Prometheus:
                {
                    List<HeroSkillActivePhase> skill1PhaseList = new List<HeroSkillActivePhase>();
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook27);
                    skill1PhaseList.Add(HeroSkillActivePhase.Hook1);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.PrometheusSkill1, skill1PhaseList));

                    List<HeroSkillActivePhase> skill2PhaseList = new List<HeroSkillActivePhase>();
                    skill2PhaseList.Add(HeroSkillActivePhase.Hook27);

                    skillList.Add(new HeroSkillInfo(HeroSkillType.PrometheusSkill2, skill2PhaseList));

                    HeroSkillRegister.SkillRegister[player.ID] = skillList;

                    HeroSkillEventManager.RegisterSkillEvent(player);
                    HeroDamageEventManager.RegisterDamageEvent(player);
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

    public static async Task PriorityHeroSkill(HeroSkillActivePhase heroSkillActivePhase, OneCardManager playedCard = null, int targetID = -1)
    {
        if (TurnManager.Instance.whoseTurn == null)
        {
            return;
        }
        Player player1 = TurnManager.Instance.whoseTurn;
        Player player2 = player1.OtherDontIgnoreDeadPlayer;
        Player player3 = player2.OtherDontIgnoreDeadPlayer;
        Player player4 = player3.OtherDontIgnoreDeadPlayer;
        Player player5 = player4.OtherDontIgnoreDeadPlayer;
        Player player6 = player5.OtherDontIgnoreDeadPlayer;

        Queue<Task> eventQueue = new Queue<Task>();

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player1, heroSkillActivePhase))
        {
            await player1.InvokeHeroSkillEvent(heroSkillActivePhase, playedCard, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player2, heroSkillActivePhase))
        {
            await player2.InvokeHeroSkillEvent(heroSkillActivePhase, playedCard, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player3, heroSkillActivePhase))
        {
            await player3.InvokeHeroSkillEvent(heroSkillActivePhase, playedCard, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player4, heroSkillActivePhase))
        {
            await player4.InvokeHeroSkillEvent(heroSkillActivePhase, playedCard, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player5, heroSkillActivePhase))
        {
            await player5.InvokeHeroSkillEvent(heroSkillActivePhase, playedCard, targetID);
        }

        if (HeroSkillRegister.NeedToActiveSkillForPlayer(player6, heroSkillActivePhase))
        {
            await player6.InvokeHeroSkillEvent(heroSkillActivePhase, playedCard, targetID);
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 计算伤害值
    /// </summary>
    /// <param name="damageSource"></param>
    /// <param name="playedCard"></param>
    /// <param name="heroSkillActivePhase"></param>
    /// <param name="targetID"></param>
    /// <param name="originDamage"></param>
    /// <returns></returns>
    public static async Task<int> CalculateDamageForSkill(Player damageSource, OneCardManager playedCard, HeroSkillActivePhase heroSkillActivePhase, int targetID, int originDamage)
    {
        if (HeroSkillRegister.NeedToActiveSkillForPlayer(damageSource, heroSkillActivePhase))
        {
            int res = await damageSource.InvokeHeroDamageCalculuateEvent(playedCard, heroSkillActivePhase, targetID, originDamage);
            return res;
        }
        return originDamage;
    }


}

