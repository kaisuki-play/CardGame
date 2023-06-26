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
            HandleCards(cardAsset1);
        }
        DeckCards.Shuffle();
        InsertNewCardAsset(JiedaoSharenAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(TiesuoAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(AnalepticAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(TaoyuanjieyiAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(ThunderAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(HuogongAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(WugufengdengAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(NanmanAsset, CardSuits.Spades, CardRank.Rank_A);
        InsertNewCardAsset(WanjianqifaAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(JuedouAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(FthjAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, 4);

        //InsertNewCardAsset(ShunshouqianyangAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(GuohechaiqiaoAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(LbssAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(BlcdAsset, CardSuits.Spades, CardRank.Rank_A);
        //InsertNewCardAsset(ZhugeliannuAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, ZhugeliannuAsset.WeaponAttackDistance);
        //InsertNewCardAsset(CixiongAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, CixiongAsset.WeaponAttackDistance);
        //InsertNewCardAsset(GuanshifuAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, GuanshifuAsset.WeaponAttackDistance);
        //InsertNewCardAsset(QinggangjianAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, QinggangjianAsset.WeaponAttackDistance);
        //InsertNewCardAsset(QinglongyanyueAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, QinglongyanyueAsset.WeaponAttackDistance);
        //InsertNewCardAsset(FangtianhuajiAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, FangtianhuajiAsset.WeaponAttackDistance);
        //InsertNewCardAsset(ZbsmAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, ZbsmAsset.WeaponAttackDistance);
        //InsertNewCardAsset(ZhuqueyushanAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, ZhuqueyushanAsset.WeaponAttackDistance);
        //InsertNewCardAsset(FrostBladeAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, FrostBladeAsset.WeaponAttackDistance);
        //InsertNewCardAsset(QilingongAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, QilingongAsset.WeaponAttackDistance);
        //InsertNewCardAsset(DiluAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.AddAHorse);
        //InsertNewCardAsset(SilverMoonAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Weapons, SilverMoonAsset.WeaponAttackDistance);
        InsertNewCardAsset(BaguazhenAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Armor);
        //InsertNewCardAsset(RenwangdunAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Armor);
        //InsertNewCardAsset(TengjiaAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Armor);
        //InsertNewCardAsset(CartAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Treasure);
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
                CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
                ca.ReadFromAsset(cardAsset);
                ca.Suits = CardSuits.None;
                ca.CardRank = CardRank.Rank_0;
                ca.TypeOfEquipment = cardAsset.TypeOfEquipment;
                ca.WeaponAttackDistance = cardAsset.WeaponAttackDistance;
                ca.Targets = cardAsset.Targets;

                GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, TurnManager.Instance.whoseTurn.PArea.HandVisual.OtherCardDrawSourceTransform.position, Quaternion.identity) as GameObject;

                // apply the look from CardAsset
                OneCardManager manager = card.GetComponent<OneCardManager>();
                manager.CardAsset = ca;
                manager.ReadCardFromAsset();
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
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Zhugeliannu://诸葛连弩
                addNewCardAsset(cardAsset, CardSuits.Clubs, CardRank.Rank_A, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Gudiandao://古锭刀
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_A, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Zhuqueyushan://朱雀羽扇
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_A, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.CixiongDoubleSwards://雌雄双股剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_2, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Qinglongyanyuedao://青龙偃月刀
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_5, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Guanshifu://贯石斧
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_5, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Zhangbashemao://丈八蛇矛
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_Q, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Fangtianhuaji://方天画戟
                addNewCardAsset(cardAsset, CardSuits.Diamonds, CardRank.Rank_Q, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Qinghongjian://青釭剑
                addNewCardAsset(cardAsset, CardSuits.Spades, CardRank.Rank_6, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
                break;
            case SubTypeOfCards.Qilingong://麒麟弓
                addNewCardAsset(cardAsset, CardSuits.Hearts, CardRank.Rank_5, TypeOfEquipment.Weapons, cardAsset.WeaponAttackDistance);
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
        CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
        ca.ReadFromAsset(cardAsset);
        ca.Suits = cardSuits;
        ca.CardRank = cardRank;
        ca.TypeOfEquipment = typeOfEquipment;
        ca.WeaponAttackDistance = weaponAttackDistance;
        ca.Targets = cardAsset.Targets;

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

    void InsertNewCardAsset(CardAsset cardAsset, CardSuits cardSuits, CardRank cardRank, TypeOfEquipment typeOfEquipment = TypeOfEquipment.None, int weaponAttackDistance = 1)
    {
        CardAsset ca = ScriptableObject.CreateInstance<CardAsset>();
        ca.ReadFromAsset(cardAsset);
        ca.Suits = cardSuits;
        ca.CardRank = cardRank;
        ca.TypeOfEquipment = typeOfEquipment;
        ca.WeaponAttackDistance = weaponAttackDistance;
        ca.Targets = cardAsset.Targets;

        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, GlobalSettings.Instance.PDeck.ChildCanvas.transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CardAsset = ca;
        manager.ReadCardFromAsset();

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
