using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TreasureManager : MonoBehaviour
{
    public static async Task OnInsertCard()
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();

        foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
        {
            if (targetPlayer.ID != TurnManager.Instance.whoseTurn.ID)
            {
                targetPlayer.ShowOp1Button = true;
                targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                targetPlayer.PArea.Portrait.ChangeOp1ButtonText("木流牛马");
                targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                {
                    targetPlayer.ShowOp1Button = false;

                    //给目标装备上自己的装备
                    (bool _, OneCardManager treasureCard) = EquipmentManager.Instance.HasEquipmentWithType(TurnManager.Instance.whoseTurn, TypeOfEquipment.Treasure);
                    await TurnManager.Instance.whoseTurn.PassTreasureToTarget(targetPlayer, treasureCard.UniqueCardID);

                    //重置宝物状态
                    //TurnManager.Instance.whoseTurn.HasTreasure = false;
                    //targetPlayer.HasTreasure = true;

                    TurnManager.Instance.IsInTreasureOutIn = false;

                    HighlightManager.DisableAllOpButtons();
                    HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);

                    TaskManager.Instance.UnBlockTask(TaskType.UnderCartTask);
                });
            }
        }

        TurnManager.Instance.whoseTurn.ShowOp2Button = true;
        TurnManager.Instance.whoseTurn.PArea.Portrait.ChangeOp2ButtonText("完成");
        TurnManager.Instance.whoseTurn.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        TurnManager.Instance.whoseTurn.PArea.Portrait.OpButton2.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            TurnManager.Instance.IsInTreasureOutIn = false;
            TaskManager.Instance.UnBlockTask(TaskType.UnderCartTask);
        });

        await TaskManager.Instance.BlockTask(TaskType.UnderCartTask);
    }
}
