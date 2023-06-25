using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Threading.Tasks;

public class UseCardManager : MonoBehaviour
{
    public static UseCardManager Instance;
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
    public async void UseAVisualCardFromHand(OneCardManager playedCard, List<int> targets)
    {
        // 出杀次数Hook
        CounterManager.Instance.AddSlashUsedCount(playedCard);
        // 默认赋值目标
        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Peach:
                // 濒死情况下需要判断
                // TODO 濒死求桃的目标是濒死角色
                if (!DyingManager.Instance.IsInDyingInquiry)
                {
                    targets.Add(playedCard.Owner.ID);
                }
                else
                {
                    targets.Add(DyingManager.Instance.DyingPlayer.ID);
                }
                break;
            case SubTypeOfCards.Analeptic:
                targets.Add(playedCard.Owner.ID);
                break;
            case SubTypeOfCards.Wuzhongshengyou:
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

        await SkillManager.StartPlayACard(playedCard);
        Debug.Log("----------------------------------------use a card phase 1------------------------------------------");

        playedCard.TargetsPlayerIDs = targets;

        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Jiedaosharen:
                {
                    TargetsSelectManager.HandleSpecialTarget(playedCard);
                    //阻塞
                    await TaskManager.Instance.BlockTask(TaskType.SpecialTargetsTask);
                }
                break;
            case SubTypeOfCards.Tiesuolianhuan:
                {
                    TargetsSelectManager.HandleMoreTargets(playedCard);
                    //阻塞
                    await TaskManager.Instance.BlockTask(TaskType.SpecialTargetsTask);
                }
                break;
        }

