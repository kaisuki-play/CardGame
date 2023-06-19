using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardsType
{
    Normal,
    Slash
}

public class HighlightManager : MonoBehaviour
{
    public static void DisableAllHeroTarget()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            player.ShowJiedaosharenTarget = false;
        }
    }

    public static void DisableAllCards()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            foreach (int cardId in player.Hand.CardsInHand)
            {
                OneCardManager oneCardManager = IDHolder.GetGameObjectWithID(cardId).GetComponent<OneCardManager>();
                oneCardManager.CanBePlayedNow = false;
            }
        }
    }

    public static void DisableAllOpButtons()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            player.ShowOp1Button = false;
            player.ShowOp2Button = false;
            player.ShowOp3Button = false;
        }
    }

    public static void DisableAllTargetsGlow()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            player.ShowTargetGlow = false;
        }
    }

    public static void DisableAllTurnGlow()
    {
        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        {
            player.IsThisTurn = false;
        }
    }

    public static void EnableCardsWithType(Player player, CardsType cardsType = CardsType.Normal)
    {
        //TODO 当前人出牌人死了
        HighlightManager.DisableAllCards();
        foreach (int cardId in player.Hand.CardsInHand)
        {
            OneCardManager oneCardManager = IDHolder.GetGameObjectWithID(cardId).GetComponent<OneCardManager>();
            oneCardManager.CanBePlayedNow = CanBePlayedNow(oneCardManager);
            if (oneCardManager.CardAsset.Targets != TargetingOptions.NoTarget)
            {
                oneCardManager.TargetComponent.SetActive(true);
            }
            else
            {
                oneCardManager.TargetComponent.SetActive(false);
            }
        }
    }

    public static void EnableCardWithCardType(Player player, SubTypeOfCards cardType, bool needTargetComponent = true)
    {
        HighlightManager.DisableAllCards();
        foreach (int cardId in player.Hand.CardsInHand)
        {
            OneCardManager oneCardManager = IDHolder.GetGameObjectWithID(cardId).GetComponent<OneCardManager>();
            if (cardType == SubTypeOfCards.Slash)
            {
                oneCardManager.CanBePlayedNow = (oneCardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
                || oneCardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash
                || oneCardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash);
            }
            else
            {
                oneCardManager.CanBePlayedNow = (oneCardManager.CardAsset.SubTypeOfCard == cardType);
            }
            if (oneCardManager.CardAsset.Targets != TargetingOptions.NoTarget)
            {
                oneCardManager.TargetComponent.SetActive(true);
            }
            else
            {
                oneCardManager.TargetComponent.SetActive(false);
            }
            if (needTargetComponent == false)
            {
                oneCardManager.TargetComponent.SetActive(false);
            }
        }
    }

    public static void ShowACards(Player player)
    {
        HighlightManager.DisableAllCards();
        foreach (int cardId in player.Hand.CardsInHand)
        {
            OneCardManager oneCardManager = IDHolder.GetGameObjectWithID(cardId).GetComponent<OneCardManager>();
            oneCardManager.CanBePlayedNow = true;
            oneCardManager.TargetComponent.SetActive(false);
        }
    }

    public static void DisACards(Player player)
    {
        HighlightManager.DisableAllCards();
        foreach (int cardId in player.Hand.CardsInHand)
        {
            OneCardManager oneCardManager = IDHolder.GetGameObjectWithID(cardId).GetComponent<OneCardManager>();
            oneCardManager.CanBePlayedNow = true;
            oneCardManager.TargetComponent.SetActive(false);
        }
    }

    public static bool CanBePlayedNow(OneCardManager oneCardManager)
    {
        switch (oneCardManager.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Slash:
            case SubTypeOfCards.ThunderSlash:
            case SubTypeOfCards.FireSlash:
                Debug.Log("最大杀的限制~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + CounterManager.Instance.SlashLimit);
                if (CounterManager.Instance.SlashCount < CounterManager.Instance.SlashLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case SubTypeOfCards.Jink:
            case SubTypeOfCards.Impeccable:
                return false;
            case SubTypeOfCards.Analeptic:
                if (CounterManager.Instance.UsedAnalepticThisTurn)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            case SubTypeOfCards.Peach:
                if (TurnManager.Instance.whoseTurn.Health == TurnManager.Instance.whoseTurn.PArea.Portrait.TotalHealth)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            default:
                return true;
        }
    }
}
