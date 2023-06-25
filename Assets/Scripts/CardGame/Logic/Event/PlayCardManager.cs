using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardManager : MonoBehaviour
{
    public static PlayCardManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayAVisualCardFromHand(OneCardManager playedCard, List<int> targets)
    {
        playedCard.TargetsPlayerIDs = targets;

        // remove this card from hand
        switch (playedCard.CardLocation)
        {
            case CardLocation.Hand:
                {
                    playedCard.Owner.Hand.DisCard(playedCard.UniqueCardID);

                    playedCard.Owner.PArea.HandVisual.PlayASpellFromHand(playedCard.UniqueCardID);
                }
                break;
            case CardLocation.UnderCart:
                {
                    playedCard.Owner.TreasureLogic.DisCard(playedCard.UniqueCardID);

                    playedCard.Owner.PArea.TreasureVisual.PlayASpellFromTreasure(playedCard.UniqueCardID);
                }
                break;
        }
    }

    public void ActivateEffect(OneCardManager playedCard)
    {
        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Slash:
            case SubTypeOfCards.ThunderSlash:
            case SubTypeOfCards.FireSlash:
                PlaySlashManager.Instance.ActiveEffect(playedCard);
                break;
            case SubTypeOfCards.Jink:
                PlayJinkManager.Instance.ActiveEffect(playedCard);
                break;
            case SubTypeOfCards.Impeccable:
                PlayImpeccableManager.Instance.ActiveEffect(playedCard);
                break;
        }
    }
}
