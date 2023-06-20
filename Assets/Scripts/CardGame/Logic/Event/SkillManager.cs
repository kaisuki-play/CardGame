using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SkillManager : MonoBehaviour
{
    public static async Task StartPlayPhase()
    {
        //EquipmentManager.Instance.ZhugeliannuHook(TurnManager.Instance.whoseTurn);
        await TaskManager.Instance.DontAwait();
    }

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
                        EquipmentManager.Instance.ZhugeliannuDisHook(TurnManager.Instance.whoseTurn, cardManager);
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

    public static async Task UseCard4(OneCardManager playedCard)
    {
        await EquipmentManager.Instance.QinggangjianHook(playedCard.Owner);
        await EquipmentManager.Instance.CixiongHook(playedCard.Owner, playedCard, GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]));

    }

    public static async Task UsedCard4(OneCardManager playedCard)
    {
        await EquipmentManager.Instance.CixiongHook(playedCard.Owner, playedCard, GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0]));
    }

    public static async Task AfterPlayAJink(Player player, Player targetPlayer)
    {
        await EquipmentManager.Instance.GuanshifuHook(player, targetPlayer);
        await EquipmentManager.Instance.QinglongyanyueHook(player, targetPlayer);
    }

    public static void NeedToPlaySlash(Player player, bool needTarget)
    {
        EquipmentManager.Instance.ZhangbashemaoHook(player, needTarget);
    }
}
