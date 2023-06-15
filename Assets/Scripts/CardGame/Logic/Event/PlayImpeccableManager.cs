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
    public void ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        ImpeccableManager.Instance.TipWillWork = !ImpeccableManager.Instance.TipWillWork;
        ImpeccableManager.Instance.RestartInquireTarget();
    }
}
