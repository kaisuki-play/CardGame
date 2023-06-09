using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettleManager : MonoBehaviour
{
    public static SettleManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    //结算前的阶段
    public void StartSettle()
    {
        BeforeDamage();
    }

    public void BeforeDamage()
    {
        if (GlobalSettings.Instance.Table.CardsOnTable.Count > 0)
        {
            GameObject card = GlobalSettings.Instance.Table.CardsOnTable[0];
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            switch (cardManager.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Slash:
                case SubTypeOfCards.FireSlash:
                case SubTypeOfCards.ThunderSlash:
                    CalculateDamage();
                    break;
                default:
                    break;
            }
        }
    }

    public void CalculateDamage()
    {
        GameObject card = GlobalSettings.Instance.Table.CardsOnTable[0];
        OneCardManager cardManager = card.GetComponent<OneCardManager>();

        int originDamage = cardManager.CardAsset.SpecialSpellAmount;

        Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0]);
        curTargetPlayer.DamageEffect(originDamage);

        AfterDamage();
    }

    public void AfterDamage()
    {
        HandleIronChain();
    }

    public void HandleIronChain()
    {
        FinishSettle();
    }

    public void FinishSettle()
    {
        TargetsManager.Instance.Targets.RemoveAt(0);
        if (TargetsManager.Instance.Targets.Count == 0)
        {
            GlobalSettings.Instance.Table.ClearCards();
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
        }
        else
        {
            GameObject card = GlobalSettings.Instance.Table.CardsOnTable[0];
            OneCardManager cardManager = card.GetComponent<OneCardManager>();

            Player nextTargetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0]);
            nextTargetPlayer.ActiveEffect(cardManager);
        }
    }
}
