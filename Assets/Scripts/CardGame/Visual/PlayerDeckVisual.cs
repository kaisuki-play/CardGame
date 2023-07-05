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
        CardAsset JiedaoSharenAsset = null;
        CardAsset NanmanAsset = null;
        CardAsset JuedouAsset = null;
        CardAsset FthjAsset = null;
        CardAsset ZbsmAsset = null;
        CardAsset ShunshouqianyangAsset = null;
        CardAsset GuohechaiqiaoAsset = null;
        CardAsset WugufengdengAsset = null;
        CardAsset TaoyuanjieyiAsset = null;
        CardAsset HuogongAsset = null;
        CardAsset AnalepticAsset = null;
        CardAsset TiesuoAsset = null;
        CardAsset LbssAsset = null;
        CardAsset BlcdAsset = null;
        CardAsset ThunderAsset = null;
        CardAsset ZhugeliannuAsset = null;
        CardAsset CixiongAsset = null;
        CardAsset GuanshifuAsset = null;
        CardAsset QinggangjianAsset = null;
        CardAsset QinglongyanyueAsset = null;
        CardAsset FangtianhuajiAsset = null;
        CardAsset ZhuqueyushanAsset = null;
        CardAsset FrostBladeAsset = null;
        CardAsset QilingongAsset = null;
        CardAsset DiluAsset = null;
        CardAsset SilverMoonAsset = null;
        CardAsset WanjianqifaAsset = null;
        CardAsset BaguazhenAsset = null;
        CardAsset RenwangdunAsset = null;
        CardAsset TengjiaAsset = null;
        CardAsset CartAsset = null;
        CardAsset ThunderHarmer = null;
        CardAsset VictorySword = null;
        foreach (CardAsset cardAsset1 in GlobalSettings.Instance.DeckSource.Cards)
        {
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Shunshouqianyang)
            {
                ShunshouqianyangAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Jiedaosharen)
            {
                JiedaoSharenAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Nanmanruqin)
            {
                NanmanAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Wanjianqifa)
            {
                WanjianqifaAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Juedou)
            {
                JuedouAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Fangtianhuaji)
            {
                FthjAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Zhangbashemao)
            {
                ZbsmAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Guohechaiqiao)
            {
                GuohechaiqiaoAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Wugufengdeng)
            {
                WugufengdengAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Taoyuanjieyi)
            {
                TaoyuanjieyiAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Huogong)
            {
                HuogongAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Analeptic)
            {
                AnalepticAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Tiesuolianhuan)
            {
                TiesuoAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Lebusishu)
            {
                LbssAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Binliangcunduan)
            {
                BlcdAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Thunder)
            {
                ThunderAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Zhugeliannu)
            {
                ZhugeliannuAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.CixiongDoubleSwards)
            {
                CixiongAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Guanshifu)
            {
                GuanshifuAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Qinghongjian)
            {
                QinggangjianAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Qinglongyanyuedao)
            {
                QinglongyanyueAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Fangtianhuaji)
            {
                FangtianhuajiAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Zhuqueyushan)
            {
                ZhuqueyushanAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.FrostBlade)
            {
                FrostBladeAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Qilingong)
            {
                QilingongAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Dilu)
            {
                DiluAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.SilverMoon)
            {
                SilverMoonAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Baguazhen)
            {
                BaguazhenAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Renwangdun)
            {
                RenwangdunAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Tengjia)
            {
                TengjiaAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Cart)
            {
                CartAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.ThunderHarmer)
            {
                ThunderHarmer = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.VictorySword)
            {
                VictorySword = cardAsset1;
            }
            HandleCards(cardAsset1);
        }
        DeckCards.Shuffle();
        InsertNewCardAsset(JiedaoSharenAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(TiesuoAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(AnalepticAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(TaoyuanjieyiAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(ThunderAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(HuogongAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(WugufengdengAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        InsertNewCardAsset(NanmanAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(WanjianqifaAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(JuedouAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(FthjAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, 4);

        InsertNewCardAsset(ShunshouqianyangAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(GuohechaiqiaoAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(LbssAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        InsertNewCardAsset(BlcdAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
        //InsertNewCardAsset(ZhugeliannuAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, ZhugeliannuAsset.WeaponAttackDistance);
        //InsertNewCardAsset(CixiongAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, CixiongAsset.WeaponAttackDistance);
        //InsertNewCardAsset(GuanshifuAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, GuanshifuAsset.WeaponAttackDistance);
        //InsertNewCardAsset(QinggangjianAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, QinggangjianAsset.WeaponAttackDistance);
        InsertNewCardAsset(QinglongyanyueAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, QinglongyanyueAsset.WeaponAttackDistance);
        //InsertNewCardAsset(FangtianhuajiAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, FangtianhuajiAsset.WeaponAttackDistance);
        //InsertNewCardAsset(ZbsmAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, ZbsmAsset.WeaponAttackDistance);
        //InsertNewCardAsset(ZhuqueyushanAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, ZhuqueyushanAsset.WeaponAttackDistance);
        //InsertNewCardAsset(FrostBladeAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, FrostBladeAsset.WeaponAttackDistance);
        //InsertNewCardAsset(QilingongAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, QilingongAsset.WeaponAttackDistance);
        //InsertNewCardAsset(DiluAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.AddAHorse);
        //InsertNewCardAsset(SilverMoonAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red, TypeOfEquipment.Weapons, SilverMoonAsset.WeaponAttackDistance);
        //InsertNewCardAsset(BaguazhenAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red, TypeOfEquipment.Armor);
        //InsertNewCardAsset(RenwangdunAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red, TypeOfEquipment.Armor);
        //InsertNewCardAsset(TengjiaAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red, TypeOfEquipment.Armor);
        //InsertNewCardAsset(CartAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red, TypeOfEquipment.Treasure);
        //InsertNewCardAsset(ThunderHarmer, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, ThunderHarmer.WeaponAttackDistance);
        //InsertNewCardAsset(VictorySword, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, VictorySword.WeaponAttackDistance);
        Debug.Log("卡牌总数: " + GlobalSettings.Instance.DeckSource.Cards.Count);
    }

    /// <summary>
    /// 创造伪牌
    /// </summary>
    /// <param name="subTypeOfCards"></param>
    /// <returns></returns>
    public OneCardManager DisguisedCardAssetWithType(Player player, SubTypeOfCards subTypeOfCards, List<int> relationCardIds, bool needTarget)
    {
        foreach (CardAsset cardAsset in GlobalSettings.Instance.DeckSource.Cards)
        {
            if (cardAsset.SubTypeOfCard == subTypeOfCards)
            {
                (CardSuits newCardSuits, CardRank newCardRank, CardColor newCardColor) = DisguisedCardSuit(relationCardIds);
                Debug.Log("~~~~~~~~~~" + newCardSuits + "~~~~~~~~~~" + newCardRank + "~~~~~~~~~~~~~~~~~~" + newCardColor);

                CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
                ca.ReadFromAsset(cardAsset);
                ca.Suits = newCardSuits;
                ca.CardRank = newCardRank;
                ca.CardColor = newCardColor;
                ca.TypeOfEquipment = cardAsset.TypeOfEquipment;
                ca.WeaponAttackDistance = cardAsset.WeaponAttackDistance;
                ca.Targets = cardAsset.Targets;

                GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, TurnManager.Instance.whoseTurn.PArea.HandVisual.OtherCardDrawSourceTransform.position, Quaternion.identity) as GameObject;

                // apply the look from CardAsset
                OneCardManager manager = card.GetComponent<OneCardManager>();
                manager.SetCardAssetA(ca);
                manager.ReadCardFromAssetA();
                if (needTarget)
                {
                    manager.TargetComponent.SetActive(true);
                }
                else
                {
                    manager.TargetComponent.SetActive(false);
                }

                // parent a new creature gameObject to table slots
                card.transform.SetParent(TurnManager.Instance.whoseTurn.PArea.HandVisual.OtherCardDrawSourceTransform.transform);

                // add our unique ID to this creature
                IDHolder id = card.AddComponent<IDHolder>();
                id.UniqueID = IDFactory.GetUniqueID();

                manager.CardLocation = CardLocation.DrawDeck;
                manager.Owner = player;
                manager.UniqueCardID = id.UniqueID;
                manager.IsDisguisedCard = true;
                manager.RelationRealCardIds = relationCardIds;
                return manager;
            }
        }
        return null;
    }

    public CardAsset CardAssetBWithType(SubTypeOfCards subTypeOfCards, CardAsset oldCardAsset)
    {
        foreach (CardAsset cardAsset in GlobalSettings.Instance.DeckSource.Cards)
        {
            if (cardAsset.SubTypeOfCard == subTypeOfCards)
            {
                CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
                ca.ReadFromAsset(cardAsset);
                ca.Suits = oldCardAsset.Suits;
                ca.CardRank = oldCardAsset.CardRank;
                ca.CardColor = oldCardAsset.CardColor;
                return ca;
            }
        }
        return oldCardAsset;
    }

    public CardAsset CardAssetBWithSuite(CardSuits cardSuits, CardAsset oldCardAsset)
    {
        CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
        ca.ReadFromAsset(oldCardAsset);
        ca.Suits = cardSuits;
        ca.CardRank = oldCardAsset.CardRank;
        ca.CardColor = CardColor.Red;
        return ca;
    }


    public (CardSuits, CardRank, CardColor) DisguisedCardSuit(List<int> relationCardIds)
    {
        if (relationCardIds.Count == 0)
        {
            return (CardSuits.None, CardRank.Rank_0, CardColor.None);
        }
        else
        {
            CardSuits cardSuits = IDHolder.GetGameObjectWithID(relationCardIds[0]).GetComponent<OneCardManager>().CardAsset.Suits;
            CardRank cardRank = IDHolder.GetGameObjectWithID(relationCardIds[0]).GetComponent<OneCardManager>().CardAsset.CardRank;
            CardColor cardColor = IDHolder.GetGameObjectWithID(relationCardIds[0]).GetComponent<OneCardManager>().CardAsset.CardColor;
            if (relationCardIds.Count > 1)
            {
                for (int i = 1; i < relationCardIds.Count; i++)
                {
                    OneCardManager cardManager = IDHolder.GetGameObjectWithID(relationCardIds[i]).GetComponent<OneCardManager>();
                    if (cardManager.CardAsset.Suits != cardSuits)
                    {
                        cardSuits = CardSuits.None;
                        break;
                    }
                }
                for (int i = 1; i < relationCardIds.Count; i++)
                {
                    OneCardManager cardManager = IDHolder.GetGameObjectWithID(relationCardIds[i]).GetComponent<OneCardManager>();
                    if (cardManager.CardAsset.CardRank != cardRank)
                    {
                        cardRank = CardRank.Rank_0;
                        break;
                    }
                }
                for (int i = 1; i < relationCardIds.Count; i++)
                {
                    OneCardManager cardManager = IDHolder.GetGameObjectWithID(relationCardIds[i]).GetComponent<OneCardManager>();
                    Debug.Log("........." + cardManager.CardAsset.CardColor + " " + cardColor);
                    if (cardManager.CardAsset.CardColor != cardColor)
                    {
                        cardColor = CardColor.None;
                        break;
                    }
                }
            }
            return (cardSuits, cardRank, cardColor);
        }
    }

    void HandleCards(CardAsset cardAsset)
    {
        switch (cardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Slash:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_7, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_8, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_8, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_9, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_9, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_10, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_10, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_10, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_10, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_J, CardColor.Red);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_3, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_4, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_5, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_6, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_7, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_8, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_8, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_9, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_9, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_10, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_10, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_J, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_J, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_6, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_7, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_8, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_9, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_10, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_K, CardColor.Red);

                break;
            case SubTypeOfCards.Jink:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_2, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_2, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_8, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_9, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_J, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_K, CardColor.Red);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_2, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_2, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_4, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_6, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_6, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_7, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_7, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_8, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_8, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_9, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_10, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_10, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_J, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_J, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_J, CardColor.Red);

                break;

            case SubTypeOfCards.Peach:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_3, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_4, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_6, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_6, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_7, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_8, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_9, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q, CardColor.Red);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_2, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3, CardColor.Red);
                break;

            case SubTypeOfCards.ThunderSlash:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_4, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_7, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_8, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_5, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_6, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_7, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_8, CardColor.Black);
                break;

            case SubTypeOfCards.FireSlash:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_3, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_4, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_7, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_10, CardColor.Red);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_4, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5, CardColor.Red);
                break;

            case SubTypeOfCards.Analeptic:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_3, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_9, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_3, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_9, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_9, CardColor.Red);
                break;

            case SubTypeOfCards.Juedou:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, CardColor.Red);
                break;

            case SubTypeOfCards.Wanjianqifa:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_A, CardColor.Red);
                break;

            case SubTypeOfCards.Nanmanruqin:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_7, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_K, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_7, CardColor.Black);
                break;
            case SubTypeOfCards.Taoyuanjieyi:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_A, CardColor.Red);
                break;
            case SubTypeOfCards.Wugufengdeng:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_3, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_4, CardColor.Red);
                break;
            case SubTypeOfCards.Shunshouqianyang:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_3, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_4, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_J, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_4, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_3, CardColor.Red);
                break;
            case SubTypeOfCards.Guohechaiqiao:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_3, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_4, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q, CardColor.Black);

                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q, CardColor.Red);

                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_3, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_4, CardColor.Black);
                break;

            case SubTypeOfCards.Impeccable:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_K, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_J, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_A, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_K, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_Q, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_K, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red);
                break;

            case SubTypeOfCards.Thunder:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_Q, CardColor.Red);
                break;
            case SubTypeOfCards.Wuzhongshengyou:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_7, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_8, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_9, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_J, CardColor.Red);
                break;
            case SubTypeOfCards.Lebusishu:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_6, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_6, CardColor.Black);
                break;
            case SubTypeOfCards.Tiesuolianhuan:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_J, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_10, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_J, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_Q, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_K, CardColor.Black);
                break;
            case SubTypeOfCards.Jiedaosharen:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_Q, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_K, CardColor.Black);
                break;
            case SubTypeOfCards.Binliangcunduan:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_4, CardColor.Black);
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_10, CardColor.Black);
                break;
            case SubTypeOfCards.Huogong:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_2, CardColor.Red);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red);
                break;

            case SubTypeOfCards.SilverLion:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.Baguazhen:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Armor);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.Tengjia:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Armor);
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.Renwangdun:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Armor);
                break;
            case SubTypeOfCards.FrostBlade://寒冰剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Zhugeliannu://诸葛连弩
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, CardColor.Red, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Gudiandao://古锭刀
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Zhuqueyushan://朱雀羽扇
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, CardColor.Red, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.CixiongDoubleSwards://雌雄双股剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Qinglongyanyuedao://青龙偃月刀
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Guanshifu://贯石斧
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5, CardColor.Red, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Zhangbashemao://丈八蛇矛
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Fangtianhuaji://方天画戟
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q, CardColor.Red, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Qinghongjian://青釭剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6, CardColor.Black, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Qilingong://麒麟弓
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5, CardColor.Red, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Jueying:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5, CardColor.Black, TypeOfEquipment.AddAHorse);
                break;
            case SubTypeOfCards.Chitu:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5, CardColor.Red, TypeOfEquipment.MinusAHorse);
                break;
            case SubTypeOfCards.Dilu:
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_5, CardColor.Black, TypeOfEquipment.AddAHorse);
                break;
            case SubTypeOfCards.Dawan:
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_K, CardColor.Black, TypeOfEquipment.MinusAHorse);
                break;
            case SubTypeOfCards.Zhuahuangfeidian:
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_K, CardColor.Red, TypeOfEquipment.AddAHorse);
                break;
            case SubTypeOfCards.Zixing:
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_K, CardColor.Red, TypeOfEquipment.MinusAHorse);
                break;
            case SubTypeOfCards.Hualiu:
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_K, CardColor.Red, TypeOfEquipment.AddAHorse);
                break;
        }
    }

    void addNewCardAsset(CardAsset cardAsset, CardSuits cardSuits, CardRank cardRank, CardColor cardColor, TypeOfEquipment typeOfEquipment = TypeOfEquipment.None, int weaponAttackDistance = 1)
    {
        CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
        ca.ReadFromAsset(cardAsset);
        ca.Suits = cardSuits;
        ca.CardRank = cardRank;
        ca.CardColor = cardColor;
        ca.TypeOfEquipment = typeOfEquipment;
        ca.WeaponAttackDistance = weaponAttackDistance;
        ca.Targets = cardAsset.Targets;

        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, GlobalSettings.Instance.PDeck.ChildCanvas.transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.SetCardAssetA(ca);
        manager.ReadCardFromAssetA();

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

    void InsertNewCardAsset(CardAsset cardAsset, CardSuits cardSuits, CardRank cardRank, CardColor cardColor, TypeOfEquipment typeOfEquipment = TypeOfEquipment.None, int weaponAttackDistance = 1)
    {
        CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
        ca.ReadFromAsset(cardAsset);
        ca.Suits = cardSuits;
        ca.CardRank = cardRank;
        ca.CardColor = cardColor;
        ca.TypeOfEquipment = typeOfEquipment;
        ca.WeaponAttackDistance = weaponAttackDistance;
        ca.Targets = cardAsset.Targets;

        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, GlobalSettings.Instance.PDeck.ChildCanvas.transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.SetCardAssetA(ca);
        manager.ReadCardFromAssetA();

        // parent a new creature gameObject to table slots
        card.transform.SetParent(GlobalSettings.Instance.PDeck.ChildCanvas.transform);

        // add a new creature to the list
        DeckCards.Insert(0, card);

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
