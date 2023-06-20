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
    public bool CixiongHook(Player player, OneCardManager playedCard, Player targetPlayer)
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
                Debug.Log("想通性别");
                return false;
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

                return true;
            }
            else
            {
                Debug.Log("解除");
                return false;
            }
        }
        else
        {
            Debug.Log("解除");
            return false;
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
    public bool GuanshifuHook(Player player, Player targetPlayer)
    {
        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasEquipment)
        {
            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Guanshifu)
            {
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

                return true;
            }
            else
            {
                Debug.Log("解除");
                return false;
            }
        }
        else
        {
            Debug.Log("解除");
            return false;
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
}
