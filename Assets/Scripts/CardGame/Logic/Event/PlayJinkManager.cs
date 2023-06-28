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

        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();

        //出闪之后的hook
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play card owner:" + playedCard.Owner.PArea.Owner);

        //await SkillManager.AfterPlayAJinkBeforeSettle(cardManager, playedCard.Owner);
        await SkillManager.AfterPlayAJink(cardManager, playedCard.Owner);
        Debug.Log("***********************************去结算需要出闪的事件***************************************************");
        await Task.WhenAll(playedCard.Owner.InvokeJinkEvent(true));

    }
}
