using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Security.Cryptography;

public enum CardSelectPanelType
{
    GuoheChaiqiao,
    Wugufengdeng,
    Shunshouqianyang,
    DisHandCard,
    UseSomeCardAsSlash,
    DisSomeCardForDestNumber,
    Judgement,
    ShowTargetACard
}

public class CardSelectVisual : MonoBehaviour
{
    public CardSelectPanelType PanelType;

    public GameObject HandSlots;
    public GameObject JudgementSlots;
    public GameObject EquipmentSlots;

    public List<GameObject> CardsOnHand = new List<GameObject>();
    public List<GameObject> CardsOnJudgement = new List<GameObject>();
    public List<GameObject> CardsOnEquipment = new List<GameObject>();

    public System.Action AfterDisCardCompletion;
    public System.Action AfterSelectCardAsOtherCardCompletion;
    public System.Action<GameObject> AfterSelectCardForJudgementCompletion;

    public int DisCardNumber = 1;
    public int AlreadyDisCardNumber = 0;
    public List<int> RankSumList = new List<int>();
    public int RankSum = 0;

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
            case CardSelectPanelType.Shunshouqianyang:
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
            case CardSelectPanelType.GuoheChaiqiao:
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
            case CardSelectPanelType.Wugufengdeng:
                {
                    (bool hasWugufengdeng, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Wugufengdeng);
                    if (hasWugufengdeng)
                    {
                        //int cardIndex = GlobalSettings.Instance.Table.CardIndexOnTable(cardManager.UniqueCardID);

                        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0]);
                        await targetPlayer.DrawACardFromDeck(originCardManager.UniqueCardID);

                        if (TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID].Count == 1)
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
            case CardSelectPanelType.DisHandCard:
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
            case CardSelectPanelType.UseSomeCardAsSlash:
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
            case CardSelectPanelType.DisSomeCardForDestNumber:
                if (selectCard.GetComponent<OneCardManager>().CanBePlayedNow == false)
                {
                    this.SelectCardIds.Add(originCardManager.UniqueCardID);
                    AlreadyDisCardNumber++;
                }
                else
                {
                    this.SelectCardIds.Remove(originCardManager.UniqueCardID);
                    AlreadyDisCardNumber--;
                }
                selectCard.GetComponent<OneCardManager>().CanBePlayedNow = !selectCard.GetComponent<OneCardManager>().CanBePlayedNow;

                int rankSum = 0;
                foreach (int cId in this.SelectCardIds)
                {
                    GameObject card = IDHolder.GetGameObjectWithID(cId);
                    OneCardManager cardManager = card.GetComponent<OneCardManager>();
                    rankSum += (int)cardManager.CardAsset.CardRank;
                }

                if (RankSumList.Contains(rankSum))
                {
                    this.RankSum = rankSum;
                    await DisSelectCards();
                    this.AfterSelectCardAsOtherCardCompletion.Invoke();
                }

                break;
            case CardSelectPanelType.Judgement:
                {
                    Dismiss();
                    this.AfterSelectCardForJudgementCompletion(originCard);
                }
                break;
            case CardSelectPanelType.ShowTargetACard:
                {
                    Dismiss();
                    this.AfterSelectCardForJudgementCompletion(originCard);
                }
                break;
        }
    }

    public async Task DisSelectCards()
    {
        while (this.SelectCardIds.Count > 0)
        {
            GameObject card = IDHolder.GetGameObjectWithID(this.SelectCardIds[0]);
            this.SelectCardIds.RemoveAt(0);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            await cardManager.Owner.PArea.HandVisual.DisCardFromHand(cardManager.UniqueCardID);
        }
        this.Dismiss();
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
