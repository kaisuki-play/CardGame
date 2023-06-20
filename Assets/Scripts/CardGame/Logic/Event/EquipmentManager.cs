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
    public void AddOrReplaceEquipment(Player player, OneCardManager cardManager)
    {
        (bool hasEquipment, OneCardManager oldEquipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, cardManager.CardAsset.TypeOfEquipment);
        //有同类型的装备，就需要替换
        if (hasEquipment)
        {
            //去旧卡
            player.PArea.EquipmentVisaul.DisCardFromEquipment(oldEquipmentCard.UniqueCardID);
            player.EquipmentLogic.RemoveCard(oldEquipmentCard.UniqueCardID);

            ZhugeliannuDisHook(player, oldEquipmentCard);
            //加新卡
            player.PArea.EquipmentVisaul.EquipWithCard(cardManager.UniqueCardID, player);
            player.EquipmentLogic.AddCard(cardManager.UniqueCardID);
        }
        else
        {
            //加新卡
            player.PArea.EquipmentVisaul.EquipWithCard(cardManager.UniqueCardID, player);
            player.EquipmentLogic.AddCard(cardManager.UniqueCardID);
        }
        ZhugeliannuHook(player);
    }

    /// <summary>
    /// 诸葛连弩hook
    /// </summary>
    /// <param name="player"></param>
    public void ZhugeliannuHook(Player player)
    {
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Zhugeliannu)
            {
                Debug.Log("诸葛连弩生效");
                CounterManager.Instance.SlashLimit += 10;
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
            CounterManager.Instance.SlashLimit -= 10;
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
    public async Task CixiongHook(Player player, OneCardManager playedCard, Player targetPlayer)
    {
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
                    await TaskManager.Instance.TaskBlockDic[TaskType.CixiongShuangguTask].Task;
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
        targetPlayer.PArea.Portrait.OpButton2.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            player.DrawSomeCards(1);
            TaskManager.Instance.UnBlockTask(TaskType.CixiongShuangguTask);
        });
    }

    /// 弃一张牌
    public void DisCardForCixiong(Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = TargetCardsPanelType.DisHandCard;
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
    public async Task GuanshifuHook(Player player, Player targetPlayer)
    {
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

                await TaskManager.Instance.TaskBlockDic[TaskType.GuanshifuTask].Task;
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
        GlobalSettings.Instance.CardSelectVisual.PanelType = TargetCardsPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 2;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            SettleManager.Instance.StartSettle();
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
                Debug.Log("不解除");
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
        GlobalSettings.Instance.CardSelectVisual.PanelType = TargetCardsPanelType.UseSomeCardAsSlash;
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
    public async Task QinglongyanyueHook(Player player, Player targetPlayer)
    {
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Qinglongyanyuedao)
            {
                Debug.Log("不解除");
                HighlightManager.DisableAllOpButtons();
                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动追杀");
                player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    TargetsManager.Instance.DefaultTarget.Add(targetPlayer.ID);
                    UseCardManager.Instance.BackToWhoseTurn();
                    UseCardManager.Instance.NeedToPlaySlash(player);
                });

                player.ShowOp3Button = true;
                player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp3Button2Text("不发动追杀");
                player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                {
                    HighlightManager.DisableAllOpButtons();
                    UseCardManager.Instance.BackToWhoseTurn();
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
        await TaskManager.Instance.DontAwait();
    }
}
