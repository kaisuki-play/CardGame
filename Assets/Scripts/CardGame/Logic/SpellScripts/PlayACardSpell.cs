using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayACardSpell : SpellEffect
{
    public override void ActivateEffect(OneCardManager oneCard, List<int> targets)
    {
        CardAsset cardAsset = oneCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with attribute:" + cardAsset.SpellAttribute);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with targets:" + targets.Count);
        // restart timer
        TurnManager.Instance.RestartTimer();

        switch (cardAsset.TypeOfCard)
        {
            case TypesOfCards.Base:
                break;
            case TypesOfCards.Tips:
                // TODO 进入无懈流程
                break;
            case TypesOfCards.Equipment:
                break;
        }

    }


}
