using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 根据装备类型查找玩家是否有装备
    /// </summary>
    /// <param name="player"></param>
    /// <param name="typeOfEquipment"></param>
    /// <returns></returns>
    public (bool, OneCardManager) HasEquipmentWithType(Player player, TypeOfEquipment typeOfEquipment)
    {
        foreach (int cardId in player.EquipmentLogic.CardsInEquipment)
        {
            GameObject card = IDHolder.GetGameObjectWithID(cardId);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfEquipment == typeOfEquipment)
            {
                return (true, cardManager);
            }
        }
        return (false, null);
    }

    /// <summary>
    /// 穿装备，如果有同类型装备，进行替换操作
    /// </summary>
    /// <param name="player"></param>
    /// <param name="cardManager"></param>
    public async Task AddOrReplaceEquipment(Player player, OneCardManager cardManager)
    {
        (bool hasEquipment, OneCardManager oldEquipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, cardManager.CardAsset.TypeOfEquipment);
        //有同类型的装备，就需要替换
        if (hasEquipment)
        {
            //去旧卡
            player.EquipmentLogic.RemoveCard(oldEquipmentCard.UniqueCardID);
            await player.PArea.EquipmentVisaul.DisCardFromEquipment(oldEquipmentCard.UniqueCardID);

            //加新卡
            player.EquipmentLogic.AddCard(cardManager.UniqueCardID);
            await player.PArea.EquipmentVisaul.EquipWithCard(cardManager.UniqueCardID, player);

        }
        else
        {
            //加新卡
            player.EquipmentLogic.AddCard(cardManager.UniqueCardID);
            await player.PArea.EquipmentVisaul.EquipWithCard(cardManager.UniqueCardID, player);
        }
    }

    /// <summary>
    /// 诸葛连弩hook
    /// </summary>
    /// <param name="player"></param>
    public void ZhugeliannuHook(Player player)
    {
        Debug.Log("诸葛连弩验证                 " + player.PArea.Owner);
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Zhugeliannu)
            {
                Debug.Log("诸葛连弩生效");
                if (player.ID == TurnManager.Instance.whoseTurn.ID)
                {
                    CounterManager.Instance.SlashLimit += 10;
                }
            }
            if (player.ID == TurnManager.Instance.whoseTurn.ID)
            {
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
        }
    }

    /// 失去诸葛连弩hook
    public void ZhugeliannuDisHook(Player player, OneCardManager equipmentCard)
    {
        if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Zhugeliannu)
        {
            Debug.Log("诸葛连弩失效");
            if (player.ID == TurnManager.Instance.whoseTurn.ID)
            {
                CounterManager.Instance.SlashLimit -= 10;
            }
        }
        if (player.ID == TurnManager.Instance.whoseTurn.ID)
        {
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
        }
    }

    /// <summary>
    /// 雌雄双股
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task CixiongHook(Player player, OneCardManager playedCard)
    {
        if (GlobalSettings.Instance.Table.CardsOnTable.Count == 0)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[GlobalSettings.Instance.LastOneCardOnTable().UniqueCardID][0]);
        TaskManager.Instance.AddATask(TaskType.CixiongShuangguTask);

        Debug.Log("-------------------------------");
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        Debug.Log("-------------------------------" + hasEquipment);
        if (hasEquipment && (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash))
        {
            //TODO 双武将 男男 男女判断
            if (player.CharAsset.CharSex == targetPlayer.CharAsset.CharSex)
            {
                await TaskManager.Instance.DontAwait();
                return;
            }
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.CixiongDoubleSwards)
            {
                Debug.Log("不解除");
                HighlightManager.DisableAllCards();
                HighlightManager.DisableAllOpButtons();
                player.ShowOp1Button = true;
                player.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp1ButtonText("发动雌雄双股剑");
                player.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    HandleCixiong(player, targetPlayer);
                });

                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("不发动雌雄双股剑");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.CixiongShuangguTask);
                });

                try
                {
                    await TaskManager.Instance.TaskBlockDic[TaskType.CixiongShuangguTask][0].Task;
                }
                catch (Exception ex)
                {
                    Debug.Log("task exception :" + ex);
                }
            }
            else
            {
                Debug.Log("解除");
                await TaskManager.Instance.DontAwait();
            }
        }
        else
        {
            Debug.Log("解除");
            await TaskManager.Instance.DontAwait();
        }
    }

    /// 目标选择弃一张牌，或者让对方摸一张牌
    public void HandleCixiong(Player player, Player targetPlayer)
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        targetPlayer.ShowOp1Button = true;
        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.ChangeOp1ButtonText("弃一张牌");
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            DisCardForCixiong(targetPlayer);
        });

        //若没有牌则不能选择弃一张牌
        if (targetPlayer.Hand.CardsInHand.Count == 0)
        {
            targetPlayer.PArea.Portrait.OpButton1.enabled = false;
        }

        targetPlayer.ShowOp2Button = true;
        targetPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.ChangeOp2ButtonText("对方摸一张牌");
        targetPlayer.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            await player.DrawSomeCards(1);
            TaskManager.Instance.UnBlockTask(TaskType.CixiongShuangguTask);
        });
    }

    /// 弃一张牌
    public void DisCardForCixiong(Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            TaskManager.Instance.UnBlockTask(TaskType.CixiongShuangguTask);
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    /// <summary>
    /// 贯石斧
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task GuanshifuHook(OneCardManager playedCard, Player targetPlayer)
    {
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash
            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        Player player = playedCard.Owner;
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Guanshifu)
            {
                TaskManager.Instance.AddATask(TaskType.GuanshifuTask);

                Debug.Log("不解除");
                HighlightManager.DisableAllCards();
                HighlightManager.DisableAllOpButtons();
                player.ShowOp1Button = true;
                player.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp1ButtonText("发动贯石斧");
                player.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.ExceptionBlockTask(TaskType.GuanshifuTask);
                    DisCardForGuanshifu(player);
                });

                int handCount = player.Hand.CardsInHand.Count;
                int equipmentCount = player.EquipmentLogic.CardsInEquipment.Count;
                if (handCount + equipmentCount < 2)
                {
                    player.PArea.Portrait.OpButton1.enabled = false;
                }

                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("不发动贯石斧");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    UseCardManager.Instance.BackToWhoseTurn();
                });

                await TaskManager.Instance.TaskBlockDic[TaskType.GuanshifuTask][0].Task;
            }
            else
            {
                UseCardManager.Instance.BackToWhoseTurn();
                await TaskManager.Instance.DontAwait();
            }
        }
        else
        {
            UseCardManager.Instance.BackToWhoseTurn();
            await TaskManager.Instance.DontAwait();
        }
    }

    //贯石斧弃两张牌造成伤害
    public void DisCardForGuanshifu(Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 2;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            await SettleManager.Instance.StartSettle();
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = targetPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    /// <summary>
    /// 青釭剑hook
    /// </summary>
    /// <param name="player"></param>
    public async Task QinggangjianHook(Player player)
    {
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Qinghongjian)
            {
                Debug.Log("青釭剑生效");
                player.IgnoreArmor = true;
            }
            else
            {
                player.IgnoreArmor = false;
            }
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 丈八蛇矛
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public void ZhangbashemaoHook(Player player, bool needTarget)
    {
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Zhangbashemao)
            {
                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动丈八蛇矛");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    SelectCardForZhangbashemao(player, needTarget);
                });

                int handCount = player.Hand.CardsInHand.Count;
                if (handCount < 2)
                {
                    player.PArea.Portrait.OpButton2.enabled = false;
                }
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
    }

    //贯石斧弃两张牌造成伤害
    public void SelectCardForZhangbashemao(Player targetPlayer, bool needTarget)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.UseSomeCardAsSlash;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 2;
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardAsOtherCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterSelectCardAsOtherCardCompletion = null;
            OneCardManager cardManager = GlobalSettings.Instance.PDeck.DisguisedCardAssetWithType(targetPlayer, SubTypeOfCards.Slash, GlobalSettings.Instance.CardSelectVisual.SelectCardIds, needTarget);
            cardManager.CanBePlayedNow = true;
            //GlobalSettings.Instance.CardSelectVisual.SelectCardIds.Clear();
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    /// <summary>
    /// 青龙偃月刀
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task QinglongyanyueHook(OneCardManager playedCard, Player targetPlayer)
    {
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash
            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        Player player = playedCard.Owner;
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Qinglongyanyuedao)
            {
                //TODO 是否是闲置状态 进入技能就默认为false
                if (!TaskManager.Instance.TaskBlockDic.ContainsKey(TaskType.QinglongyanyueTask))
                {
                    OneCardManager lastCardOnTable = GlobalSettings.Instance.LastOneCardOnTable();
                    if (lastCardOnTable != null)
                    {
                        if (TargetsManager.Instance.TargetsDic.ContainsKey(lastCardOnTable.UniqueCardID))
                        {
                            TargetsManager.Instance.TargetsDic[lastCardOnTable.UniqueCardID].RemoveAt(0);
                            if (TargetsManager.Instance.TargetsDic[lastCardOnTable.UniqueCardID].Count == 0)
                            {
                                TargetsManager.Instance.TargetsDic.Remove(lastCardOnTable.UniqueCardID);
                            }
                        }
                        await GlobalSettings.Instance.Table.ClearAllCardsWithNoTargets();
                    }
                }
                TaskManager.Instance.AddATask(TaskType.QinglongyanyueTask);
                Debug.Log("不解除");
                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动追杀");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    Debug.Log("之前是否有阻塞~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + TaskManager.Instance.TaskBlockDic.ContainsKey(TaskType.QinglongyanyueTask));
                    HighlightManager.DisableAllOpButtons();
                    TargetsManager.Instance.DefaultTarget.Add(targetPlayer.ID);
                    UseCardManager.Instance.NeedToPlaySlash(EventEnum.QinglongyanyuedaoNeedToPlaySlash, player);
                });

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动追杀");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.QinglongyanyueTask);
                });
                await TaskManager.Instance.TaskBlockDic[TaskType.QinglongyanyueTask][0].Task;
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 触发方天画戟
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public async Task ActiveFangtianhuaji(OneCardManager playedCard)
    {
        Player player = playedCard.Owner;
        //没有手牌的时候
        if (player.Hand.CardsInHand.Count > 0)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Fangtianhuaji)
            {
                Debug.Log("不解除");
                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动方天画戟");
                player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
                {
                    HighlightManager.DisableAllOpButtons();
                    await MultipleTargetsForFangtianhuaji(playedCard);
                });

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.FangtianhuajiTask);
                });
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
        await TaskManager.Instance.BlockTask(TaskType.FangtianhuajiTask);
    }

    /// <summary>
    /// 方天画戟多选目标
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public async Task MultipleTargetsForFangtianhuaji(OneCardManager playedCard)
    {
        if (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash)
        {
            (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);

            //有武器但是武器不是方天画戟
            if (!hasWeapon || (hasWeapon && weaponCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Fangtianhuaji))
            {
                TaskManager.Instance.UnBlockTask(TaskType.FangtianhuajiTask);
                await TaskManager.Instance.DontAwait();
                return;
            }
            HighlightManager.DisableAllCards();
            HighlightManager.DisableAllOpButtons();

            int SelectTargetNumber = 0;

            foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
            {
                if (targetPlayer.ID != playedCard.Owner.ID && !playedCard.TargetsPlayerIDs.Contains(targetPlayer.ID))
                {
                    targetPlayer.ShowOp1Button = true;
                    targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                    targetPlayer.PArea.Portrait.ChangeOp1ButtonText("多选对象");
                    targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                    {
                        targetPlayer.ShowOp1Button = false;
                        playedCard.TargetsPlayerIDs.Add(targetPlayer.ID);
                        SelectTargetNumber++;
                        if (SelectTargetNumber == 2)
                        {
                            TaskManager.Instance.UnBlockTask(TaskType.FangtianhuajiTask);
                        }
                    });
                }
            }

            TurnManager.Instance.whoseTurn.ShowOp2Button = true;
            TurnManager.Instance.whoseTurn.PArea.Portrait.ChangeOp2ButtonText("完成");
            TurnManager.Instance.whoseTurn.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            TurnManager.Instance.whoseTurn.PArea.Portrait.OpButton2.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TurnManager.Instance.whoseTurn.ShowOp1Button = false;
                TaskManager.Instance.UnBlockTask(TaskType.FangtianhuajiTask);
            });
        }
    }

    /// <summary>
    /// 触发朱雀羽扇
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public async Task ActiveZhuqueyushan(OneCardManager playedCard)
    {
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        Player player = playedCard.Owner;
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Zhuqueyushan)
            {
                Debug.Log("不解除");
                TaskManager.Instance.AddATask(TaskType.ZhuqueyushanTask);

                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动朱雀羽扇");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    playedCard.CardAsset.SpellAttribute = SpellAttribute.FireSlash;
                    TaskManager.Instance.UnBlockTask(TaskType.ZhuqueyushanTask);
                });

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.ZhuqueyushanTask);
                });

                await TaskManager.Instance.TaskBlockDic[TaskType.ZhuqueyushanTask][0].Task;
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
    }

    /// <summary>
    /// 触发寒冰剑
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public async Task ActiveFrostBlade(OneCardManager playedCard, Player targetPlayer)
    {
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
            || playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash
            || playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash)
        {
            await TaskManager.Instance.DontAwait();
        }
        Player player = playedCard.Owner;
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.FrostBlade)
            {
                Debug.Log("不解除");
                TaskManager.Instance.AddATask(TaskType.FrostBladeTask);

                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动寒冰剑");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    SelectCardForFrostBlade(targetPlayer);
                });

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.FrostBladeTask);
                });

                await TaskManager.Instance.TaskBlockDic[TaskType.FrostBladeTask][0].Task;
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
    }


    //寒冰剑弃两张手牌，防止伤害
    public void SelectCardForFrostBlade(Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 2;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            TaskManager.Instance.ExceptionBlockTask(TaskType.FrostBladeTask, "寒冰剑生效");
            await UseCardManager.Instance.FinishSettle();
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = targetPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    /// <summary>
    /// 麒麟弓触发
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task ActiveQilingong(OneCardManager playedCard, Player targetPlayer)
    {
        //如果不是杀则不触发
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
             && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash
             && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }

        Player player = playedCard.Owner;
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Qilingong)
            {
                Debug.Log("不解除");
                TaskManager.Instance.AddATask(TaskType.QilingongTask);

                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动麒麟弓");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    SelectCardForQilingong(targetPlayer);
                });

                bool hasAddAHorse = HasEquipmentWithType(targetPlayer, TypeOfEquipment.AddAHorse).Item1;
                bool hasMinusAHorse = HasEquipmentWithType(targetPlayer, TypeOfEquipment.MinusAHorse).Item1;
                if (!hasAddAHorse && !hasMinusAHorse)
                {
                    player.PArea.Portrait.OpButton2.enabled = false;
                }

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.QilingongTask);
                });

                await TaskManager.Instance.TaskBlockDic[TaskType.QilingongTask][0].Task;
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
    }

    //发动麒麟弓
    public void SelectCardForQilingong(Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 1;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            TaskManager.Instance.UnBlockTask(TaskType.QilingongTask);
        };

        for (int i = targetPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfEquipment == TypeOfEquipment.AddAHorse || cardManager.CardAsset.TypeOfEquipment == TypeOfEquipment.MinusAHorse)
            {
                GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
            }
        }
    }

    /// <summary>
    /// 银月枪
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public async Task ActiveSilverMoon(OneCardManager playedCard)
    {
        Player player = playedCard.Owner;
        //不要是本回合出牌
        if (player.ID == TurnManager.Instance.whoseTurn.ID)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        //使用牌为黑色
        if (playedCard.CardAsset.Suits != CardSuits.Spades && playedCard.CardAsset.Suits != CardSuits.Clubs)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.SilverMoon)
            {
                Debug.Log("不解除");
                TaskManager.Instance.AddATask(TaskType.SilverMoonTask);

                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动银月枪");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    SelectOtherToPlayJink(player);
                });

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.SilverMoonTask);
                });

                await TaskManager.Instance.TaskBlockDic[TaskType.SilverMoonTask][0].Task;
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
    }

    public void SelectOtherToPlayJink(Player player)
    {
        foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
        {
            if (targetPlayer.ID != player.ID)
            {
                targetPlayer.ShowOp1Button = true;
                targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                targetPlayer.PArea.Portrait.ChangeOp1ButtonText("多选对象");
                targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    UseCardManager.Instance.NeedToPlayJinkNew(EventEnum.SilverMoonNeedToPlayJink, targetPlayer);
                });
            }
        }
    }

    /// <summary>
    /// 八卦阵
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task ActiveBaguazhen(OneCardManager playedCard, Player targetPlayer)
    {
        if (playedCard.Owner.IgnoreArmor)
        {
            await TaskManager.Instance.DontAwait();
        }
        else
        {
            (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
            if (hasEquipment)
            {
                if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Baguazhen)
                {
                    Debug.Log("不解除");

                    TaskManager.Instance.AddATask(TaskType.BaguazhenTask);
                    //HighlightManager.DisableAllOpButtons();
                    targetPlayer.ShowOp2Button = true;
                    targetPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                    targetPlayer.PArea.Portrait.ChangeOp2ButtonText("发动八卦阵");
                    targetPlayer.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        HighlightManager.DisableAllCards();
                        //翻卡牌
                        OneCardManager flopedCard = await TurnManager.Instance.whoseTurn.FlopCard();
                        Debug.Log("翻出卡牌的花色:" + flopedCard.CardAsset.Suits);
                        //判定牌为黑色，无事发生
                        if (flopedCard.CardAsset.Suits == CardSuits.Spades || flopedCard.CardAsset.Suits == CardSuits.Clubs)
                        {
                            targetPlayer.ShowOp2Button = false;
                            //UseCardManager.Instance.NeedToPlayJink(targetPlayer);
                            TaskManager.Instance.UnBlockTask(TaskType.BaguazhenTask);
                        }
                        else
                        {
                            //TODO出闪的动画
                            await UseCardManager.Instance.FinishSettle();
                            TaskManager.Instance.ExceptionBlockTask(TaskType.BaguazhenTask, "八卦阵帮着出了闪");
                        }
                    });

                    targetPlayer.ShowOp3Button = true;
                    targetPlayer.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                    targetPlayer.PArea.Portrait.ChangeOp3Button2Text("不发动");
                    targetPlayer.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        TaskManager.Instance.UnBlockTask(TaskType.BaguazhenTask);
                    });

                    await TaskManager.Instance.TaskBlockDic[TaskType.BaguazhenTask][0].Task;
                }
                else
                {
                    Debug.Log("解除");
                    targetPlayer.ShowOp2Button = false;
                }
            }
            else
            {
                Debug.Log("解除");
                targetPlayer.ShowOp2Button = false;
            }
        }
    }

    /// <summary>
    /// 触发仁王盾
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task ActiveRenwangdun(OneCardManager playedCard, Player targetPlayer)
    {
        if (playedCard.Owner.IgnoreArmor)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        //如果不是杀则不触发
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
             && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash
             && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }

        //使用牌为黑色
        if (playedCard.CardAsset.Suits != CardSuits.Spades && playedCard.CardAsset.Suits != CardSuits.Clubs)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }

        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Renwangdun)
            {
                //TaskManager.Instance.AddATask(TaskType.RenwangdunTask);

                //直接进入结算
                await UseCardManager.Instance.FinishSettle();

                //TaskManager.Instance.ExceptionBlockTask(TaskType.RenwangdunTask);

                //await TaskManager.Instance.TaskBlockDic[TaskType.RenwangdunTask].Task;
                await TaskManager.Instance.ReturnException("黑杀无效");
            }
            else
            {
                Debug.Log("解除");
                targetPlayer.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            targetPlayer.ShowOp2Button = false;
        }
    }

    /// <summary>
    /// 藤甲
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task ActiveTengjia(OneCardManager playedCard, Player targetPlayer)
    {
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
        //南蛮入侵、万箭齐发直接跳过
        if (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Nanmanruqin || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Wanjianqifa)
        {
            if (hasEquipment && equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Tengjia)
            {
                //直接进入结算
                await UseCardManager.Instance.FinishSettle();


                await TaskManager.Instance.ReturnException("直接跳到结算");
                //Debug.Log("到这里了");
                //// 创建一个已经完成且包含异常的Task对象
                //Exception exception = new Exception("直接跳到结算");
                //await Task.FromException(exception);
            }
        }
        else
        {
            if (playedCard.Owner.IgnoreArmor)
            {
                await TaskManager.Instance.DontAwait();
                return;
            }
            //如果不是杀则不触发
            if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
                 && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash
                 && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash)
            {
                await TaskManager.Instance.DontAwait();
                return;
            }

            //杀无属性
            if (playedCard.CardAsset.SpellAttribute != SpellAttribute.None)
            {
                await TaskManager.Instance.DontAwait();
                return;
            }

            if (hasEquipment && equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Tengjia)
            {
                //直接进入结算
                await UseCardManager.Instance.FinishSettle();

                Exception exception = new Exception("直接跳到结算");
                await Task.FromException(exception);
            }
            else
            {
                Debug.Log("解除");
                targetPlayer.ShowOp2Button = false;
            }
        }

    }

    /// <summary>
    /// 木流牛马hook
    /// </summary>
    /// <param name="player"></param>
    public void CartHook(Player player)
    {
        (bool hasTreasure, OneCardManager treasureCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Treasure);
        if (hasTreasure)
        {
            if (treasureCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Cart)
            {
                Debug.Log("木流牛马生效");
                foreach (Player p in GlobalSettings.Instance.PlayerInstances)
                {
                    p.HasTreasure = false;
                }
                player.HasTreasure = true;
            }
        }
    }

    /// 失去木流牛马hook
    public async void CartDisHook(Player player, OneCardManager equipmentCard)
    {
        if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Cart)
        {
            Debug.Log("木流牛马失效");
            player.HasTreasure = false;
            while (TurnManager.Instance.whoseTurn.TreasureLogic.CardsInTreasure.Count > 0)
            {
                int cardId = TurnManager.Instance.whoseTurn.TreasureLogic.CardsInTreasure[0];
                await player.DisACardFromTreasure(cardId);
            }
        }
    }

    /// <summary>
    /// 雷神之锤
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task ActiveThunderHarmer(OneCardManager playedCard, Player targetPlayer)
    {
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
            || playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash
            || playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        Player player = playedCard.Owner;
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderHarmer)
            {
                Debug.Log("不解除");
                TaskManager.Instance.AddATask(TaskType.ThunderHarmerTask);

                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动雷神之锤");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    SelectOneForThunderHarmer(player);
                });

                List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
                if (canAttackTargets.Count == 0)
                {
                    player.PArea.Portrait.OpButton2.enabled = false;
                }

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.ThunderHarmerTask);
                });

                await TaskManager.Instance.TaskBlockDic[TaskType.ThunderHarmerTask][0].Task;
            }
            else
            {
                Debug.Log("解除");
                player.ShowOp2Button = false;
            }
        }
        else
        {
            Debug.Log("解除");
            player.ShowOp2Button = false;
        }
    }

    public void SelectOneForThunderHarmer(Player player)
    {
        List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
        foreach (int targetPlayerID in canAttackTargets)
        {
            if (targetPlayerID != player.ID)
            {
                Player tPlayer = GlobalSettings.Instance.FindPlayerByID(targetPlayerID);
                tPlayer.ShowOp1Button = true;
                tPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                tPlayer.PArea.Portrait.ChangeOp1ButtonText("选择溅射对象");
                tPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    SelectCardForThunderHarmer(player, tPlayer);
                });
            }
        }
    }

    public void SelectCardForThunderHarmer(Player player, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 1;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            List<int> relationCardIds = new List<int>();
            //
            OneCardManager thunderDamageCardManager = GlobalSettings.Instance.PDeck.DisguisedCardAssetWithType(player, SubTypeOfCards.ThunderSlash, relationCardIds, false);
            SettleManager.Instance.StartSettle(thunderDamageCardManager, thunderDamageCardManager.CardAsset.SpellAttribute, targetPlayer);
            TaskManager.Instance.UnBlockTask(TaskType.ThunderHarmerTask);
        };

        for (int i = player.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(player.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = player.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(player.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    /// <summary>
    /// 胜利之剑
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public async Task VictorySwordHook(Player player, OneCardManager playedCard)
    {
        if (GlobalSettings.Instance.Table.CardsOnTable.Count == 0)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[GlobalSettings.Instance.LastOneCardOnTable().UniqueCardID][0]);

        TaskManager.Instance.AddATask(TaskType.VictorySwordTask);

        Debug.Log("-------------------------------");
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        Debug.Log("-------------------------------" + hasEquipment);
        if (hasEquipment && (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash))
        {
            //TODO 双武将 男男 男女判断
            if (player.CharAsset.CharSex == targetPlayer.CharAsset.CharSex)
            {
                await TaskManager.Instance.DontAwait();
                return;
            }
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.VictorySword)
            {
                Debug.Log("不解除");
                HighlightManager.DisableAllCards();
                HighlightManager.DisableAllOpButtons();
                player.ShowOp1Button = true;
                player.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp1ButtonText("发动胜利之剑");
                player.PArea.Portrait.OpButton1.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    HandleVictorySword(player, targetPlayer);
                });

                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("不发动");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TaskManager.Instance.UnBlockTask(TaskType.VictorySwordTask);
                });

                try
                {
                    await TaskManager.Instance.TaskBlockDic[TaskType.VictorySwordTask][0].Task;
                }
                catch (Exception ex)
                {
                    Debug.Log("task exception :" + ex);
                }
            }
            else
            {
                Debug.Log("解除");
                await TaskManager.Instance.DontAwait();
            }
        }
        else
        {
            Debug.Log("解除");
            await TaskManager.Instance.DontAwait();
        }
    }

    /// 目标选择弃一张牌，或者让对方摸一张牌
    public async void HandleVictorySword(Player player, Player targetPlayer)
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();

        //翻卡牌
        //红色武器持有者摸一张牌
        //黑色被杀目标要弃一张
        OneCardManager flopedCard = await TurnManager.Instance.whoseTurn.FlopCard();
        Debug.Log("翻出卡牌的花色:" + flopedCard.CardAsset.Suits);
        if (flopedCard.CardAsset.Suits == CardSuits.Spades || flopedCard.CardAsset.Suits == CardSuits.Clubs)
        {
            HighlightManager.DisableAllOpButtons();
            //若没有牌则不能选择弃一张牌
            if (targetPlayer.Hand.CardsInHand.Count == 0)
            {
                TaskManager.Instance.UnBlockTask(TaskType.VictorySwordTask);
            }
            else
            {
                DisCardForVictorySword(targetPlayer);
            }
        }
        else
        {
            HighlightManager.DisableAllOpButtons();
            await player.DrawSomeCards(1);
            TaskManager.Instance.UnBlockTask(TaskType.VictorySwordTask);
        }
    }

    /// 弃一张牌
    public void DisCardForVictorySword(Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            TaskManager.Instance.UnBlockTask(TaskType.VictorySwordTask);
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

}
