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

    // 1
    /// <summary>
    /// 使用牌：当一个玩家，点了一张手牌，然后点了默认的目标，则视为玩家已经使用了此牌。
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="target"></param>
    public void PlayAVisualCardFromHand(OneCardManager playedCard, List<int> targets)
    {
        // 默认赋值目标
        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Peach:
                //TODO 濒死情况下目标是looser
                targets.Add(playedCard.Owner.ID);
                break;
            case SubTypeOfCards.Wuzhongshengyou:
            case SubTypeOfCards.Thunder:
                targets.Add(playedCard.Owner.ID);
                break;
            case SubTypeOfCards.Nanmanruqin:
            case SubTypeOfCards.Wanjianqifa:
                foreach (Player player in GlobalSettings.Instance.PlayerInstances)
                {
                    if (player.ID != playedCard.Owner.ID)
                    {
                        targets.Add(player.ID);
                    }
                }
                break;
            case SubTypeOfCards.Wugufengdeng:
            case SubTypeOfCards.Taoyuanjieyi:
                foreach (Player player in GlobalSettings.Instance.PlayerInstances)
                {
                    targets.Add(player.ID);
                }
                break;
        }

        //借刀杀人单独出来1.5流程
        if (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Jiedaosharen)
        {
            playedCard.SpecialTargetPlayerID = targets[0];

            HandleSpecialTarget(playedCard);
        }
        else
        {
            playedCard.TargetsPlayerIDs = targets;

            // remove this card from hand
            playedCard.Owner.Hand.DisCard(playedCard.UniqueCardID);

            playedCard.Owner.PArea.HandVisual.PlayASpellFromHand(playedCard.UniqueCardID);
        }
    }

    public void HandleSpecialTarget(OneCardManager playedCard)
    {
        Player specialTargetPlayer = GlobalSettings.Instance.FindPlayerByID(playedCard.SpecialTargetPlayerID);
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(specialTargetPlayer, TypeOfEquipment.Weapons);
        //判断借刀对象是否有武器
        if (hasWeapon)
        {
            Debug.Log("有武器");
            specialTargetPlayer.PArea.Portrait.TargetComponent.GetComponent<DragOnTarget>().CardManager = playedCard;
            specialTargetPlayer.ShowJiedaosharenTarget = true;
        }
    }

    public void HandleJiedaosharen(OneCardManager playedCard, List<int> targets)
    {
        playedCard.TargetsPlayerIDs = targets;

        // remove this card from hand
        playedCard.Owner.Hand.DisCard(playedCard.UniqueCardID);

        playedCard.Owner.PArea.HandVisual.PlayASpellFromHand(playedCard.UniqueCardID);
    }

    // 2
    /// <summary>
    /// 牌的位置，从手牌，改为pending
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="target"></param>
    public void HandleTargets(OneCardManager playedCard, List<int> targets, int specialTarget = -1)
    {
        // 默认目标
        if (targets.Count != 0)
        {
            TargetsManager.Instance.SetTargets(targets);
        }
        // 指定了借刀目标
        TargetsManager.Instance.SpecialTarget = specialTarget;

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
                                    switch (cardManager.CardAsset.TypeOfCard)
                                    {
                                        case TypesOfCards.Tips:
                                            {
                                                switch (cardManager.CardAsset.SubTypeOfCard)
                                                {
                                                    case SubTypeOfCards.Juedou:
                                                        TipCardManager.Instance.PlayCardOwner = playedCard.Owner;
                                                        TipCardManager.Instance.ActiveTipCard();
                                                        break;
                                                    default:
                                                        TipCardManager.Instance.SkipTipCard();
                                                        break;
                                                }
                                            }
                                            break;
                                        default:
                                            NeedToPlayJink();
                                            break;
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
                                    switch (cardManager.CardAsset.TypeOfCard)
                                    {
                                        case TypesOfCards.Tips:
                                            {
                                                TipCardManager.Instance.SkipTipCard();
                                            }
                                            break;
                                        default:
                                            {
                                                PlayCardManager.Instance.BackToWhoseTurn();
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        case SubTypeOfCards.Peach:
                            {
                                //是否是濒死流程
                                if (DyingManager.Instance.IsInDyingInquiry)
                                {
                                    DyingManager.Instance.Healing();
                                }
                                else
                                {
                                    if (TargetsManager.Instance.Targets.Count > 0)
                                    {
                                        GlobalSettings.Instance.Table.ClearCards();
                                        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0]);
                                        //恢复一点体力
                                        HealthManager.Instance.HealingEffect(1, targetPlayer);
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
    public void NeedToPlaySlash(Player targetPlayer = null)
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        if (targetPlayer == null)
        {
            int targetId = TargetsManager.Instance.Targets[0];
            targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);
        }

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
