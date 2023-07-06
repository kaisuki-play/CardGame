using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DG.Tweening;

public enum CardSelectPanelType
{
    GuoheChaiqiao,
    Wugufengdeng,
    Shunshouqianyang,
    DisHandCard,
    DisCard,
    UseSomeCardAsSlash,
    DisSomeCardForDestNumber,
    Judgement,
    ShowTargetACard,
    GiveCardToOther,
    ShowAllCardForSameColor,
    SelectSomeCardsToAsACard,
    SelectATypeOfCardPutOnDeckTop,
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

    public void Show()
    {
        SelectCardIds.Clear();
        this.gameObject.SetActive(true);
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
            Debug.Log(card.GetComponent<OneCardManager>().ShowCardID);
            await HandleCard(card);
        });

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.SetCardAssetA(cardManager.CardAsset);
        manager.ReadCardFromAssetA();
        manager.UniqueCardID = IDFactory.GetUniqueID();
        manager.ShowCardID = cardManager.UniqueCardID;
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
        int cardId = selectCard.GetComponent<OneCardManager>().ShowCardID;

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
                        await UseCardManager.Instance.FinishSettle();
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
                        await UseCardManager.Instance.FinishSettle();
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
                            await UseCardManager.Instance.FinishSettle();
                        }
                        else
                        {
                            Destroy(selectCard);
                            GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(false);
                            await UseCardManager.Instance.FinishSettle();
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
            case CardSelectPanelType.DisCard:
                {
                    switch (originCardManager.CardLocation)
                    {
                        case CardLocation.Hand:
                            await originCardManager.Owner.PArea.HandVisual.DisCardFromHand(originCardManager.UniqueCardID);
                            break;
                        case CardLocation.Judgement:
                            await originCardManager.Owner.PArea.JudgementVisual.DisCardFromJudgement(originCardManager.UniqueCardID);
                            break;
                        case CardLocation.Equipment:
                            await originCardManager.Owner.PArea.EquipmentVisaul.DisCardFromEquipment(originCardManager.UniqueCardID);
                            break;
                    }
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
            case CardSelectPanelType.GiveCardToOther:
                {
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

                    if (AlreadyDisCardNumber == 2)
                    {
                        this.AfterSelectCardAsOtherCardCompletion.Invoke();
                    }
                }
                break;
            case CardSelectPanelType.SelectSomeCardsToAsACard:
                if (selectCard.GetComponent<OneCardManager>().CanBePlayedNow == false)
                {
                    this.SelectCardIds.Add(cardId);
                    AlreadyDisCardNumber++;
                }
                else
                {
                    this.SelectCardIds.Remove(cardId);
                    AlreadyDisCardNumber--;
                }
                selectCard.GetComponent<OneCardManager>().CanBePlayedNow = !selectCard.GetComponent<OneCardManager>().CanBePlayedNow;
                if (AlreadyDisCardNumber == DisCardNumber)
                {
                    this.AfterDisCardCompletion.Invoke();
                }
                break;
            case CardSelectPanelType.SelectATypeOfCardPutOnDeckTop:
                {
                    Dismiss();

                    var tcs = new TaskCompletionSource<bool>();

                    originCardManager.Owner.Hand.DisCard(originCardManager.UniqueCardID);
                    originCardManager.Owner.PArea.HandVisual.RemoveCard(originCard);

                    originCard.transform.SetParent(null);

                    Sequence s = DOTween.Sequence();
                    s.Append(originCard.transform.DOMove(GlobalSettings.Instance.PDeck.ChildCanvas.transform.position, 1f));
                    s.OnComplete(() =>
                    {
                        tcs.SetResult(true);
                    });
                    await tcs.Task;

                    originCard.transform.SetParent(GlobalSettings.Instance.PDeck.ChildCanvas.transform);

                    GlobalSettings.Instance.PDeck.DeckCards.Insert(0, originCard);

                    originCardManager.CanBePlayedNow = false;

                    await originCardManager.ChangeOwnerAndLocation(null, CardLocation.DrawDeck);

                    this.AfterDisCardCompletion.Invoke();
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
