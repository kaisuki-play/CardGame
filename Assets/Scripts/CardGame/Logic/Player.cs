﻿using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;

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

    /// <summary>
    /// 是否已经死亡
    /// </summary>
    private bool _isDead = false;
    public bool IsDead
    {
        get
        {
            return _isDead;
        }

        set
        {
            _isDead = value;
            this.PArea.Portrait.DieImage.gameObject.SetActive(_isDead);
        }
    }

    private bool _isInIronChain = false;
    public bool IsInIronChain
    {
        get
        {
            return _isInIronChain;
        }

        set
        {
            _isInIronChain = value;
            this.PArea.Portrait.TiesuoText.gameObject.SetActive(_isInIronChain);
        }
    }

    /// <summary>
    /// 寻找下一个逆时针玩家
    /// </summary>
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
            Player curPlayer = null;
            if (currentIndex > 0)
            {
                curPlayer = GlobalSettings.Instance.PlayerInstances[currentIndex - 1];
            }
            else
            {
                curPlayer = GlobalSettings.Instance.PlayerInstances[GlobalSettings.Instance.PlayerInstances.Length - 1];
            }
            if (curPlayer.IsDead)
            {
                return curPlayer.OtherPlayer;
            }
            else
            {
                return curPlayer;
            }
        }
    }

    public Player OtherThunderPlayer
    {
        get
        {
            if (DelayTipManager.HasDelayTips(this.OtherPlayer, SubTypeOfCards.Thunder))
            {
                return this.OtherPlayer.OtherThunderPlayer;
            }
            else
            {
                return this.OtherPlayer;
            }
        }
    }

    /// <summary>
    /// 给玩家加ID
    /// </summary>
    public void TransmitInfoAboutPlayerToVisual()
    {
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
    }


    /// <summary>
    /// 计算距离，看是否可以攻击到
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool CanAttack(int targetID)
    {
        int distanceCanTouch = 1;
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);
        //TODO 武器距离
        (bool hasWeapon, OneCardManager cardManager) = EquipmentManager.Instance.HasEquipmentWithType(this, TypeOfEquipment.Weapons);
        if (hasWeapon)
        {
            distanceCanTouch = cardManager.CardAsset.WeaponAttackDistance;
        }

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

        Debug.Log("dis: " + dis + " disR: " + disR + " distanceCanTouch: " + distanceCanTouch);

        if (dis <= distanceCanTouch || disR <= distanceCanTouch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 先就+1马、-1马进行计算距离
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="originDistance"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 血量
    /// </summary>
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
        }
    }

    /// <summary>
    /// 显示第一个操作按钮
    /// </summary>
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
            PArea.Portrait.OpButton1.enabled = true;
            if (!value)
            {
                PArea.Portrait.ChangeOp1ButtonText("Cancel");
            }
        }
    }

    /// <summary>
    /// 显示第二个操作按钮
    /// </summary>
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
            PArea.Portrait.OpButton2.enabled = true;
            if (!value)
            {
                PArea.Portrait.ChangeOp2ButtonText("Cancel");
            }
        }
    }

    /// <summary>
    /// 显示第三个操作按钮
    /// </summary>
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
            PArea.Portrait.OpButton3.gameObject.SetActive(value);
            PArea.Portrait.OpButton3.enabled = true;
            if (!value)
            {
                PArea.Portrait.ChangeOp3Button2Text("Cancel");
            }
        }
    }

    /// <summary>
    /// 显示目标头像高亮
    /// </summary>
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

    /// <summary>
    /// 是否是本玩家回合，如果是则高亮头像
    /// </summary>
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

    /// <summary>
    /// 被借刀者拖拽到被杀目标头上
    /// </summary>
    private bool _showJiedaosharenTarget = false;
    public bool ShowJiedaosharenTarget
    {
        get
        {
            return _showJiedaosharenTarget;
        }
        set
        {
            _showJiedaosharenTarget = value;
            PArea.Portrait.TargetComponent.gameObject.SetActive(value);
        }
    }

    // ALL METHODS
    void Awake()
    {
        // obtain unique id from IDFactory
        PlayerID = IDFactory.GetUniqueID();
    }

    public void ChangePortraitColor(Color color)
    {
        this.PArea.Portrait.PortraitGlowImage.GetComponent<Image>().color = color;
    }


    /// <summary>
    /// 摸几张牌
    /// </summary>
    /// <param name="numberOfCards"></param>
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

    /// <summary>
    /// 摸一张牌 TODO去掉重复
    /// </summary>
    /// <param name="fast"></param>
    public void DrawACard(bool fast = false)
    {
        if (GlobalSettings.Instance.PDeck.DeckCards.Count > 0)
        {
            if (Hand.CardsInHand.Count < PArea.HandVisual.Slots.Children.Length)
            {
                GameObject card = GlobalSettings.Instance.PDeck.DeckCards[0];
                OneCardManager cardManager = card.GetComponent<OneCardManager>();

                cardManager.ChangeOwnerAndLocation(this, CardLocation.Hand);

                Hand.CardsInHand.Insert(0, cardManager.UniqueCardID);
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
    /// 摸指定的牌
    /// </summary>
    /// <param name="cardId"></param>
    public void DrawACard(int cardId)
    {
        if (GlobalSettings.Instance.PDeck.DeckCards.Count > 0)
        {
            if (Hand.CardsInHand.Count < PArea.HandVisual.Slots.Children.Length)
            {
                GameObject card = IDHolder.GetGameObjectWithID(cardId);
                OneCardManager cardManager = card.GetComponent<OneCardManager>();

                cardManager.ChangeOwnerAndLocation(this, CardLocation.Hand);

                Hand.CardsInHand.Insert(0, cardManager.UniqueCardID);
                GlobalSettings.Instance.PDeck.DeckCards.Remove(card);

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
    /// 给别人武器卡 TODO做成通用的
    /// </summary>
    /// <param name="targetPlayer"></param>
    public void GiveWeaponToTargetWithCardType(Player targetPlayer)
    {
        Debug.Log("给刀 " + this.PArea.Owner);
        Debug.Log("给刀 " + targetPlayer.PArea.Owner);
        (bool hasWeapon, OneCardManager weaponCard) = EquipmentManager.Instance.HasEquipmentWithType(this, TypeOfEquipment.Weapons);

        GiveCardToTarget(targetPlayer, weaponCard);
    }

    /// <summary>
    /// 把指定的牌给目标
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="cardManager"></param>
    public void GiveCardToTarget(Player targetPlayer, OneCardManager cardManager)
    {
        switch (cardManager.CardLocation)
        {
            case CardLocation.Judgement:
                //删除装备来源的人的卡
                this.JudgementLogic.CardsInJudgement.Remove(cardManager.UniqueCardID);
                this.PArea.JudgementVisual.RemoveCard(cardManager.gameObject);
                break;
            case CardLocation.Hand:
                this.Hand.CardsInHand.Remove(cardManager.UniqueCardID);
                this.PArea.HandVisual.RemoveCard(cardManager.gameObject);
                break;
            case CardLocation.Equipment:
                this.EquipmentLogic.CardsInEquipment.Remove(cardManager.UniqueCardID);
                this.PArea.EquipmentVisaul.RemoveCard(cardManager.gameObject);
                break;
        }
        cardManager.ChangeOwnerAndLocation(targetPlayer, CardLocation.Hand);

        //新增给目标人的卡
        targetPlayer.Hand.CardsInHand.Insert(0, cardManager.UniqueCardID);

        targetPlayer.PArea.HandVisual.GetACardFromOther(cardManager.gameObject, this);
    }

    /// <summary>
    /// 传递下一个目标
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="cardManager"></param>
    public void PassDelayTipToTarget(Player targetPlayer, OneCardManager cardManager)
    {
        GameObject card = cardManager.gameObject;
        //上家的延时锦囊去掉
        cardManager.Owner.JudgementLogic.RemoveCard(cardManager.UniqueCardID);

        card.transform.SetParent(null);

        //可视化加卡
        targetPlayer.PArea.JudgementVisual.AddCard(card);
        //逻辑加卡
        targetPlayer.JudgementLogic.AddCard(cardManager.UniqueCardID);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOLocalMove(targetPlayer.PArea.JudgementVisual.Slots.Children[0].transform.localPosition, 1f));
        s.OnComplete(() =>
        {
            cardManager.ChangeOwnerAndLocation(targetPlayer, CardLocation.Judgement);
        });
    }

    /// <summary>
    /// 拖拽目标
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public void DragTarget(OneCardManager playedCard, List<int> targets)
    {
        playedCard.isUsedCard = true;
        if (playedCard.isUsedCard)
        {
            UseACard(playedCard, targets);
        }
        else
        {
            PlayACard(playedCard, targets);
        }
    }

    /// <summary>
    /// 拖拽卡牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public void DragCard(OneCardManager playedCard, List<int> targets)
    {
        if (playedCard.CardAsset.TypeOfCard == TypesOfCards.Tips && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Impeccable
            || (playedCard.CardAsset.TypeOfCard == TypesOfCards.Equipment)
            || (playedCard.CardAsset.TypeOfCard == TypesOfCards.DelayTips
            || (playedCard.CardAsset.TypeOfCard == TypesOfCards.Base && (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Peach || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Analeptic))))
        {
            playedCard.isUsedCard = true;
        }
        else
        {
            playedCard.isUsedCard = false;
        }
        if (playedCard.isUsedCard)
        {
            UseACard(playedCard, targets);
        }
        else
        {
            PlayACard(playedCard, targets);
        }
    }

    /// <summary>
    /// 使用牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public void UseACard(OneCardManager playedCard, List<int> targets)
    {
        if (playedCard.CardAsset.TypeOfCard == TypesOfCards.DelayTips)
        {
            DelayTipManager.UseADelayTipsCardFromHand(playedCard, targets);
        }
        else
        {
            UseCardManager.Instance.UseAVisualCardFromHand(playedCard, targets);
        }
    }

    /// <summary>
    /// 打出牌
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targets"></param>
    public void PlayACard(OneCardManager playedCard, List<int> targets)
    {
        PlayCardManager.Instance.PlayAVisualCardFromHand(playedCard, targets);
    }

    /// <summary>
    /// 根据cardId弃一张手牌
    /// </summary>
    /// <param name="cardId"></param>
    public void DisACardFromHand(int cardId)
    {
        this.Hand.DisCard(cardId);
        this.PArea.HandVisual.DisCardFromHand(cardId);
    }

    /// <summary>
    /// 根据cardId弃一张判定区的牌
    /// </summary>
    /// <param name="cardId"></param>
    public void DisACardFromJudgement(int cardId)
    {
        this.JudgementLogic.RemoveCard(cardId);
        this.PArea.JudgementVisual.DisCardFromJudgement(cardId);
    }

    /// <summary>
    /// 所有牌都弃掉
    /// </summary>
    public void DisAllCards()
    {
        foreach (int cardId in this.Hand.CardsInHand)
        {
            this.PArea.HandVisual.DisCardFromHand(cardId);
        }
        foreach (int cardId in this.JudgementLogic.CardsInJudgement)
        {
            this.PArea.JudgementVisual.DisCardFromJudgement(cardId);
        }
        foreach (int cardId in this.EquipmentLogic.CardsInEquipment)
        {
            this.PArea.EquipmentVisaul.DisCardFromEquipment(cardId);
        }
        this.Hand.CardsInHand.Clear();
        this.EquipmentLogic.CardsInEquipment.Clear();
        this.JudgementLogic.CardsInJudgement.Clear();
    }


    /// <summary>
    /// 翻卡牌
    /// </summary>
    /// <returns></returns>
    public async Task<OneCardManager> FlopCard()
    {
        GameObject card = GlobalSettings.Instance.PDeck.DeckCards[0];
        OneCardManager cardManager = card.GetComponent<OneCardManager>();

        //从牌堆中弃掉
        GlobalSettings.Instance.PDeck.DeckCards.Remove(card);

        OneCardManager returnValue = await WaitForSequenceCoroutine(cardManager);
        Debug.Log("返回翻牌的卡牌");
        return returnValue;
    }

    /// <summary>
    /// 等待翻卡牌结束
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private async Task<OneCardManager> WaitForSequenceCoroutine(OneCardManager card)
    {
        TaskCompletionSource<OneCardManager> tcs = new TaskCompletionSource<OneCardManager>();

        Sequence s = DOTween.Sequence();
        Sequence s1 = DOTween.Sequence();
        s.Append(card.transform.DOMove(TurnManager.Instance.whoseTurn.PArea.HandVisual.PlayPreviewSpot.position, 1f));
        s.Insert(0f, card.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(1f);
        s.OnComplete(() =>
        {
            card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

            Sequence s1 = DOTween.Sequence();
            s1.AppendInterval(0.1f);
            s1.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));

            s1.OnComplete(() =>
            {
                card.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
                Debug.Log("s1 完成");
                tcs.SetResult(card);
            });
        });

        OneCardManager returnValue = await tcs.Task;

        return returnValue;

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
