using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayCardManager : MonoBehaviour
{
    public static PlayCardManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public async Task PlayAVisualCardFromHand(OneCardManager playedCard, List<int> targets)
    {
        playedCard.TargetsPlayerIDs = targets;

        // remove this card from hand
        switch (playedCard.CardLocation)
        {
            case CardLocation.Hand:
                {
                    playedCard.Owner.Hand.DisCard(playedCard.UniqueCardID);

                    await playedCard.Owner.PArea.HandVisual.PlayASpellFromHand(playedCard.UniqueCardID);
                }
                break;
            case CardLocation.UnderCart:
                {
                    playedCard.Owner.TreasureLogic.DisCard(playedCard.UniqueCardID);

                    await playedCard.Owner.PArea.TreasureVisual.PlayASpellFromTreasure(playedCard.UniqueCardID);
                }
                break;
        }
    }

    public async Task ActivateEffect(OneCardManager playedCard)
    {
        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Slash:
            case SubTypeOfCards.ThunderSlash:
            case SubTypeOfCards.FireSlash:
                await PlaySlashManager.Instance.ActiveEffect(playedCard);
                break;
            case SubTypeOfCards.Jink:
                //TODO 闪pending后触发银月枪
                await PlayJinkManager.Instance.ActiveEffect(playedCard);
                break;
            case SubTypeOfCards.Impeccable:
                await PlayImpeccableManager.Instance.ActiveEffect(playedCard);
                break;
        }
    }
}
