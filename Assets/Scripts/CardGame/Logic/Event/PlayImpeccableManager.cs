using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayImpeccableManager : MonoBehaviour
{
    public static PlayImpeccableManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public async void ActiveEffect(OneCardManager playedCard)
    {
        await SkillManager.UseACard(playedCard);

        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);

        //TODO 修改为打出无懈时 加SKillManager一个hook AfterPlayAImpeccable
        //使用了一张牌的hook
        await SkillManager.UseACard(playedCard);

        ImpeccableManager.Instance.TipWillWork = !ImpeccableManager.Instance.TipWillWork;
        ImpeccableManager.Instance.RestartInquireTarget();
    }
}
