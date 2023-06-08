using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DeckType
{
    Baguazhen,
    Tengjia,
    FrostBlade,
    Jiedaosharen,
    Renwangdun,
    Huogong,
    Tiesuolianhuan,
    Qinggangjian,
    Qinglongyanyue,
    Gudiandao,
    Zhugeliannu,
    Cixiong,
    Guanshifu,
    Zhangbashemao,
    Fangtianhuaji,
    Zhuqueyushan,
    Qilinggong,
    Guohechaiqiao,
}

public class Deck : MonoBehaviour
{

    public List<CardAsset> Cards = new List<CardAsset>();
    public List<GameObject> CardObjs = new List<GameObject>();

    void Awake()
    {
        switch (GlobalSettings.Instance.DeckType)
        {
            case DeckType.Baguazhen:
                //八卦阵
                AddDeckCards(SubTypeOfCards.Baguazhen, TypeOfEquipment.Armor);
                break;
            case DeckType.Tengjia:
                //藤甲
                AddDeckCards(SubTypeOfCards.Tengjia, TypeOfEquipment.Armor);
                break;
            case DeckType.FrostBlade:
                //寒冰剑
                AddDeckCards(SubTypeOfCards.FrostBlade, TypeOfEquipment.Weapons);
                break;
            case DeckType.Jiedaosharen:
                //借刀杀人
                AddDeckCards(SubTypeOfCards.Jiedaosharen, TypeOfEquipment.None);
                break;
            case DeckType.Renwangdun:
                //仁王盾
                AddDeckCards(SubTypeOfCards.Renwangdun, TypeOfEquipment.Armor);
                break;
            case DeckType.Huogong:
                //火攻
                AddDeckCards(SubTypeOfCards.Huogong, TypeOfEquipment.None);
                break;
            case DeckType.Tiesuolianhuan:
                //铁索连环
                AddDeckCards(SubTypeOfCards.Tiesuolianhuan, TypeOfEquipment.None);
                break;
            case DeckType.Qinggangjian:
                //青釭剑
                AddDeckCards(SubTypeOfCards.Qinghongjian, TypeOfEquipment.Weapons);
                break;
            case DeckType.Qinglongyanyue:
                //青龙偃月刀
                AddDeckCards(SubTypeOfCards.Qinglongyanyuedao, TypeOfEquipment.Weapons);
                break;
            case DeckType.Gudiandao:
                //古锭刀
                AddDeckCards(SubTypeOfCards.Gudiandao, TypeOfEquipment.Weapons);
                break;
            case DeckType.Zhugeliannu:
                //诸葛连弩
                AddDeckCards(SubTypeOfCards.Zhugeliannu, TypeOfEquipment.Weapons);
                break;
            case DeckType.Cixiong:
                //雌雄双股剑
                AddDeckCards(SubTypeOfCards.CixiongDoubleSwards, TypeOfEquipment.Weapons);
                break;
            case DeckType.Guanshifu:
                //贯石斧
                AddDeckCards(SubTypeOfCards.Guanshifu, TypeOfEquipment.Weapons);
                break;
            case DeckType.Zhangbashemao:
                //丈八蛇矛
                AddDeckCards(SubTypeOfCards.Zhangbashemao, TypeOfEquipment.Weapons);
                break;
            case DeckType.Fangtianhuaji:
                //方天画戟
                AddDeckCards(SubTypeOfCards.Fangtianhuaji, TypeOfEquipment.Weapons);
                break;
            case DeckType.Zhuqueyushan:
                //朱雀羽扇
                AddDeckCards(SubTypeOfCards.Zhuqueyushan, TypeOfEquipment.Weapons);
                break;
            case DeckType.Qilinggong:
                //麒麟弓
                AddDeckCards(SubTypeOfCards.Qilingong, TypeOfEquipment.Weapons);
                break;
            case DeckType.Guohechaiqiao:
                //过河拆桥
                AddDeckCards(SubTypeOfCards.Guohechaiqiao, TypeOfEquipment.None);
                break;
        }
    }

