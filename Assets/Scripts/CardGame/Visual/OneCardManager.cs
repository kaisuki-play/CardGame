using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum CardLocation
{
    DrawDeck,
    DisDeck,
    Table,
    Hand,
    Judgement,
    Equipment,
    //游戏外的牌
    HeroA, // 武将A
    HeroB, // 武将B
    HeroO, // 其他人给的
    UnderCart //木流牛马里的
}

public enum CardRegion
{
    OnTableCard,//包括所有玩家的手牌、装备牌、判定牌
    PlayerCard,//包括一名玩家的手牌、装备牌
    PlayerRegion,//包括一名玩家的手牌、装备牌、判定牌
    OutOfGame
}

public enum DragOrClick
{
    Drag,
    Click
}

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour
{
    public int UniqueCardID;
    public CardAsset CardAsset;

    [Header("Image References")]
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image HighlightGlowImage;

    [Header("Target Component")]
    public GameObject TargetComponent;
    [Header("Card Suits Rank")]
    public Image CardSuitsImage;
    public Text CardRankText;

    [Header("Card Spell Effect")]
    public SpellEffect Effect;

    [Header("Card Owner Location")]
    public Player Owner;
    public CardLocation CardLocation;
    public Player PlayCardPlayer;
    public List<int> TargetsPlayerIDs;
    public List<int> SpecialTargetPlayerIDs;
    public bool isUsedCard = false;

    [Header("Fire Attack")]
    public bool ShownCard;
    public CardSuits ShownCardSuit;

    [Header("Disguised Cards")]
    public bool IsDisguisedCard;
    public List<int> RelationRealCardIds;

    public static Dictionary<int, OneCardManager> CardsCreatedThisGame = new Dictionary<int, OneCardManager>();

    void Awake()
    {
        if (CardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            HighlightGlowImage.enabled = value;
        }
    }

    public string CardRankString(CardRank cardRank)
    {
        switch (cardRank)
        {
            case CardRank.Rank_A:
                return "A";
            case CardRank.Rank_2:
                return "2";
            case CardRank.Rank_3:
                return "3";
            case CardRank.Rank_4:
                return "4";
            case CardRank.Rank_5:
                return "5";
            case CardRank.Rank_6:
                return "6";
            case CardRank.Rank_7:
                return "7";
            case CardRank.Rank_8:
                return "8";
            case CardRank.Rank_9:
                return "9";
            case CardRank.Rank_10:
                return "10";
            case CardRank.Rank_J:
                return "J";
            case CardRank.Rank_Q:
                return "Q";
            case CardRank.Rank_K:
                return "K";
            default:
                return "None";
        }
    }

    public void ReadCardFromAsset()
    {
        if (CardSuitsImage != null)
        {
            CardSuitsImage.sprite = GlobalSettings.Instance.CardSuits[(int)CardAsset.Suits];
        }
        if (CardRankText != null)
        {
            CardRankText.text = CardRankString(CardAsset.CardRank);
        }
        CardGraphicImage.sprite = CardAsset.CardImage;
    }

    public async Task ChangeOwnerAndLocation(Player owner, CardLocation cardLocation)
    {

        switch (cardLocation)
        {
            case CardLocation.DisDeck:
                {
                    //到弃牌堆之后 清理pending数组，新增弃牌堆数组
                    GlobalSettings.Instance.Table.CardsOnTable.Remove(this.gameObject);
                    GlobalSettings.Instance.DisDeck.DisDeckCards.Add(this.gameObject);
                }
                break;
            case CardLocation.Table:
                {
                    //卡牌到pending后触发
                    await SkillManager.AfterUsedCardPending(this);
                    Debug.Log("----------------------------pending checkpoint完成 继续往下走-----------------------------");
                }
                break;
        }

        await SkillManager.CardLocationChanged(this, owner, this.Owner, cardLocation, this.CardLocation);

        if (this.CardLocation == CardLocation.Equipment && cardLocation != CardLocation.Equipment)
        {
            Vector3 newScale = new Vector3(1, 1, 1f);
            if (cardLocation == CardLocation.DisDeck)
            {
                newScale = new Vector3(200, 200, 1f);
            }
            this.gameObject.transform.localScale = newScale;
        }
        this.Owner = owner;
        this.CardLocation = cardLocation;
        await TaskManager.Instance.DontAwait();
    }
}
