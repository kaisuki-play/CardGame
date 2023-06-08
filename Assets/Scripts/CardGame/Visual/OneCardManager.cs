using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum CardLocation
{
    DrawDeck,
    DisDeck,
    Table,
    Hand,
    Judgement,
    Equipment,
    Warrior,
    OutOfGame
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
    public List<Player> TargetsPlayer;

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
                return "A";
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

    public void ChangeOwnerAndLocation(Player owner, CardLocation cardLocation)
    {
        this.Owner = owner;
        this.CardLocation = cardLocation;
    }
}
