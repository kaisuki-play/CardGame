using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayJinkManager : MonoBehaviour
{
    public static PlayJinkManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with attribute:" + cardAsset.SpellAttribute);
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
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
                                    bool guanshifuBlock = EquipmentManager.Instance.GuanshifuHook(cardManager.Owner, playedCard.Owner);
                                    if (!guanshifuBlock)
                                    {
                                        UseCardManager.Instance.BackToWhoseTurn();
                                    }
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
