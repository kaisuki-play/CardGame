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

        await SkillManager.UseACard(playedCard);

        //TODO 挪到这里 await SkillManager.AfterPlayAJink(cardManager.Owner, playedCard.Owner);
        await SkillManager.AfterPlayAJink(cardManager, playedCard.Owner);

        if (cardManager != null)
        {
            switch (cardManager.CardAsset.TypeOfCard)
            {
                case TypesOfCards.Tips:
                    {
                        TipCardManager.Instance.SkipTipCard();
                    }
                    break;
                default:
                    {
                        switch (cardManager.CardAsset.SubTypeOfCard)
                        {
                            case SubTypeOfCards.Slash:
                            case SubTypeOfCards.ThunderSlash:
                            case SubTypeOfCards.FireSlash:
                                {
                                    UseCardManager.Instance.BackToWhoseTurn();
                                }
                                break;
                            default:
                                {
                                    (bool hasJiedaosharen, OneCardManager jiedaosharenCard) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
                                    if (hasJiedaosharen)
                                    {
                                        GlobalSettings.Instance.Table.ClearCardsFromLast();
                                        TipCardManager.Instance.JiedaoSharenNextTarget();
                                    }
                                    else
                                    {
                                        UseCardManager.Instance.BackToWhoseTurn();
                                    }
                                }
                                break;
                        }

                    }
                    break;
            }
        }
    }
}
