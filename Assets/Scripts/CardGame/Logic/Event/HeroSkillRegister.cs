using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroSkillType
{
    AthenaSkill1,
    AthenaSkill2,
    MattSkill1,
    MattSkill2,
    FenrirSkill1,
    FenrirSkill2
}
public class HeroSkillRegister : MonoBehaviour
{
    //技能注册表
    public static Dictionary<int, List<HeroSkillType>> SkillRegister = new Dictionary<int, List<HeroSkillType>>();

    public static void HandlePlayersSkill()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            HeroSkillRegister.HandlePlayerSkill(player);
        }
    }

    public static void HandlePlayerSkill(Player player)
    {
        List<HeroSkillType> skillList = new List<HeroSkillType>();
        Debug.Log(player.PArea.Owner + "~~~~~~~~~~~~~~~~~~~~~~~~~~" + player.ID);
        switch (player.CharAsset.PlayerWarrior)
        {
            case PlayerWarrior.Athena:
                skillList.Add(HeroSkillType.AthenaSkill1);
                skillList.Add(HeroSkillType.AthenaSkill2);
                HeroSkillRegister.SkillRegister[player.ID] = skillList;
                break;
            case PlayerWarrior.Maat:
                skillList.Add(HeroSkillType.MattSkill1);
                skillList.Add(HeroSkillType.MattSkill2);
                HeroSkillRegister.SkillRegister[player.ID] = skillList;
                break;
            case PlayerWarrior.Fenrir:
                skillList.Add(HeroSkillType.FenrirSkill1);
                skillList.Add(HeroSkillType.FenrirSkill2);
                HeroSkillRegister.SkillRegister[player.ID] = skillList;
                break;
        }

    }
}
