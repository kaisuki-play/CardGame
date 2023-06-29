using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlaySlashManager : MonoBehaviour
{
    public static PlaySlashManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public async void ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with attribute:" + cardAsset.SpellAttribute);
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();

        //if (cardManager != null)
        //{
        //    switch (cardManager.CardAsset.SubTypeOfCard)
        //    {
        //        case SubTypeOfCards.Juedou:
        //            TipCardManager.Instance.PlayCardOwner = playedCard.Owner;
        //            TipCardManager.Instance.ActiveTipCard();
        //            break;
        //        default:
        //            //结束当前目标结算
        //            UseCardManager.Instance.FinishSettle();
        //            break;
        //    }
        //}

        await Task.WhenAll(playedCard.Owner.InvokeSlashEvent(true));
    }
}
