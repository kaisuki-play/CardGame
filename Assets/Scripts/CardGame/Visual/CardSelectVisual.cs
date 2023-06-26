using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CardSelectVisual : MonoBehaviour
{
    public TargetCardsPanelType PanelType;

    public GameObject HandSlots;
    public GameObject JudgementSlots;
    public GameObject EquipmentSlots;

    public List<GameObject> CardsOnHand = new List<GameObject>();
    public List<GameObject> CardsOnJudgement = new List<GameObject>();
    public List<GameObject> CardsOnEquipment = new List<GameObject>();

    public System.Action AfterDisCardCompletion;
    public System.Action AfterSelectCardAsOtherCardCompletion;
    public int DisCardNumber = 1;
    public int AlreadyDisCardNumber = 0;

    public List<int> SelectCardIds = new List<int>();

    public void Dismiss()
    {
        DisAllCards();
        this.gameObject.SetActive(false);
    }

    // 顺手牵羊、过河拆桥、寒冰剑 加牌
    public void AddHandCardsAtIndex(OneCardManager cardManager)
    {
        GameObject slots;
        List<GameObject> cardlist;
        switch (cardManager.CardLocation)
        {
            case CardLocation.Hand:
                slots = HandSlots;
                cardlist = CardsOnHand;
                break;
            case CardLocation.Judgement:
                slots = JudgementSlots;
                cardlist = CardsOnJudgement;
                break;
            case CardLocation.Equipment:
                slots = EquipmentSlots;
                cardlist = CardsOnEquipment;
                break;
            default:
                slots = HandSlots;
                cardlist = CardsOnHand;
                break;
        }
        // create a new card from prefab
        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.CardToSelectPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        card.GetComponent<Button>().onClick.RemoveAllListeners();
        card.GetComponent<Button>().onClick.AddListener(async () =>
        {
            Debug.Log(card.GetComponent<OneCardManager>().UniqueCardID);
            await HandleCard(card);
        });

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CardAsset = cardManager.CardAsset;
        manager.ReadCardFromAsset();
        manager.UniqueCardID = cardManager.UniqueCardID;
        manager.Owner = cardManager.Owner;

        // parent a new creature gameObject to table slots
        card.transform.SetParent(slots.transform);

        // add a new creature to the list
        cardlist.Add(card);

        // add our unique ID to this creature
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = -1;
    }

    public async Task HandleCard(GameObject selectCard)
    {
        int cardId = selectCard.GetComponent<OneCardManager>().UniqueCardID;

        GameObject originCard = IDHolder.GetGameObjectWithID(cardId);
        OneCardManager originCardManager = originCard.GetComponent<OneCardManager>();
        switch (GlobalSettings.Instance.CardSelectVisual.PanelType)
        {
            //顺手牵羊
            case TargetCardsPanelType.Shunshouqianyang:
                {
                    (bool hasShunshouqianyang, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Shunshouqianyang);
                    if (hasShunshouqianyang)
                    {
                        await originCardManager.Owner.GiveCardToTarget(cardManager.Owner, originCardManager);
                        GlobalSettings.Instance.CardSelectVisual.Dismiss();
                        UseCardManager.Instance.FinishSettle();
                    }
                }
                break;
            //过河拆桥
            case TargetCardsPanelType.GuoheChaiqiao:
                {
                    (bool hasGuohechaiqiao, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Guohechaiqiao);
                    if (hasGuohechaiqiao)
                    {
                        await originCardManager.Owner.PArea.HandVisual.DisCardFromHand(originCardManager.UniqueCardID);
                        GlobalSettings.Instance.CardSelectVisual.Dismiss();
                        UseCardManager.Instance.FinishSettle();
                    }
                }
                break;
            //五谷丰登
            case TargetCardsPanelType.Wugufengdeng:
                {
                    (bool hasWugufengdeng, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Wugufengdeng);
                    if (hasWugufengdeng)
                    {
                        int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);

                        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.Targets[cardIndex][0]);
                        await targetPlayer.DrawACardFromDeck(originCardManager.UniqueCardID);

                        if (TargetsManager.Instance.Targets[cardIndex].Count == 1)
                        {
                            GlobalSettings.Instance.CardSelectVisual.Dismiss();
                            UseCardManager.Instance.FinishSettle();
                        }
                        else
                        {
                            Destroy(selectCard);
                            GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(false);
                            UseCardManager.Instance.FinishSettle();
                        }
                    }
                }
                break;
            //弃牌
            case TargetCardsPanelType.DisHandCard:
                {
                    await originCardManager.Owner.PArea.HandVisual.DisCardFromHand(originCardManager.UniqueCardID);
                    Destroy(selectCard);
                    AlreadyDisCardNumber++;
                    if (AlreadyDisCardNumber == DisCardNumber)
                    {
                        Debug.Log("弃完牌了");
                        GlobalSettings.Instance.CardSelectVisual.Dismiss();
                        this.AfterDisCardCompletion.Invoke();
                    }
                }
                break;
            case TargetCardsPanelType.UseSomeCardAsSlash:
                {
                    this.SelectCardIds.Add(originCardManager.UniqueCardID);
                    Destroy(selectCard);
                    AlreadyDisCardNumber++;
                    if (AlreadyDisCardNumber == DisCardNumber)
                    {
                        this.AfterSelectCardAsOtherCardCompletion.Invoke();
                        GlobalSettings.Instance.CardSelectVisual.Dismiss();
                    }
                }
                break;
        }
    }

    public void DisAllCards()
    {
        foreach (GameObject card in CardsOnHand)
        {
            Destroy(card);
        }
        foreach (GameObject card in CardsOnJudgement)
        {
            Destroy(card);
        }
        foreach (GameObject card in CardsOnEquipment)
        {
            Destroy(card);
        }
        CardsOnHand.Clear();
        CardsOnJudgement.Clear();
        CardsOnEquipment.Clear();
        this.AlreadyDisCardNumber = 0;
        this.DisCardNumber = 1;
    }
}
