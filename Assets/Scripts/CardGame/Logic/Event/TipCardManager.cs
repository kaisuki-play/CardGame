using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCardManager : MonoBehaviour
{
    public static TipCardManager Instance;
    public Player PlayCardOwner;
    private void Awake()
    {
        Instance = this;
    }

    public void SkipTipCard()
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if (cardManager != null)
        {
            TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].RemoveAt(0);
            //cardManager.TargetsPlayerIDs.RemoveAt(0);
            Debug.Log("现在还有几张牌的目标~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + TargetsManager.Instance.Targets.Count);
            if (TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1].Count != 0)
            {
                PlayCardManager.Instance.ActiveEffect(cardManager);
            }
            else
            {
                PlayCardManager.Instance.BackToWhoseTurn();
            }
        }
    }

    /// <summary>
    /// 无懈可击询问完毕，触发锦囊效果阶段
    /// </summary>
    public void ActiveTipCard()
    {
        OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
        if (cardManager != null)
        {
            switch (cardManager.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Nanmanruqin:
                    PlayCardManager.Instance.NeedToPlaySlash();
                    break;
                case SubTypeOfCards.Wanjianqifa:
                    PlayCardManager.Instance.NeedToPlayJink();
                    break;
                case SubTypeOfCards.Juedou:
                    if (this.PlayCardOwner != null)
                    {
                        Debug.Log("进来决斗了");
                        if (this.PlayCardOwner.ID == TargetsManager.Instance.Targets[TargetsManager.Instance.Targets.Count - 1][0])
                        {
                            Debug.Log("出牌是目标，高亮决斗出牌人");
                            PlayCardManager.Instance.NeedToPlaySlash(cardManager.Owner);
                        }
                        else
                        {
                            Debug.Log("出牌是决斗出牌人，高亮目标");
                            PlayCardManager.Instance.NeedToPlaySlash();
                        }
                    }
                    else
                    {
                        Debug.Log("出牌是决斗出牌人，高亮目标");
                        PlayCardManager.Instance.NeedToPlaySlash();
                    }
                    break;
                case SubTypeOfCards.Jiedaosharen:
                    Debug.Log("借刀杀人出杀设定");
                    PlayCardManager.Instance.NeedToPlaySlashInJiedaosharen();
                    break;
                case SubTypeOfCards.Wugufengdeng:
                case SubTypeOfCards.Shunshouqianyang:
                case SubTypeOfCards.Guohechaiqiao:
                default:
                    Debug.Log("锦囊");
                    ShowTargetAllCards(GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[0][0]));
                    //SkipTipCard();
                    break;
            }
        }
    }

    /// <summary>
    /// 借刀杀人清理目标
    /// </summary>
    public void JiedaosharenTargetsClear()
    {
        if (TargetsManager.Instance.SpecialTarget.Count > 0)
        {
            TargetsManager.Instance.SpecialTarget.RemoveAt(0);
        }

        DebugManager.Instance.Print2LevelList(TargetsManager.Instance.Targets);

        (bool hasJiedaosharen, OneCardManager jiedaoCardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
        if (hasJiedaosharen && TargetsManager.Instance.SpecialTarget.Count == 0)
        {
            GlobalSettings.Instance.Table.ClearCards(jiedaoCardManager.UniqueCardID);
        }
    }

    /// <summary>
    /// 借刀杀人多个目标的时候
    /// </summary>
    public void JiedaoSharenNextTarget()
    {
        OneCardManager jiedaosharenCard = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen).Item2;
        if (GlobalSettings.Instance.Table.CardsOnTable.Count > 0)
        {
            TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)].RemoveAt(0);
            if (TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)].Count > 0)
            {
                PlayCardManager.Instance.ActiveEffect(jiedaosharenCard);
            }
            else
            {
                PlayCardManager.Instance.BackToWhoseTurn();
            }
        }
        else
        {
            PlayCardManager.Instance.BackToWhoseTurn();
        }

    }

    public void GiveJiedaoSharenWeapon()
    {
        (bool hasJiedaosharen, OneCardManager jiedaosharenCard) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen);
        int targetId = TargetsManager.Instance.Targets[GlobalSettings.Instance.Table.CardIndexOnTable(jiedaosharenCard.UniqueCardID)][0];
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetId);
        targetPlayer.GiveWeaponToTargetWithCardType(jiedaosharenCard.Owner);
    }

    public void ShowTargetAllCards(Player targetPlayer)
    {
        GlobalSettings.Instance.TargetCardsPanel.gameObject.SetActive(true);
        for (int i = 0; i < targetPlayer.Hand.CardsInHand.Count; i++)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.TargetCardsPanel.AddHandCardsAtIndex(cardManager, i);
        }
        for (int i = 0; i < targetPlayer.JudgementLogic.CardsInJudgement.Count; i++)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.JudgementLogic.CardsInJudgement[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.TargetCardsPanel.AddHandCardsAtIndex(cardManager, i);
        }
        for (int i = 0; i < targetPlayer.EquipmentLogic.CardsInEquipment.Count; i++)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.TargetCardsPanel.AddHandCardsAtIndex(cardManager, i);
        }
    }
}
