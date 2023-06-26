using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GoldenFingerVisual : MonoBehaviour
{
    public GameObject CardsSlots;

    public List<GameObject> CardsOnGoldenFinger = new List<GameObject>();

    public Player curDrawCardPlayer;

    public System.Action Completion;

    private int _playerCount = 0;
    private int _playerDrawCardsCount = 0;
    private int _playerDrawCardsLimit = 4;

    public void Dismiss()
    {
        DisAllCards();
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        GlobalSettings.Instance.GoldenFingerVisual.gameObject.SetActive(true);

        for (int i = 15; i >= 0; i--)
        {
            GameObject card = GlobalSettings.Instance.PDeck.DeckCards[i];
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.GoldenFingerVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    // 顺手牵羊、过河拆桥、寒冰剑 加牌
    public void AddHandCardsAtIndex(OneCardManager cardManager)
    {
        GameObject slots = CardsSlots;
        List<GameObject> cardlist = CardsOnGoldenFinger;

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

    private async Task HandleCard(GameObject selectCard)
    {
        int cardId = selectCard.GetComponent<OneCardManager>().UniqueCardID;

        Destroy(selectCard);
        CardsOnGoldenFinger.Remove(selectCard);

        GameObject originCard = IDHolder.GetGameObjectWithID(cardId);
        OneCardManager originCardManager = originCard.GetComponent<OneCardManager>();

        Player targetPlayer = this.curDrawCardPlayer;
        await targetPlayer.DrawACardFromDeck(originCardManager.UniqueCardID);

        _playerDrawCardsCount++;

        Dismiss();
        Show();

        if (_playerDrawCardsCount == _playerDrawCardsLimit)
        {
            _playerDrawCardsCount = 0;
            curDrawCardPlayer = curDrawCardPlayer.OtherPlayer;
            _playerCount++;
        }
        if (_playerCount == GlobalSettings.Instance.PlayerInstances.Length)
        {
            Debug.Log("发牌完毕");
            Dismiss();
            this.Completion.Invoke();
        }
    }

    public void DisAllCards()
    {
        foreach (GameObject card in CardsOnGoldenFinger)
        {
            Destroy(card);
        }
        CardsOnGoldenFinger.Clear();
    }
}
