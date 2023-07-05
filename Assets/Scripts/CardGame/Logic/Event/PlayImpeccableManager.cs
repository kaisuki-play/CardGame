using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayImpeccableManager : MonoBehaviour
{
    public static PlayImpeccableManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public async Task ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);

        ImpeccableManager.Instance.TipWillWork = !ImpeccableManager.Instance.TipWillWork;
        ImpeccableManager.Instance.RestartInquireTarget();

        await TaskManager.Instance.DontAwait();
    }
}
