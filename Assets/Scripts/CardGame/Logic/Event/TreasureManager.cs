using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TreasureManager : MonoBehaviour
{
    public static async Task OnEndTurn()
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
                targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    targetPlayer.ShowOp1Button = false;
                    while (TurnManager.Instance.whoseTurn.TreasureLogic.CardsInTreasure.Count > 0)
                    {
                        int cardId = TurnManager.Instance.whoseTurn.TreasureLogic.CardsInTreasure[0];
                        targetPlayer.GiveAssignCardToTreasure(cardId);
                    }

                    //重置宝物状态
                    TurnManager.Instance.whoseTurn.HasTreasure = false;
                    targetPlayer.HasTreasure = true;

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
            TurnManager.Instance.whoseTurn.ShowOp1Button = false;
            TaskManager.Instance.UnBlockTask(TaskType.UnderCartTask);
        });

        await TaskManager.Instance.BlockTask(TaskType.UnderCartTask);
    }
}