    public void AddDeckCards(SubTypeOfCards subTypeOfCards, TypeOfEquipment typeOfEquipment)
    {
        Debug.Log("卡牌总数: " + DeckSource.Instance.Cards.Count);
        CardAsset testAsset = null;
        CardAsset wanjianqifaAsset = null;
        CardAsset baguazhenAsset = null;
        CardAsset juedouAsset = null;
        CardAsset renwangdunAsset = null;
        CardAsset jiedaosharenAsset = null;
        CardAsset tengjiaAsset = null;
        CardAsset silverlionAsset = null;
        CardAsset hanbingjianAsset = null;
        CardAsset qinglongyanyueAsset = null;
        CardAsset qinggangjianAsset = null;
        CardAsset tiesuoAsset1 = null;
        CardAsset nanmanruqinAsset = null;
        CardAsset mHorse = null;
        CardAsset aHorse = null;
        CardAsset wine = null;
        foreach (CardAsset cardAsset1 in DeckSource.Instance.Cards)
        {
            HandleCards(cardAsset1);
            if (cardAsset1.SubTypeOfCard == subTypeOfCards)
            {
                testAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Wanjianqifa)
            {
                wanjianqifaAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Baguazhen)
            {
                baguazhenAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Juedou)
            {
                juedouAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Renwangdun)
            {
                renwangdunAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Jiedaosharen)
            {
                jiedaosharenAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Tengjia)
            {
                tengjiaAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.SilverLion)
            {
                silverlionAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.FrostBlade)
            {
                hanbingjianAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Qinglongyanyuedao)
            {
                qinglongyanyueAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Qinghongjian)
            {
                qinggangjianAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Tiesuolianhuan)
            {
                tiesuoAsset1 = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Nanmanruqin)
            {
                nanmanruqinAsset = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Zhuahuangfeidian)
            {
                aHorse = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Chitu)
            {
                mHorse = cardAsset1;
            }
            if (cardAsset1.SubTypeOfCard == SubTypeOfCards.Analeptic)
            {
                wine = cardAsset1;
            }
        }
        Cards.Shuffle();
        CardObjs.Shuffle();

        CardAsset card = new CardAsset();
        card.ReadFromAsset(testAsset);
        card.TypeOfEquipment = typeOfEquipment;
        card.WeaponAttackDistance = testAsset.WeaponAttackDistance;
        Cards.Insert(0, card);

        CardAsset card1 = new CardAsset();
        card1.ReadFromAsset(wanjianqifaAsset);
        card1.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card1);

        CardAsset card2 = new CardAsset();
        card2.ReadFromAsset(baguazhenAsset);
        card2.TypeOfEquipment = TypeOfEquipment.Armor;
        Cards.Insert(0, card2);

        CardAsset card3 = new CardAsset();
        card3.ReadFromAsset(juedouAsset);
        card3.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card3);

        CardAsset card4 = new CardAsset();
        card4.ReadFromAsset(renwangdunAsset);
        card4.TypeOfEquipment = TypeOfEquipment.Armor;
        Cards.Insert(0, card4);

        CardAsset card5 = new CardAsset();
        card5.ReadFromAsset(jiedaosharenAsset);
        card5.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card5);

        CardAsset card6 = new CardAsset();
        card6.ReadFromAsset(tengjiaAsset);
        card6.TypeOfEquipment = TypeOfEquipment.Armor;
        Cards.Insert(0, card6);

        CardAsset card7 = new CardAsset();
        card7.ReadFromAsset(silverlionAsset);
        card7.TypeOfEquipment = TypeOfEquipment.Armor;
        Cards.Insert(0, card7);

        CardAsset card8 = new CardAsset();
        card8.ReadFromAsset(hanbingjianAsset);
        card8.TypeOfEquipment = TypeOfEquipment.Weapons;
        card8.WeaponAttackDistance = hanbingjianAsset.WeaponAttackDistance;
        Cards.Insert(0, card8);

        CardAsset card9 = new CardAsset();
        card9.ReadFromAsset(qinglongyanyueAsset);
        card9.TypeOfEquipment = TypeOfEquipment.Weapons;
        card9.WeaponAttackDistance = qinglongyanyueAsset.WeaponAttackDistance;
        Cards.Insert(0, card9);

        CardAsset card10 = new CardAsset();
        card10.ReadFromAsset(qinggangjianAsset);
        card10.TypeOfEquipment = TypeOfEquipment.Weapons;
        card10.WeaponAttackDistance = qinggangjianAsset.WeaponAttackDistance;
        Cards.Insert(0, card10);

        CardAsset card11 = new CardAsset();
        card11.ReadFromAsset(tiesuoAsset1);
        card11.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card11);

        CardAsset card12 = new CardAsset();
        card12.ReadFromAsset(tiesuoAsset1);
        card12.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card12);

        CardAsset card13 = new CardAsset();
        card13.ReadFromAsset(tengjiaAsset);
        card13.TypeOfEquipment = TypeOfEquipment.Armor;
        Cards.Insert(0, card13);

        CardAsset card14 = new CardAsset();
        card14.ReadFromAsset(nanmanruqinAsset);
        card14.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card14);

        CardAsset card15 = new CardAsset();
        card15.ReadFromAsset(mHorse);
        card15.TypeOfEquipment = TypeOfEquipment.MinusAHorse;
        Cards.Insert(0, card15);

        CardAsset card16 = new CardAsset();
        card16.ReadFromAsset(aHorse);
        card16.TypeOfEquipment = TypeOfEquipment.AddAHorse;
        Cards.Insert(0, card16);

        CardAsset card17 = new CardAsset();
        card17.ReadFromAsset(wine);
        card17.TypeOfEquipment = TypeOfEquipment.None;
        Cards.Insert(0, card17);
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
        Cards.Add(ca);

        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, GlobalSettings.Instance.PDeck.ChildCanvas.transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CardAsset = ca;
        manager.ReadCardFromAsset();

        // parent a new creature gameObject to table slots
        card.transform.SetParent(GlobalSettings.Instance.PDeck.ChildCanvas.transform);

        // add a new creature to the list
        CardObjs.Add(card);

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
