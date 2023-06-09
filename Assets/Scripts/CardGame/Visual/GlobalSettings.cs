﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class GlobalSettings : MonoBehaviour
{
    [Header("Golden Finger")]
    public bool OneKeyImpeccable;
    public bool UseGoldenFinger;
    public GameObject GoldenFingerCardPrefab;
    public DeckType DeckType;
    [Header("Players")]
    public Player[] PlayerInstances;
    [Header("PlayDeck")]
    public PlayerDeckVisual PDeck;
    [Header("DisCard ShowCard")]
    public DisDeckVisual DisDeck;
    public TableVisual Table;
    [Header("Deck")]
    public DeckSource DeckSource;
    [Header("Colors")]
    public Color32 CardBodyStandardColor;
    public Color32 CardRibbonsStandardColor;
    public Color32 CardGlowColor;
    [Header("Numbers and Values")]
    public float CardPreviewTime = 0.1f;
    public float CardTransitionTime = 0.1f;
    public float CardPreviewTimeFast = 0.1f;
    public float CardTransitionTimeFast = 0.1f;
    [Header("Prefabs and Assets")]
    //public GameObject NoTargetSupportDragOnTargetPrefab;
    public GameObject ShowCardsForSelectingPrefab;
    public GameObject ShowCardAreaCardNoGiveOtherCardPrefab;
    public GameObject ShowCardAreaCardPrefab;
    public GameObject DisCardAreaCardPrefab;
    public GameObject BaseCardPrefab;
    public GameObject BaseNoTargetCardPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    public GameObject ButtonPrefab;
    public GameObject SkillShowCardPrefab;
    public GameObject HealthPrefab;
    [Header("Other")]
    public Button CenterButton;
    public GameObject GameOverPanel;
    [Header("Card Suit Images")]
    public Sprite[] CardSuits;
    [Header("SelectCardVisual")]
    public Button GlobalButton;
    [Header("BaseCardAsset")]
    public CardAsset SlashAsset;
    public CardAsset FireSlashAsset;
    public CardAsset ThunderSlashAsset;
    public CardAsset PeachSlashAsset;
    public CardAsset AnapliticSlashAsset;
    public CardAsset JinkAsset;

    public Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();

    public static GlobalSettings Instance;

    void Awake()
    {
        Instance = this;
        Players.Add(AreaPosition.Main, PlayerInstances[0]);
        Players.Add(AreaPosition.Opponent1, PlayerInstances[1]);
        Players.Add(AreaPosition.Opponent2, PlayerInstances[2]);
        Players.Add(AreaPosition.Opponent3, PlayerInstances[3]);
        Players.Add(AreaPosition.Opponent4, PlayerInstances[4]);
        Players.Add(AreaPosition.Opponent5, PlayerInstances[5]);

    }

    // remove players who is dead from PlayerInstances
    public void RemoveDiePlayer()
    {
        PlayerInstances = PlayerInstances.Where(n => n.IsDie == false).ToArray();
    }

    // Determine if it is a player, because there was a creature card before, keep it for the time being, and then remove it
    public bool IsPlayer(int id)
    {
        if (PlayerInstances[0].ID == id || PlayerInstances[1].ID == id || PlayerInstances[2].ID == id || PlayerInstances[3].ID == id || PlayerInstances[4].ID == id || PlayerInstances[5].ID == id)
            return true;
        else
            return false;
    }

    // Find the Player logical object according to the ID, which corresponds to the Player in Logic in the Hierarchy.
    public Player FindPlayerByID(int id)
    {
        foreach (Player player in PlayerInstances)
        {
            if (player.ID == id)
            {
                return player;
            }
        }
        return null;
    }
}
