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

    /// <summary>
    /// 失去诸葛连弩hook
    /// </summary>
    /// <param name="player"></param>
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
            Debug.Log("-------------------------------");
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
            targetPlayer.DisACardFromHand(targetPlayer.Hand.CardsInHand[0]);
            TaskManager.Instance.UnBlockTask(TaskType.CixiongShuangguTask);
        });

        targetPlayer.ShowOp2Button = true;
        targetPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.ChangeOp2ButtonText("对方摸一张牌");
        targetPlayer.PArea.Portrait.OpButton2.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            player.DrawACard(1);
            TaskManager.Instance.UnBlockTask(TaskType.CixiongShuangguTask);
        });
    }
}
