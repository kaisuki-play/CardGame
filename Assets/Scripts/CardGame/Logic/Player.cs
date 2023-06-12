﻿using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class PersonComparer : IComparer<Player>
{
    public int Compare(Player x, Player y)
    {
        if (x.PArea.Owner < y.PArea.Owner)
        {
            return 1;
        }
        else if (x.PArea.Owner > y.PArea.Owner)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}

public class Player : MonoBehaviour
{
    // PUBLIC FIELDS
    // int ID that we get from ID factory
    public int PlayerID;
    // a Character Asset that contains data about this Hero
    public CharacterAsset CharAsset;
    // a script with references to all the visual game objects for this player
    public PlayerArea PArea;

    // REFERENCES TO LOGICAL STUFF THAT BELONGS TO THIS PLAYER
    public Hand Hand;
    public Judgement JudgementLogic;
    public Equipment EquipmentLogic;


    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get { return PlayerID; }
    }

    private bool _isDie = false;
    public bool IsDie
    {
        get
        {
            return _isDie;
        }

        set
        {
            _isDie = value;
            this.PArea.Portrait.DieImage.gameObject.SetActive(_isDie);
        }
    }

    // Look counterclockwise to find the player next in the current player's pick
    public Player OtherPlayer
    {
        get
        {
            int currentIndex = 0;
            for (int i = 0; i < GlobalSettings.Instance.PlayerInstances.Length; i++)
            {
                if (GlobalSettings.Instance.PlayerInstances[i].ID == this.ID)
                {
                    currentIndex = i;
                    break;
                }
            }
            if (currentIndex > 0)
            {
                return GlobalSettings.Instance.PlayerInstances[currentIndex - 1];
            }
            else
            {
                return GlobalSettings.Instance.PlayerInstances[GlobalSettings.Instance.PlayerInstances.Length - 1];
            }
        }
    }

    public void TransmitInfoAboutPlayerToVisual()
    {
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
    }


    // Calculate clockwise according to counterclockwise whether it is within range of the attack TODO:need to add distance in the future.
    public bool CanAttack(int targetID)
    {
        int distanceCanTouch = 1;
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);
        //TODO 武器距离
        //(bool hasWeapon, CardLogic cardLogic) = HasEquipmentWithType(TypeOfEquipment.Weapons);
        //if (hasWeapon)
        //{
        //    distanceCanTouch = cardLogic.Ca.WeaponAttackDistance;
        //}

        int currentIndex = 0;
        int targetIndex = 0;
        for (int i = 0; i < GlobalSettings.Instance.PlayerInstances.Length; i++)
        {
            if (GlobalSettings.Instance.PlayerInstances[i].ID == this.ID)
            {
                currentIndex = i;
            }
            if (GlobalSettings.Instance.PlayerInstances[i].ID == targetID)
            {
                targetIndex = i;
            }
        }

        int distance = 0;
        int distanceReverse = 0;
        if (targetIndex > currentIndex)
        {
            distance = currentIndex + (GlobalSettings.Instance.PlayerInstances.Length - targetIndex);
        }
        else
        {
            distance = currentIndex - targetIndex;
        }

        if (targetIndex > currentIndex)
        {
            distanceReverse = targetIndex - currentIndex;
        }
        else
        {
            distanceReverse = targetIndex + (GlobalSettings.Instance.PlayerInstances.Length - currentIndex);
        }

        int dis = CalculateDistance(targetPlayer, distance);
        int disR = CalculateDistance(targetPlayer, distanceReverse);

        if (dis <= distanceCanTouch || disR <= distanceCanTouch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int CalculateDistance(Player targetPlayer, int originDistance)
    {
        bool playerHasMinusHorse = false;
        foreach (int cardId in this.EquipmentLogic.CardsInEquipment)
        {
            GameObject card = IDHolder.GetGameObjectWithID(cardId);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfEquipment == TypeOfEquipment.MinusAHorse)
            {
                playerHasMinusHorse = true;
                break;
            }
        }
        bool targetHasAddHorse = false;
        foreach (int cardId in targetPlayer.EquipmentLogic.CardsInEquipment)
        {
            GameObject card = IDHolder.GetGameObjectWithID(cardId);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfEquipment == TypeOfEquipment.AddAHorse)
            {
                targetHasAddHorse = true;
                break;
            }
        }
        return originDistance - (playerHasMinusHorse ? 1 : 0) + (targetHasAddHorse ? 1 : 0);
    }

    // health for character
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            int maxHealth = (PArea.Portrait.TotalHealth > 0 ? PArea.Portrait.TotalHealth : CharAsset.MaxHealth);
            if (value > maxHealth)
            {
                _health = maxHealth;
            }
            else
            {
                _health = value;
            }
            if (value <= 0)
            {
                Die();
            }

        }
    }

    private bool _showOp1Button = false;
    public bool ShowOp1Button
    {
        get
        {
            return _showOp1Button;
        }

        set
        {
            _showOp1Button = value;
            PArea.Portrait.OpButton1.gameObject.SetActive(value);
        }
    }

    private bool _showOp2Button = false;
    public bool ShowOp2Button
    {
        get
        {
            return _showOp2Button;
        }

        set
        {
            _showOp2Button = value;
            PArea.Portrait.OpButton2.gameObject.SetActive(value);
        }
    }

    private bool _showOp3Button = false;
    public bool ShowOp3Button
    {
        get
        {
            return _showOp3Button;
        }

        set
        {
            _showOp3Button = value;
            PArea.Portrait.OpButton2.gameObject.SetActive(value);
        }
    }

    private bool _showTargetGlow = false;
    public bool ShowTargetGlow
    {
        get
        {
            return _showTargetGlow;
        }
        set
        {
            _showTargetGlow = value;
            PArea.Portrait.TargetGlowImage.gameObject.SetActive(value);
        }
    }

    private bool _isThisTurn = false;
    public bool IsThisTurn
    {
        get
        {
            return _isThisTurn;
        }
        set
        {
            _isThisTurn = value;
            PArea.Portrait.PortraitGlowImage.gameObject.SetActive(value);
        }
    }

    // ALL METHODS
    void Awake()
    {
        // obtain unique id from IDFactory
        PlayerID = IDFactory.GetUniqueID();
    }

    public virtual void OnTurnStart()
    {

    }

    public void OnTurnEnd()
    {
        //重置杀的次数限制
        CounterManager.Instance.ResetSlashLimit();
        CounterManager.Instance.ResetAnaleptic();
    }


    // STUFF THAT OUR PLAYER CAN DO

    public void DrawSomeCards(int numberOfCards)
    {
        if (GlobalSettings.Instance.PDeck.DeckCards.Count > 0)
        {
            if (Hand.CardsInHand.Count < PArea.HandVisual.Slots.Children.Length)
            {
                OneCardManager[] cardManagers = new OneCardManager[numberOfCards];
                for (int i = 0; i < numberOfCards; i++)
                {
                    // 1) logic: add card to hand
                    GameObject card = GlobalSettings.Instance.PDeck.DeckCards[i];
                    OneCardManager cardManager = card.GetComponent<OneCardManager>();
                    Hand.CardsInHand.Insert(0, cardManager.UniqueCardID);
                    // Debug.Log(hand.CardsInHand.Count);
                    // 2) logic: remove the card from the deck
                    GlobalSettings.Instance.PDeck.DeckCards.RemoveAt(0);
                    this.PArea.HandVisual.DrawACard(card);
                }
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }
    }

    public void DrawACard(bool fast = false)
    {
        if (GlobalSettings.Instance.PDeck.DeckCards.Count > 0)
        {
            if (Hand.CardsInHand.Count < PArea.HandVisual.Slots.Children.Length)
            {
                GameObject card = GlobalSettings.Instance.PDeck.DeckCards[0];
                OneCardManager cardManager = card.GetComponent<OneCardManager>();

                OneCardManager newCard = card.GetComponent<OneCardManager>();
                newCard.ChangeOwnerAndLocation(this, CardLocation.Hand);

                Hand.CardsInHand.Insert(0, newCard.UniqueCardID);
                GlobalSettings.Instance.PDeck.DeckCards.RemoveAt(0);

                // 2) create a command
                this.PArea.HandVisual.DrawACard(card);
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }

    }

    /// <summary>
    /// 使用牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public void UseACard(OneCardManager playedCard, List<int> targets)
    {
        playedCard.isUsedCard = true;
        PlayASpellFromHand(playedCard, targets);
    }

    /// <summary>
    /// 打出牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public void PlayACard(OneCardManager playedCard, List<int> targets)
    {
        if (playedCard.CardAsset.TypeOfCard == TypesOfCards.Tips && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Impeccable)
        {
            playedCard.isUsedCard = true;
        }
        else
        {
            playedCard.isUsedCard = false;
        }
        PlayASpellFromHand(playedCard, targets);
    }

    // 1
    /// <summary>
    /// 使用牌：当一个玩家，点了一张手牌，然后点了默认的目标，则视为玩家已经使用了此牌。
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="target"></param>
    public void PlayASpellFromHand(OneCardManager playedCard, List<int> targets)
    {
        // 默认赋值目标
        switch (playedCard.CardAsset.SubTypeOfCard)
        {
            case SubTypeOfCards.Peach:
            case SubTypeOfCards.Wuzhongshengyou:
            case SubTypeOfCards.Thunder:
                targets.Add(playedCard.Owner.ID);
                break;
            case SubTypeOfCards.Nanmanruqin:
            case SubTypeOfCards.Wanjianqifa:
                foreach (Player player in GlobalSettings.Instance.PlayerInstances)
                {
                    if (player.ID != playedCard.Owner.ID)
                    {
                        targets.Add(player.ID);
                    }
                }
                break;
            case SubTypeOfCards.Wugufengdeng:
            case SubTypeOfCards.Taoyuanjieyi:
                foreach (Player player in GlobalSettings.Instance.PlayerInstances)
                {
                    targets.Add(player.ID);
                }
                break;
        }

        playedCard.TargetsPlayerIDs = targets;

        // remove this card from hand
        Hand.DisCard(playedCard.UniqueCardID);

        this.PArea.HandVisual.PlayASpellFromHand(playedCard.UniqueCardID);

    }



    public void DamageEffect(int amount)
    {
        this.Health -= amount;
        this.PArea.Portrait.TakeDamage(amount, this.Health);
    }


    public void FlopCard(CardAsset c)
    {
        // Instantiate a card depending on its type
        GameObject card = GameObject.Instantiate(GlobalSettings.Instance.BaseCardPrefab, TurnManager.Instance.whoseTurn.PArea.HandVisual.DeckTransform.position, Quaternion.Euler(new Vector3(0, 0, -90))) as GameObject;

        // apply the look of the card based on the info from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CardAsset = c;
        manager.ReadCardFromAsset();

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(TurnManager.Instance.whoseTurn.PArea.HandVisual.PlayPreviewSpot.position, 1f));
        s.Insert(0f, card.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(4f);
        s.OnComplete(() =>
        {
            //Command.CommandExecutionComplete();
            Destroy(card);
        });
    }


    // near death status,need to ask peach in reverse order.
    public void Die()
    {
        Debug.Log("DIE");
    }

    // Load from asset to Character
    public void LoadCharacterInfoFromAsset()
    {
        Health = CharAsset.MaxHealth;
        // change the visuals for portrait, hero power, etc...
        PArea.Portrait.charAsset = CharAsset;
        PArea.Portrait.ApplyLookFromAsset();
    }

}
