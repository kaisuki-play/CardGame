using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayCardManager : MonoBehaviour
{
    public static PlayCardManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // 2
    /// <summary>
    /// 牌的位置，从手牌，改为pending
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="target"></param>
    public void HandleTargets(OneCardManager playedCard, List<int> targets)
    {
        //switch (playedCard.CardAsset.SubTypeOfCard)
        //{
        //    case SubTypeOfCards.Peach:
        //    case SubTypeOfCards.Wuzhongshengyou:
        //        targets.Add(playedCard.Owner.ID);
        //        break;
        //    case SubTypeOfCards.Nanmanruqin:
        //    case SubTypeOfCards.Wanjianqifa:
        //        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        //        {
        //            if (player.ID != playedCard.Owner.ID)
        //            {
        //                targets.Add(player.ID);
        //            }
        //        }
        //        break;
        //    case SubTypeOfCards.Wugufengdeng:
        //    case SubTypeOfCards.Taoyuanjieyi:
        //        foreach (Player player in GlobalSettings.Instance.PlayerInstances)
        //        {
        //            targets.Add(player.ID);
        //        }
        //        break;
        //}

        // 默认目标
        if (targets.Count != 0)
        {
            TargetsManager.Instance.SetTargets(targets);
        }

        // TODO 指定目标时：牌指定了默认目标后，有些技能，可以增加或减少目标的个数，甚至修改牌的 使用者等。
        HandleTargetsOrder(playedCard);

    }

    /// <summary>
    /// 3处理targets逆向顺序
    /// </summary>
    /// <param name="playedCard"></param>
    public void HandleTargetsOrder(OneCardManager playedCard)
    {
        if (playedCard.TargetsPlayerIDs.Count > 1)
        {
            TargetsManager.Instance.Order(playedCard);
        }
        HandleFixedTargets(playedCard);
    }

    /// <summary>
    /// 指定目标后：目标已经不变了，在此牌结算前，是否有额外的效果。
    /// </summary>
    /// <param name="playedCard"></param>
    public void HandleFixedTargets(OneCardManager playedCard)
    {
        HandleSpellFromHand(playedCard);
    }

    /// <summary>
    /// 执行卡牌统一入口脚本效果
    /// </summary>
    /// <param name="playedCard"></param>
    public void HandleSpellFromHand(OneCardManager playedCard)
    {
        ActiveEffect(playedCard);
    }

    /// <summary>
    /// 执行卡牌效果
    /// </summary>
    /// <param name="playedCard"></param>
    public void ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with attribute:" + cardAsset.SpellAttribute);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with targets:" + TargetsManager.Instance.Targets.Count);
        // restart timer
        TurnManager.Instance.RestartTimer();

        switch (cardAsset.TypeOfCard)
        {
            case TypesOfCards.Base:
                {
                    switch (cardAsset.SubTypeOfCard)
                    {
                        // 出杀
                        case SubTypeOfCards.Slash:
                        case SubTypeOfCards.FireSlash:
                        case SubTypeOfCards.ThunderSlash:
                            {
                                OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();
                                if (cardManager != null)
                                {
                                    if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips)
                                    {
                                        TipCardManager.Instance.SkipTipCard();
                                    }
                                    else
                                    {
                                        NeedToPlayJink();
                                    }
                                }
                            }
                            break;
                        // 出闪
                        case SubTypeOfCards.Jink:
                            {
                                OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();
                                if (cardManager != null)
                                {
                                    if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips)
                                    {
                                        TipCardManager.Instance.SkipTipCard();
                                    }
                                    else
                                    {
                                        PlayCardManager.Instance.BackToWhoseTurn();
                                    }
                                }
                            }
                            break;
                    }
                }
                break;
            case TypesOfCards.Tips:
                // 先进入无懈流程，之后再进入触发锦囊效果阶段
                if (cardAsset.SubTypeOfCard == SubTypeOfCards.Impeccable)
                {
                    ImpeccableManager.Instance.TipWillWork = !ImpeccableManager.Instance.TipWillWork;
                    ImpeccableManager.Instance.RestartInquireTarget();
                }
                else
                {
                    ImpeccableManager.Instance.StartInquireNextTarget();
                }
                break;
            case TypesOfCards.Equipment:
                break;
        }
    }


    /// <summary>
    /// 需要出闪
    /// </summary>
    public void NeedToPlayJink()
    {
        HighlightManager.DisableAllCards();
        int targetId = TargetsManager.Instance.Targets[0];
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);

        HighlightManager.EnableCardWithCardType(targetPlayer, SubTypeOfCards.Jink);
        targetPlayer.ShowOp1Button = true;
        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            targetPlayer.ShowOp1Button = false;
            SettleManager.Instance.StartSettle();
        });
    }

    /// <summary>
    /// 需要出杀
    /// </summary>
    public void NeedToPlaySlash()
    {
        HighlightManager.DisableAllCards();
        int targetId = TargetsManager.Instance.Targets[0];
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);

        HighlightManager.EnableCardWithCardType(targetPlayer, SubTypeOfCards.Slash, needTargetComponent: false);
        targetPlayer.ShowOp1Button = true;
        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            targetPlayer.ShowOp1Button = false;
            SettleManager.Instance.StartSettle();
        });
    }


    public void BackToWhoseTurn()
    {
        HighlightManager.DisableAllTargetsGlow();
        HighlightManager.DisableAllCards();
        GlobalSettings.Instance.Table.ClearCards();
        HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
    }

}
