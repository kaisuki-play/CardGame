using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

// this class should be attached to the deck
// generates new cards and places them into the hand
public class PlayerDeckVisual : MonoBehaviour
{
    public Canvas ChildCanvas;
    public List<GameObject> DeckCards = new List<GameObject>();
    void Awake()
    {
        AddDeckCards();
    }

    public void AddDeckCards()
    {
        Debug.Log("卡牌总数: " + DeckSource.Instance.Cards.Count);
        foreach (CardAsset cardAsset1 in DeckSource.Instance.Cards)
        {
            HandleCards(cardAsset1);
        }
        DeckCards.Shuffle();
    }

    void HandleCards(CardAsset cardAsset)
    {
        switch (cardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Slash:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_10);

                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_J);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_5);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_J);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_K);

                break;
            case SubTypeOfCards.Jink:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_K);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_J);

                break;

            case SubTypeOfCards.Peach:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3);
                break;

            case SubTypeOfCards.ThunderSlash:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_8);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_5);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_8);
                break;

            case SubTypeOfCards.FireSlash:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_10);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5);
                break;

            case SubTypeOfCards.Analeptic:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_9);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_9);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_9);
                break;

            case SubTypeOfCards.Juedou:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A);
                break;

            case SubTypeOfCards.Wanjianqifa:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_A);
                break;

            case SubTypeOfCards.Nanmanruqin:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_K);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_7);
                break;
            case SubTypeOfCards.Taoyuanjieyi:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_A);
                break;
            case SubTypeOfCards.Wugufengdeng:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_4);
                break;
            case SubTypeOfCards.Shunshouqianyang:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_J);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3);
                break;
            case SubTypeOfCards.Guohechaiqiao:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q);

                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_3);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_4);
                break;

            case SubTypeOfCards.Impeccable:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_K);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_A);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_K);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_Q);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_K);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q);
                break;

            case SubTypeOfCards.Thunder:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q);
                break;
            case SubTypeOfCards.Wuzhongshengyou:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_7);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_8);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_9);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_J);
                break;
            case SubTypeOfCards.Lebusishu:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_6);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_6);
                break;
            case SubTypeOfCards.Tiesuolianhuan:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_10);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_J);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_Q);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_K);
                break;
            case SubTypeOfCards.Jiedaosharen:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_Q);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_K);
                break;
            case SubTypeOfCards.Binliangcunduan:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_4);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_10);
                break;
            case SubTypeOfCards.Huogong:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_2);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q);
                break;

            case SubTypeOfCards.SilverLion:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.Baguazhen:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, TypeOfEquipment.Armor);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.Tengjia:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, TypeOfEquipment.Armor);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.Renwangdun:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.FrostBlade://寒冰剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, TypeOfEquipment.Weapons, 2);
                break;
            case SubTypeOfCards.Zhugeliannu://诸葛连弩
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A, TypeOfEquipment.Weapons, 1);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, TypeOfEquipment.Weapons, 1);
                break;
            case SubTypeOfCards.Gudiandao://古锭刀
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, 2);
                break;
            case SubTypeOfCards.Zhuqueyushan://朱雀羽扇
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, TypeOfEquipment.Weapons, 4);
                break;
            case SubTypeOfCards.CixiongDoubleSwards://雌雄双股剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, TypeOfEquipment.Weapons, 2);
                break;
            case SubTypeOfCards.Qinglongyanyuedao://青龙偃月刀
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5, TypeOfEquipment.Weapons, 3);
                break;
            case SubTypeOfCards.Guanshifu://贯石斧
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5, TypeOfEquipment.Weapons, 3);
                break;
            case SubTypeOfCards.Zhangbashemao://丈八蛇矛
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q, TypeOfEquipment.Weapons, 3);
                break;
            case SubTypeOfCards.Fangtianhuaji://方天画戟
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Weapons, 4);
                break;
            case SubTypeOfCards.Qinghongjian://青釭剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6, TypeOfEquipment.Weapons, 2);
                break;
            case SubTypeOfCards.Qilingong://麒麟弓
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5, TypeOfEquipment.Weapons, 5);
                break;
            case SubTypeOfCards.Jueying:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5, TypeOfEquipment.AddAHorse);
                break;
            case SubTypeOfCards.Chitu:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5, TypeOfEquipment.MinusAHorse);
                break;
            case SubTypeOfCards.Dilu:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_5, TypeOfEquipment.AddAHorse);
                break;
            case SubTypeOfCards.Dawan:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_K, TypeOfEquipment.MinusAHorse);
                break;
            case SubTypeOfCards.Zhuahuangfeidian:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_K, TypeOfEquipment.AddAHorse);
                break;
            case SubTypeOfCards.Zixing:
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_K, TypeOfEquipment.MinusAHorse);
                break;
            case SubTypeOfCards.Hualiu:
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_K, TypeOfEquipment.AddAHorse);
                break;
        }
    }

    void addNewCardAsset(CardAsset cardAsset, CardSuits cardSuits, CardRank cardRank, TypeOfEquipment typeOfEquipment = TypeOfEquipment.None, int weaponAttackDistance = 1)
    {
        CardAsset ca = new CardAsset();
        ca.ReadFromAsset(cardAsset);
        ca.Suits = cardSuits;
        ca.CardRank = cardRank;
        ca.TypeOfEquipment = typeOfEquipment;
        ca.WeaponAttackDistance = weaponAttackDistance;

        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, GlobalSettings.Instance.PDeck.ChildCanvas.transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CardAsset = ca;
        manager.ReadCardFromAsset();

        // parent a new creature gameObject to table slots
        card.transform.SetParent(GlobalSettings.Instance.PDeck.ChildCanvas.transform);

        // add a new creature to the list
        DeckCards.Add(card);

        // add our unique ID to this creature
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = IDFactory.GetUniqueID();

        manager.CardLocation = CardLocation.DrawDeck;
        manager.Owner = null;
        manager.UniqueCardID = id.UniqueID;

        if (ca.SpellScriptName != null && ca.SpellScriptName != "")
        {
            manager.Effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
        }

    }
}
