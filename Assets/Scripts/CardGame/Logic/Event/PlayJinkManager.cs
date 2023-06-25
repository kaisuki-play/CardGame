using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayJinkManager : MonoBehaviour
{
    public static PlayJinkManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public async void ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with attribute:" + cardAsset.SpellAttribute);
        if (TargetsManager.Instance.NeedToPlayJinkTargets.Count > 0)
        {
            Debug.Log("无事发生");
            TaskManager.Instance.UnBlockTask(TaskType.SilverMoonTask);
            return;
        }
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();

        //使用了一张牌的hook
        await SkillManager.UseACard(playedCard);

        //出闪之后的hook
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play card owner:" + playedCard.Owner.PArea.Owner);
        await SkillManager.AfterPlayAJink(cardManager, playedCard.Owner);

        if (cardManager != null)
        {
            UseCardManager.Instance.FinishSettle();
        }
    }
}
