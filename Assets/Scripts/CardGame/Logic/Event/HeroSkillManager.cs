using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HeroSkillManager : MonoBehaviour
{
    public static async Task ActiveAthenaSkill1(OneCardManager playedCard)
    {
        //牌必须是锦囊牌
        if (playedCard.CardAsset.TypeOfCard != TypesOfCards.Tips && playedCard.CardAsset.TypeOfCard != TypesOfCards.DelayTips)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        foreach (int keys in HeroSkillRegister.SkillRegister.Keys)
        {
            Debug.Log(keys);
        }
        if (HeroSkillRegister.SkillRegister.ContainsKey(playedCard.Owner.ID) && HeroSkillRegister.SkillRegister[playedCard.Owner.ID].Contains(HeroSkillType.AthenaSkill1))
        {
            TaskManager.Instance.AddATask(TaskType.AthenaSkill1);

            Player player = playedCard.Owner;

            HighlightManager.DisableAllOpButtons();
            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动技能1");
            player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                await player.DrawSomeCards(1);
                TaskManager.Instance.UnBlockTask(TaskType.AthenaSkill1);
            });

            List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
            if (canAttackTargets.Count == 0)
            {
                player.PArea.Portrait.OpButton2.enabled = false;
            }

            player.ShowOp3Button = true;
            player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp3Button2Text("不发动技能1");
            player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TaskManager.Instance.UnBlockTask(TaskType.AthenaSkill1);
            });

            await TaskManager.Instance.TaskBlockDic[TaskType.AthenaSkill1][0].Task;
        }
    }
}
