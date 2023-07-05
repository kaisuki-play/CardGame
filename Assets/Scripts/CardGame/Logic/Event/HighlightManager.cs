using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

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
            //手牌区
            foreach (int cardId in player.Hand.CardsInHand)
            {
                OneCardManager oneCardManager = IDHolder.GetGameObjectWithID(cardId).GetComponent<OneCardManager>();
                oneCardManager.CanBePlayedNow = false;
            }
            //装备区
            foreach (int cardId in player.TreasureLogic.CardsInTreasure)
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

    public static async void EnableCardsWithType(Player player, CardsType cardsType = CardsType.Normal)
    {
        //TODO 当前人出牌人死了
        HighlightManager.DisableAllCards();
        //手牌区
        foreach (int cardId in player.Hand.CardsInHand)
        {
            EnableSingleCard(player, cardId);
        }
        //宝物区
        foreach (int cardId in player.TreasureLogic.CardsInTreasure)
        {
            EnableSingleCard(player, cardId);
        }

        if (CounterManager.Instance.SlashCount < CounterManager.Instance.SlashLimit)
        {
            await SkillManager.NeedToPlaySlash(player, true);
        }
        //List<HeroSkillInfo> skillList = HeroSkillRegister.SkillRegister[player.ID];
        //bool hasSkill = false;
        //foreach (HeroSkillInfo heroSkillInfo in skillList)
        //{
        //    if (heroSkillInfo.SkillType == HeroSkillType.OsirisSkill1)
        //    {
        //        hasSkill = true;
        //        break;
        //    }
        //}
        //if (hasSkill)
        //{
        //    await HeroSkillManager.ActiveOsirisSkill1(player);
        //}
        await HeroSkillRegister.PriorityHeroSkill(HeroSkillActivePhase.HookHightlight);
    }

    public static void EnableCardWithCardType(Player player, SubTypeOfCards cardType, bool needTargetComponent = true)
    {
        HighlightManager.DisableAllCards();
        //手牌区
        foreach (int cardId in player.Hand.CardsInHand)
        {
            EnableSingleCardWithType(player, cardId, cardType, needTargetComponent);
        }
        //宝物区
        foreach (int cardId in player.TreasureLogic.CardsInTreasure)
        {
            EnableSingleCardWithType(player, cardId, cardType, needTargetComponent);
        }
    }

    /// <summary>
    /// 没有目标组件高亮所有手牌
    /// </summary>
    /// <param name="player"></param>
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
        if (HeroSkillState.HeroSkillCardTypeDic_Once.ContainsKey(HeroSKillStateKey.YangxiuSkill3State))
        {
            TypesOfCards typesOfCard = HeroSkillState.HeroSkillCardTypeDic_Once[HeroSKillStateKey.YangxiuSkill3State];
            if (oneCardManager.CardAsset.TypeOfCard == typesOfCard)
            {
                return false;
            }
        }
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

    public static void EnableSingleCard(Player player, int cardId)
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

    public static void EnableSingleCardWithType(Player player, int cardId, SubTypeOfCards cardType, bool needTargetComponent = true)
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
        if (DyingManager.Instance.IsInDyingInquiry)
        {
            if (cardType == SubTypeOfCards.Peach && DyingManager.Instance.DyingPlayer.ID == player.ID)
            {
                EnableCardWithCardType(player, SubTypeOfCards.Analeptic);
            }
        }
    }
}
