using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SkillManager : MonoBehaviour
{
    /// <summary>
    /// 出牌阶段开始
    /// </summary>
    /// <returns></returns>
    public static async Task StartPlayPhase()
    {
        EquipmentManager.Instance.ZhugeliannuHook(TurnManager.Instance.whoseTurn);
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 装备移动
    /// </summary>
    /// <param name="newOwner"></param>
    /// <param name="oldOwner"></param>
    /// <param name="newLocation"></param>
    /// <param name="oldLocation"></param>
    /// <param name="cardManager"></param>
    public static void HandleEquipmentMove(Player newOwner, Player oldOwner, CardLocation newLocation, CardLocation oldLocation, OneCardManager cardManager)
    {
        if (TurnManager.Instance.whoseTurn == null)
        {
            return;
        }
        switch (cardManager.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Zhugeliannu:
                {
                    if (newOwner == TurnManager.Instance.whoseTurn && newLocation == CardLocation.Equipment)
                    {
                        EquipmentManager.Instance.ZhugeliannuHook(TurnManager.Instance.whoseTurn);
                    }
                    else
                    {
                        if (oldLocation != CardLocation.DrawDeck)
                        {
                            EquipmentManager.Instance.ZhugeliannuDisHook(TurnManager.Instance.whoseTurn, cardManager);
                        }
                    }
                }
                break;
            case SubTypeOfCards.Zhangbashemao:
                {
                    EquipmentManager.Instance.ZhangbashemaoHook(TurnManager.Instance.whoseTurn, true);
                }
                break;
        }
    }

    /// <summary>
    /// 1-7 第四步
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task StartFixedTargets(OneCardManager playedCard)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.CixiongDoubleSwards:
                    await EquipmentManager.Instance.CixiongHook(playedCard.Owner, playedCard, GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]));
                    break;
                case SubTypeOfCards.Qinghongjian:
                    await EquipmentManager.Instance.QinggangjianHook(playedCard.Owner);
                    break;
            }
        }
        else
        {
            await TaskManager.Instance.DontAwait();
        }

    }

    /// <summary>
    /// 出了闪
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static async Task AfterPlayAJink(Player player, Player targetPlayer)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Guanshifu:
                    await EquipmentManager.Instance.GuanshifuHook(player, targetPlayer);
                    break;
                case SubTypeOfCards.Qinglongyanyuedao:
                    await EquipmentManager.Instance.QinglongyanyueHook(player, targetPlayer);
                    break;
                default:
                    UseCardManager.Instance.FinishSettle();
                    await TaskManager.Instance.DontAwait();
                    break;
            }
        }
        else
        {
            await TaskManager.Instance.DontAwait();
        }

    }

    /// <summary>
    /// 需要出杀，或者闲置状态杀没有到上限
    /// </summary>
    /// <param name="player"></param>
    /// <param name="needTarget"></param>
    /// <returns></returns>
    public static async Task NeedToPlaySlash(Player player, bool needTarget)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(player, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Zhangbashemao:
                    EquipmentManager.Instance.ZhangbashemaoHook(player, needTarget);
                    break;
            }
        }
        else
        {
            await TaskManager.Instance.DontAwait();
        }
    }

    /// <summary>
    /// 1-7 第二步
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task StartHandleTargets(OneCardManager playedCard)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Fangtianhuaji:
                    await EquipmentManager.Instance.ActiveFangtianhuaji(playedCard);
                    break;
                default:
                    await TaskManager.Instance.DontAwait();
                    break;
            }
        }
        else
        {
            await TaskManager.Instance.DontAwait();
        }
    }
}
