using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetsSelectManager : MonoBehaviour
{
    /// 1.5
    /// <summary>
    /// 有武器则需要指定杀目标
    /// </summary>
    /// <param name="playedCard"></param>
    public static void HandleSpecialTarget(OneCardManager playedCard)
    {
        Player specialTargetPlayer = GlobalSettings.Instance.FindPlayerByID(playedCard.TargetsPlayerIDs[0]);
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(specialTargetPlayer, TypeOfEquipment.Weapons);
        //判断借刀对象是否有武器
        if (hasWeapon)
        {
            Debug.Log("有武器");
            // TODO 判断借刀对象是否有可杀的目标
            specialTargetPlayer.PArea.Portrait.TargetComponent.GetComponent<DragOnTarget>().CardManager = playedCard;
            specialTargetPlayer.ShowJiedaosharenTarget = true;
        }
    }

    /// 1.5 第四类选择目标
    /// <summary>
    /// 选完借刀目标，杀人目标，出借刀杀人的牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public static void HandleJiedaosharen(OneCardManager playedCard, int specialTarget = -1)
    {
        playedCard.SpecialTargetPlayerIDs.Add(specialTarget);

        HighlightManager.DisableAllHeroTarget();

        TaskManager.Instance.UnBlockTask();
    }

    /// <summary>
    /// 默认一个目标后，选择多个其他目标的
    /// </summary>
    /// <param name="playedCard"></param>
    public static void HandleMoreTargets(OneCardManager playedCard)
    {
        List<Player> alivePlayers = GlobalSettings.Instance.PlayerInstances.Where(n => n.IsDead == false).ToList();
        foreach (Player targetPlayer in alivePlayers)
        {
            if (!playedCard.TargetsPlayerIDs.Contains(targetPlayer.ID))
            {
                targetPlayer.ShowOp1Button = true;
                targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    playedCard.TargetsPlayerIDs.Add(targetPlayer.ID);
                    TaskManager.Instance.UnBlockTask();
                });
            }
        }

        TurnManager.Instance.whoseTurn.ShowOp2Button = true;
        TurnManager.Instance.whoseTurn.PArea.Portrait.ChangeOp2ButtonText("完成");
        TurnManager.Instance.whoseTurn.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        TurnManager.Instance.whoseTurn.PArea.Portrait.OpButton2.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TurnManager.Instance.whoseTurn.ShowOp1Button = false;
            TaskManager.Instance.UnBlockTask();
        });
    }
}
