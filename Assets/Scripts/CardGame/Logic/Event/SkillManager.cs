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
        Debug.Log("new: " + newLocation + " old: " + oldLocation);
        if (TurnManager.Instance.whoseTurn == null)
        {
            return;
        }
        switch (cardManager.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Zhugeliannu:
                {
                    //情况1，牌从不是装备区的牌到装备区
                    //情况2，互换装备，卡牌的Owner不再是原来的主人 TODO之后进pending就不需要了
                    if ((oldLocation != CardLocation.Equipment && newLocation == CardLocation.Equipment) || (oldOwner != null && oldOwner != newOwner && (oldLocation == CardLocation.Equipment && newLocation == CardLocation.Equipment)))
                    {
                        EquipmentManager.Instance.ZhugeliannuHook(newOwner);
                    }
                    else
                    {
                        //情况1，失去装备区的装备,卡牌的位置不再是装备区
                        //情况2，互换装备，卡牌的Owner不再是原来的主人 TODO之后进pending就不需要了
                        if ((oldLocation == CardLocation.Equipment && newLocation != CardLocation.Equipment) || (oldOwner != null && oldOwner != newOwner && (oldLocation == CardLocation.Equipment && newLocation == CardLocation.Equipment)))
                        {
                            EquipmentManager.Instance.ZhugeliannuDisHook(oldOwner, cardManager);
                        }
                    }
                }
                break;
            case SubTypeOfCards.Zhangbashemao:
                {
                    EquipmentManager.Instance.ZhangbashemaoHook(newOwner, true);
                }
                break;
            case SubTypeOfCards.Qinghongjian:
                if ((oldLocation == CardLocation.Equipment && newLocation != CardLocation.Equipment) || (oldOwner != null && oldOwner != newOwner && (oldLocation == CardLocation.Equipment && newLocation == CardLocation.Equipment)))
                {
                    oldOwner.IgnoreArmor = false;
                }
                break;
            case SubTypeOfCards.Cart:
                {
                    if ((oldLocation != CardLocation.Equipment && newLocation == CardLocation.Equipment) || (oldOwner != null && oldOwner != newOwner && (oldLocation == CardLocation.Equipment && newLocation == CardLocation.Equipment)))
                    {
                        if (oldOwner != null)
                        {
                            oldOwner.HasTreasure = false;
                        }
                        EquipmentManager.Instance.CartHook(newOwner);
                    }
                    else if ((oldLocation == CardLocation.Equipment && newLocation != CardLocation.Equipment) || (oldOwner != null && oldOwner != newOwner && (oldLocation == CardLocation.Equipment && newLocation == CardLocation.Equipment)))
                    {
                        EquipmentManager.Instance.CartDisHook(oldOwner, cardManager);
                    }
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
    /// 需要打出闪之前 1-7的6
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task BeforeNeedPlayAJink(OneCardManager playedCard = null, Player targetPlayer = null)
    {
        if (playedCard == null)
        {
            playedCard = GlobalSettings.Instance.LastOneCardOnTable();
        }
        if (targetPlayer == null)
        {
            targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]);
        }
        (bool hasArmor, OneCardManager armorCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
        if (hasArmor)
        {
            switch (armorCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Renwangdun:
                    await EquipmentManager.Instance.ActiveRenwangdun(playedCard, targetPlayer);
                    break;
                case SubTypeOfCards.Tengjia:
                    await EquipmentManager.Instance.ActiveTengjia(playedCard, targetPlayer);
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


    /// <summary>
    /// 需要出闪
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static async Task NeedPlayAJink(Player targetPlayer, OneCardManager playedCard = null)
    {
        if (playedCard == null)
        {
            playedCard = GlobalSettings.Instance.LastOneCardOnTable();
        }
        (bool hasArmor, OneCardManager armorCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
        if (hasArmor)
        {
            switch (armorCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Baguazhen:
                    await EquipmentManager.Instance.ActiveBaguazhen(playedCard, targetPlayer);
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

    /// <summary>
    /// 出了闪
    /// </summary>
    /// <param name="player"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static async Task AfterPlayAJink(OneCardManager playedCard, Player targetPlayer)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Guanshifu:
                    await EquipmentManager.Instance.GuanshifuHook(playedCard, targetPlayer);
                    break;
                case SubTypeOfCards.Qinglongyanyuedao:
                    await EquipmentManager.Instance.QinglongyanyueHook(playedCard, targetPlayer);
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
    /// 开始出一张牌 1-7的1
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task StartPlayACard(OneCardManager playedCard)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Zhuqueyushan:
                    await EquipmentManager.Instance.ActiveZhuqueyushan(playedCard);
                    break;
                case SubTypeOfCards.SilverMoon:
                    await EquipmentManager.Instance.ActiveSilverMoon(playedCard);
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

    /// <summary>
    /// 开始计算伤害
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <param name="originalDamage"></param>
    /// <returns></returns>
    public static int StartCalculateDamageForSource(OneCardManager playedCard, Player targetPlayer, int originalDamage, SpellAttribute spellAttribute = SpellAttribute.None, bool isFromIronChain = false)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                //伤害来源装备古锭刀
                case SubTypeOfCards.Gudiandao:
                    {
                        //1.伤害类型为杀
                        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
                            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash
                            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash)
                        {
                            return originalDamage;
                        }
                        //2.非铁索传导
                        if (isFromIronChain)
                        {
                            return originalDamage;
                        }
                        //3.受害者手牌为0
                        if (targetPlayer.Hand.CardsInHand.Count == 0)
                        {
                            return originalDamage + 1;
                        }
                        else
                        {
                            return originalDamage;
                        }
                    }
                default:
                    return originalDamage;
            }
        }
        else
        {
            return originalDamage;
        }
    }

    public static int StartCalculateDamageForTarget(OneCardManager playedCard, Player targetPlayer, int originalDamage, SpellAttribute spellAttribute = SpellAttribute.None, bool isFromIronChain = false)
    {
        (bool hasArmor, OneCardManager armorCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
        if (hasArmor)
        {
            switch (armorCard.CardAsset.SubTypeOfCard)
            {
                //伤害目标装备藤甲
                case SubTypeOfCards.Tengjia:
                    {
                        //忽视防具
                        if (playedCard.Owner.IgnoreArmor)
                        {
                            return originalDamage;
                        }
                        //确保是火属性
                        if (spellAttribute != SpellAttribute.FireSlash)
                        {
                            return originalDamage;
                        }
                        //藤甲伤害+1、其余暂时不加
                        (bool hasEquipment, OneCardManager equipmentCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
                        if (hasEquipment)
                        {
                            if (equipmentCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Tengjia)
                            {
                                return originalDamage + 1;
                            }
                            else
                            {
                                return originalDamage;
                            }
                        }
                        else
                        {
                            return originalDamage;
                        }
                    }
                case SubTypeOfCards.SilverLion:
                    if (playedCard.Owner.IgnoreArmor)
                    {
                        return originalDamage;
                    }
                    return 1;
                default:
                    return originalDamage;
            }
        }
        else
        {
            return originalDamage;
        }
    }

    /// <summary>
    /// 1-7的6 南蛮入侵、万箭齐发AOE 需要出杀、需要出闪前
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static async Task BeforeAOEForTarget(OneCardManager playedCard, Player targetPlayer)
    {
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~询问是否有防具");
        (bool hasArmor, OneCardManager armorCard) = EquipmentManager.Instance.HasEquipmentWithType(targetPlayer, TypeOfEquipment.Armor);
        if (hasArmor)
        {
            switch (armorCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Tengjia:
                    Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~询问是否有防具:藤甲");
                    await EquipmentManager.Instance.ActiveTengjia(playedCard, targetPlayer);
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

    /// <summary>
    /// 计算伤害的时候
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static async Task BeforeCalculateDamage(OneCardManager playedCard, Player targetPlayer)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.FrostBlade:
                    {
                        //TODO 铁索传导
                        await EquipmentManager.Instance.ActiveFrostBlade(playedCard, targetPlayer);
                    }
                    break;
            }
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 伤害后
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <param name="isFromIronChain"></param>
    /// <returns></returns>
    public static async Task StartAfterDamage(OneCardManager playedCard, Player targetPlayer, bool isFromIronChain = false)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Qilingong:
                    {
                        if (isFromIronChain)
                        {
                            await TaskManager.Instance.DontAwait();
                        }
                        else
                        {
                            await EquipmentManager.Instance.ActiveQilingong(playedCard, targetPlayer);
                        }
                    }
                    break;
            }
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 使用了一张牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static async Task UseACard(OneCardManager playedCard)
    {
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(playedCard.Owner, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            switch (weaponCard.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.SilverMoon:
                    await EquipmentManager.Instance.ActiveSilverMoon(playedCard);
                    break;
            }
        }
        await TaskManager.Instance.DontAwait();

    }

}