        switch (playedCard.CardLocation)
        {
            case CardLocation.Hand:
                {
                    playedCard.Owner.Hand.DisCard(playedCard.UniqueCardID);

                    playedCard.Owner.PArea.HandVisual.UseASpellFromHand(playedCard.UniqueCardID);
                }
                break;
            case CardLocation.UnderCart:
                {
                    playedCard.Owner.TreasureLogic.DisCard(playedCard.UniqueCardID);

                    playedCard.Owner.PArea.TreasureVisual.UseASpellFromTreasure(playedCard.UniqueCardID);
                }
                break;
        }
        // remove this card from hand

    }

    // 2
    /// <summary>
    /// 牌的位置，从手牌，改为pending
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="target"></param>
    public async void HandleTargets(OneCardManager playedCard, List<int> targets, List<int> specialIds)
    {
        //模拟借刀杀人多个目标的技能。
        //await MultipleTargetsForJiedaosharen(playedCard);
        await SkillManager.StartHandleTargets(playedCard);
        // 默认目标
        if (targets.Count != 0)
        {
            TargetsManager.Instance.SetTargets(targets);
        }
        // 指定了借刀目标
        if (specialIds.Count != 0)
        {
            TargetsManager.Instance.SpecialTarget.AddRange(specialIds);
        }

        // TODO 指定目标时：牌指定了默认目标后，有些技能，可以增加或减少目标的个数，甚至修改牌的 使用者等。
        HandleTargetsOrder(playedCard);
    }

    /// <summary>
    /// 3 处理targets逆向顺序
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
    /// 4 指定目标后：目标已经不变了，在此牌结算前，是否有额外的效果。
    /// </summary>
    /// <param name="playedCard"></param>
    public async void HandleFixedTargets(OneCardManager playedCard)
    {
        Debug.Log("阶段4");
        await SkillManager.StartFixedTargets(playedCard);
        Debug.Log("继续第五步");
        HandleImpeccable(playedCard);
    }

    /// <summary>
    /// 5 执行卡牌统一入口脚本效果
    /// </summary>
    /// <param name="playedCard"></param>
    /// TODO 这里写无懈
    public void HandleImpeccable(OneCardManager playedCard)
    {
        //// 先进入无懈流程，之后再进入触发锦囊效果阶段
        if (playedCard.CardAsset.TypeOfCard == TypesOfCards.Tips && GlobalSettings.Instance.OneKeyImpeccable == false)
        {
            ImpeccableManager.Instance.StartInquireNextTarget();
        }
        else
        {
            ActiveEffect(playedCard);
        }

    }

    /// <summary>
    /// 6 执行卡牌效果
    /// </summary>
    /// <param name="playedCard"></param>
    public async void ActiveEffect(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card:" + cardAsset.SubTypeOfCard);
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with attribute:" + cardAsset.SpellAttribute);
        if (TargetsManager.Instance.Targets.Count > 0)
        {
            Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~play one card with targets:" + TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count);
        }
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
                                OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
                                if (cardManager != null)
                                {
                                    await SkillManager.BeforeNeedPlayAJink();
                                    Debug.Log("需要出闪");
                                    NeedToPlayJink();
                                }
                            }
                            break;
                        case SubTypeOfCards.Peach:
                            {
                                Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
                                GlobalSettings.Instance.Table.ClearCards(playedCard.UniqueCardID);
                                //恢复一点体力
                                HealthManager.Instance.HealingEffect(1, targetPlayer);
                                HealthManager.Instance.SettleAfterHealing();
                            }
                            break;
                        // TODO 1.酒的红色高亮 2.濒死把酒对自己当桃
                        case SubTypeOfCards.Analeptic:
                            {
                                CounterManager.Instance.UsedAnalepticThisTurn = true;
                                Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
                                targetPlayer.ChangePortraitColor(Color.red);
                                HighlightManager.EnableCardsWithType(targetPlayer);
                                FinishSettle();
                            }
                            break;
                    }
                }
                break;
            case TypesOfCards.Tips:
                TipCardManager.Instance.ActiveTipCard();
                break;
            case TypesOfCards.Equipment:
                break;
        }
    }


    /// <summary>
    /// 完成结算
    /// </summary>
    public void FinishSettle()
    {
        //移除另外目标的玩家
        if (TargetsManager.Instance.SpecialTarget.Count > 0)
        {
            TargetsManager.Instance.SpecialTarget.RemoveAt(0);
        }
        //移除已经结算完伤害的玩家
        if (TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count > 0)
        {
            TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].RemoveAt(0);
        }
        Debug.Log("还有几个牌" + TargetsManager.Instance.Targets.Count);
        Debug.Log("还有几个目标" + TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1]);
        if (TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count == 0)
        {
            //去除所有目标高亮
            HighlightManager.DisableAllTargetsGlow();
            HighlightManager.DisableAllOpButtons();
            //移除卡
            GlobalSettings.Instance.Table.ClearCardsFromLast();
            if (TargetsManager.Instance.Targets.Count == 0)
            {
                //高亮当前回合人
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
            else
            {
                ActiveLastOneCardOnTable();
            }
        }
        else
        {
            ActiveLastOneCardOnTable();
        }
    }

    public void ActiveLastOneCardOnTable()
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();

        if (cardManager.CardAsset.SubTypeOfCard == SubTypeOfCards.Jiedaosharen)
        {
            TipCardManager.Instance.JiedaoSharenNextTarget();
        }
        else
        {
            Player nextTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
            UseCardManager.Instance.HandleImpeccable(cardManager);
        }
    }

    /// <summary>
    /// 需要出闪
    /// </summary>
    public async void NeedToPlayJink(Player targetPlayer = null)
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        if (targetPlayer == null)
        {
            int targetId = TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0];
            targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);
        }

        HighlightManager.EnableCardWithCardType(targetPlayer, SubTypeOfCards.Jink);
        targetPlayer.ShowOp1Button = true;
        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            targetPlayer.ShowOp1Button = false;
            SettleManager.Instance.StartSettle();
        });

        await SkillManager.NeedPlayAJink(targetPlayer);
    }

    /// <summary>
    /// 需要出杀
    /// </summary>
    public async void NeedToPlaySlash(Player targetPlayer = null, bool isJiedaoSharen = false, bool needTarget = false)
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        if (targetPlayer == null)
        {
            int targetId = TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0];
            targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);
        }

        HighlightManager.EnableCardWithCardType(targetPlayer, SubTypeOfCards.Slash, needTargetComponent: isJiedaoSharen || needTarget);
        targetPlayer.ShowOp1Button = true;
        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            Debug.Log("dont play slash~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + TargetsManager.Instance.Targets[0].Count);
            targetPlayer.ShowOp1Button = false;
            //借刀杀人需要处理
            if (isJiedaoSharen)
            {
                TipCardManager.Instance.GiveJiedaoSharenWeapon();
            }
            else
            {
                SettleManager.Instance.StartSettle();
            }
        });

        await SkillManager.NeedToPlaySlash(targetPlayer, needTarget);
    }

    public void BackToWhoseTurn()
    {
        HighlightManager.DisableAllTargetsGlow();
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        GlobalSettings.Instance.Table.ClearCardsFromLast();
        TargetsManager.Instance.SpecialTarget.Clear();
        HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
    }

    //模拟借刀杀人多个目标的情况
    public async Task MultipleTargetsForJiedaosharen(OneCardManager playedCard)
    {
        if (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Jiedaosharen && playedCard.SpecialTargetPlayerIDs.Count <= 1 && GlobalSettings.Instance.TestJiedaoShaRenMultiplayers == true)
        {
            HighlightManager.DisableAllCards();
            HighlightManager.DisableAllOpButtons();

            foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
            {
                if (targetPlayer.ID != playedCard.Owner.ID && !playedCard.TargetsPlayerIDs.Contains(targetPlayer.ID))
                {
                    (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Weapons);
                    //判断借刀对象是否有武器
                    if (hasWeapon)
                    {
                        Debug.Log("有武器");
                        targetPlayer.ShowOp1Button = true;
                        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                        {
                            HighlightManager.DisableAllOpButtons();
                            playedCard.TargetsPlayerIDs.Add(targetPlayer.ID);
                            targetPlayer.PArea.Portrait.TargetComponent.GetComponent<DragOnTarget>().CardManager = playedCard;
                            targetPlayer.ShowJiedaosharenTarget = true;
                        });
                    }
                }
            }

            await TaskManager.Instance.BlockTask(TaskType.SpecialTargetsTask);
        }
    }

}
