using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DelayTipManager : MonoBehaviour
{
    //是否有延时锦囊
    public static bool HasDelayTips(Player targetPlayer)
    {
        return targetPlayer.JudgementLogic.CardsInJudgement.Count != 0;
    }

    //处理延时锦囊
    public static async void HandleDelayTip(Player targetPlayer)
    {
        int lastJudgementCard = targetPlayer.JudgementLogic.CardsInJudgement[targetPlayer.JudgementLogic.CardsInJudgement.Count - 1];
        GameObject judgementCard = IDHolder.GetGameObjectWithID(lastJudgementCard);
        OneCardManager judgementCardManager = judgementCard.GetComponent<OneCardManager>();
        switch (judgementCardManager.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Lebusishu:
                Debug.Log("乐不思蜀");
                break;
            case SubTypeOfCards.Binliangcunduan:
                Debug.Log("兵粮寸断");
                break;
            case SubTypeOfCards.Thunder:
                Debug.Log("闪电");
                break;
        }
        await Task.Delay(5000);
        Debug.Log("ddddd");
        TaskManager.Instance.UnBlockTask();
    }
}
