using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DelayTipManager : MonoBehaviour
{
    public static OneCardManager flopCardManager = null;
    /// <summary>
    /// 出了一张延时锦囊牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public static async Task UseADelayTipsCardFromHand(OneCardManager playedCard, List<int> targets)
    {
        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Thunder:
                targets.Add(playedCard.Owner.ID);
                break;
        }

        playedCard.TargetsPlayerIDs = targets;

        // remove this card from hand
        playedCard.Owner.Hand.DisCard(playedCard.UniqueCardID);

        await playedCard.Owner.PArea.HandVisual.UseASpellFromHand(playedCard.UniqueCardID);
    }

    //是否有延时锦囊
    public static bool HasDelayTips(Player targetPlayer)
    {
        return targetPlayer.JudgementLogic.CardsInJudgement.Count != 0;
    }

    //是否有延时锦囊
    public static bool HasDelayTips(Player targetPlayer, SubTypeOfCards delayTipType)
    {
        foreach (int cardId in targetPlayer.JudgementLogic.CardsInJudgement)
        {
            GameObject card = IDHolder.GetGameObjectWithID(cardId);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.SubTypeOfCard == delayTipType)
            {
                return true;
            }
        }
        return false;
    }

    public static async Task DisDelayTip(Player targetPlayer)
    {
        int lastJudgementCard = targetPlayer.JudgementLogic.CardsInJudgement[targetPlayer.JudgementLogic.CardsInJudgement.Count - 1];
        GameObject judgementCard = IDHolder.GetGameObjectWithID(lastJudgementCard);
        OneCardManager judgementCardManager = judgementCard.GetComponent<OneCardManager>();
        await targetPlayer.DisACardFromJudgement(judgementCardManager.UniqueCardID);
        if (HasDelayTips(targetPlayer))
        {
            ImpeccableManager.Instance.StartInquireNextTarget();
        }
        else
        {
            TaskManager.Instance.UnBlockTask(TaskType.DelayTask);
        }
    }

    //处理延时锦囊
    public static async Task HandleDelayTip(Player targetPlayer)
    {
        int lastJudgementCard = targetPlayer.JudgementLogic.CardsInJudgement[targetPlayer.JudgementLogic.CardsInJudgement.Count - 1];
        GameObject judgementCard = IDHolder.GetGameObjectWithID(lastJudgementCard);
        OneCardManager judgementCardManager = judgementCard.GetComponent<OneCardManager>();

        //翻卡牌
        OneCardManager flopedCard = await TurnManager.Instance.whoseTurn.FlopCard();
        Debug.Log("翻出卡牌的花色:" + flopedCard.CardAsset.Suits);

        DelayTipManager.flopCardManager = flopedCard;

        //TODO 判定生效前
        await SkillManager.BeforeJudgementWork(DelayTipManager.flopCardManager, lastJudgementCard);

        switch (judgementCardManager.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Lebusishu:
                Debug.Log("乐不思蜀");
                if (DelayTipManager.flopCardManager.CardAsset.Suits != CardSuits.Hearts)
                {
                    TurnManager.Instance.SkipPlayCardPhase = true;
                }
                await targetPlayer.DisACardFromJudgement(judgementCardManager.UniqueCardID);
                break;
            case SubTypeOfCards.Binliangcunduan:
                Debug.Log("兵粮寸断");
                if (DelayTipManager.flopCardManager.CardAsset.Suits != CardSuits.Clubs)
                {
                    TurnManager.Instance.SkipDrawCardPhase = true;
                }
                await targetPlayer.DisACardFromJudgement(judgementCardManager.UniqueCardID);
                break;
            case SubTypeOfCards.Thunder:
                Debug.Log("闪电");
                if (DelayTipManager.flopCardManager.CardAsset.Suits == CardSuits.Spades && DelayTipManager.flopCardManager.CardAsset.CardRank != CardRank.Rank_A && DelayTipManager.flopCardManager.CardAsset.CardRank != CardRank.Rank_10 && DelayTipManager.flopCardManager.CardAsset.CardRank != CardRank.Rank_J && DelayTipManager.flopCardManager.CardAsset.CardRank != CardRank.Rank_Q && DelayTipManager.flopCardManager.CardAsset.CardRank != CardRank.Rank_K)
                {
                    TaskManager.Instance.DelayTipTask = new TaskCompletionSource<bool>();
                    await targetPlayer.DisACardFromJudgement(judgementCardManager.UniqueCardID);
                    SettleManager.Instance.StartSettle(judgementCardManager, SpellAttribute.ThunderSlash, TurnManager.Instance.whoseTurn, 3);
                    await TaskManager.Instance.DelayTipTask.Task;
                }
                else
                {
                    await TurnManager.Instance.whoseTurn.PassDelayTipToTarget(TurnManager.Instance.whoseTurn.OtherThunderPlayer, judgementCardManager);
                }

                break;
        }
        if (!HasDelayTips(targetPlayer))
        {
            Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~解除延时锦囊的阻塞");
            TaskManager.Instance.UnBlockTask(TaskType.DelayTask);
        }
        else
        {
            ImpeccableManager.Instance.StartInquireNextTarget();
        }
    }
}
