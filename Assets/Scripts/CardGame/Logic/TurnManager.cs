using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public enum TurnPhase
{
    SkipTurn,//是否跳过回合
    StartTurn,//回合开始
    SkipJudgement,//是否跳过判定
    StartJudgement,//没有特殊技能，什么都不干
    Judgement,
    EndJudgement,
    SkipDrawCard,
    StartDrawCard,
    DrawCard,
    EndDrawCard,
    SkipPlay,
    StartPlay,
    PlayCard,
    EndPlayCard,
    SkipDisCard,
    StartDisCard,
    DisCard,
    EndDisCard,
    EndTurn
}

// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour
{

    // PUBLIC FIELDS
    public Text StatusText;

    public Text MessageText;

    // for Singleton Pattern
    public static TurnManager Instance;

    public Player[] Players;

    /// <summary>
    /// 是否跳过各个阶段
    /// </summary>
    public bool SkipJudgementPhase = false;
    public bool SkipDrawCardPhase = false;
    public bool SkipPlayCardPhase = false;
    public bool SkipDisCardPhase = false;

    public bool IsInactiveStatus = true;

    public int DrawCardLimitPerTurn = 1;

    // Record the phases in per turn
    private TurnPhase _turnPhase;
    public TurnPhase TurnPhase
    {
        get
        {
            return _turnPhase;
        }

        set
        {
            _turnPhase = value;
            HandleTurnPhase();
        }
    }

    //处理回合内小阶段的钩子
    public async void HandleTurnPhase()
    {
        switch (this.TurnPhase)
        {
            case TurnPhase.StartTurn:
                StatusText.text = "Start Turn";
                Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~回合开始");
                await SkillManager.TurnStart();
                TurnManager.Instance.TurnPhase = TurnPhase.SkipJudgement;
                break;
            ///是否跳过判定阶段
            case TurnPhase.SkipJudgement:
                if (this.SkipJudgementPhase)
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.SkipDrawCard;
                }
                else
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.StartJudgement;
                }
                break;
            case TurnPhase.StartJudgement:
                //TODO 技能Hook
                TurnManager.Instance.TurnPhase = TurnPhase.Judgement;
                break;
            case TurnPhase.Judgement:
                StatusText.text = "Judgement Phase";
                if (DelayTipManager.HasDelayTips(whoseTurn))
                {
                    //DelayTipManager.HandleDelayTip(whoseTurn);
                    ImpeccableManager.Instance.StartInquireNextTarget();
                    await TaskManager.Instance.BlockTask(TaskType.DelayTask);
                }
                if (whoseTurn.IsDead)
                {
                    OnEndTurn();
                }
                else
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.EndJudgement;
                }
                break;
            case TurnPhase.EndJudgement:
                //TODO 技能Hook
                TurnManager.Instance.TurnPhase = TurnPhase.SkipDrawCard;
                break;
            ///是否跳过发牌阶段
            case TurnPhase.SkipDrawCard:
                if (this.SkipDrawCardPhase)
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.SkipPlay;
                }
                else
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.StartDrawCard;
                }
                break;
            case TurnPhase.StartDrawCard:
                //摸牌阶段开始hook
                await SkillManager.DrawCardPhaseStart();
                TurnManager.Instance.TurnPhase = TurnPhase.DrawCard;
                break;
            case TurnPhase.DrawCard:
                StatusText.text = "DrawCard Phase";
                await whoseTurn.DrawSomeCards(this.DrawCardLimitPerTurn);
                TurnManager.Instance.TurnPhase = TurnPhase.EndDrawCard;
                break;
            case TurnPhase.EndDrawCard:
                //TODO 技能Hook
                TurnManager.Instance.TurnPhase = TurnPhase.SkipPlay;
                break;
            ///是否跳过出牌阶段
            case TurnPhase.SkipPlay:
                if (this.SkipPlayCardPhase)
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.SkipDisCard;
                }
                else
                {
                    TurnManager.Instance.TurnPhase = TurnPhase.StartPlay;
                }
                break;
            case TurnPhase.StartPlay:
                //TODO 技能Hook
                await SkillManager.StartPlayPhase();
                TurnManager.Instance.TurnPhase = TurnPhase.PlayCard;
                break;
            case TurnPhase.PlayCard:
                StatusText.text = "PlayCard Phase";
                await HeroSkillRegister.PriorityHeroSkill(HeroSkillActivePhase.Hook27);
                //TODO临时的，需要去掉的
                //if (whoseTurn.CharAsset.PlayerWarrior == PlayerWarrior.Fenrir)
                //{
                //    foreach (int cardId in whoseTurn.Hand.CardsInHand)
                //    {
                //        GameObject card = IDHolder.GetGameObjectWithID(cardId);
                //        OneCardManager cardManager = card.GetComponent<OneCardManager>();
                //        if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips || cardManager.CardAsset.TypeOfCard == TypesOfCards.DelayTips)
                //        {
                //            cardManager.LaunchCardB(GlobalSettings.Instance.PDeck.CardAssetBWithType(SubTypeOfCards.Slash, cardManager.CardAsset));
                //        }
                //    }
                //}
                HighlightManager.EnableCardsWithType(whoseTurn);
                break;
            case TurnPhase.EndPlayCard:
                //TODO 技能Hook
                await HeroSkillRegister.PriorityHeroSkill(HeroSkillActivePhase.Hook28);
                TurnManager.Instance.TurnPhase = TurnPhase.SkipDisCard;
                break;
            ///是否跳过弃牌阶段
            case TurnPhase.SkipDisCard:
                if (this.SkipDisCardPhase)
                {
                    Debug.Log("跳过弃牌阶段");
                    TurnManager.Instance.TurnPhase = TurnPhase.EndTurn;
                }
                else
                {
                    Debug.Log("不跳过弃牌阶段");
                    TurnManager.Instance.TurnPhase = TurnPhase.StartDisCard;
                }
                break;
            case TurnPhase.StartDisCard:
                //TODO 技能Hook
                TurnManager.Instance.TurnPhase = TurnPhase.DisCard;
                break;
            case TurnPhase.DisCard:
                StatusText.text = "DisCard Phase";
                break;
            case TurnPhase.EndDisCard:
                //TODO 技能Hook
                TurnManager.Instance.TurnPhase = TurnPhase.EndTurn;
                break;
            case TurnPhase.EndTurn:
                StatusText.text = "End Turn Phase";
                await HeroSkillRegister.PriorityHeroSkill(HeroSkillActivePhase.Hook31);
                OnEndTurn();
                break;
        }
    }

    // PROPERTIES
    private Player _whoseTurn;
    public Player whoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;
            TurnManager.Instance.whoseTurn.HasTreasure = EquipmentManager.Instance.HasEquipmentWithType(TurnManager.Instance.whoseTurn, TypeOfEquipment.Treasure).Item1;
            // 去掉所有的头像高亮
            HighlightManager.DisableAllTurnGlow();
            // 设置回合人头像高亮
            _whoseTurn.IsThisTurn = true;
            // 设置为玩家的开始阶段
            TurnPhase = TurnPhase.StartTurn;
        }
    }


    // METHODS
    void Awake()
    {
        Instance = this;
        Players = GlobalSettings.Instance.PlayerInstances;
    }

    // game is start
    void Start()
    {
        OnGameStart();
    }

    public void OnGameStart()
    {
        Debug.Log("游戏开始");
        foreach (Player p in Players)
        {
            p.LoadCharacterInfoFromAsset();
            p.TransmitInfoAboutPlayerToVisual();
            // move both portraits to the center
            p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.position;
        }

        HeroSkillRegister.HandlePlayersSkill();

        Sequence s = DOTween.Sequence();
        s.Append(Players[0].PArea.Portrait.transform.DOMove(Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[5].PArea.Portrait.transform.DOMove(Players[5].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[4].PArea.Portrait.transform.DOMove(Players[4].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[3].PArea.Portrait.transform.DOMove(Players[3].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[2].PArea.Portrait.transform.DOMove(Players[2].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[1].PArea.Portrait.transform.DOMove(Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(GlobalSettings.Instance.UseGoldenFinger ? GoldenFinger : DrawCards);
    }

    async void DrawCards()
    {
        // draw 4 cards for first player and 5 for second player
        int initDraw = 4;
        for (int i = 0; i < initDraw; i++)
        {
            for (int j = Players.Length - 1; j >= 0; j--)
            {
                await Players[j].DrawACard(true);
            }
        }

        TurnManager.Instance.whoseTurn = Players[5];

        //for (int j = Players.Length - 1; j >= 0; j--)
        //{
        //    Players[j].DrawCardsForTreasure(5);
        //}

    }

    void GoldenFinger()
    {
        GlobalSettings.Instance.GoldenFingerVisual.curDrawCardPlayer = GlobalSettings.Instance.PlayerInstances[Players.Length - 1];
        GlobalSettings.Instance.GoldenFingerVisual.Show();
        GlobalSettings.Instance.GoldenFingerVisual.Completion = () =>
        {
            TurnManager.Instance.whoseTurn = Players[5];
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndPhase();
    }

    // do something at each phase end.
    public void EndPhase()
    {
        // restart timer
        RestartTimer();
        switch (this.TurnPhase)
        {
            case TurnPhase.StartTurn:
                TurnManager.Instance.TurnPhase = TurnPhase.SkipJudgement;
                break;
            case TurnPhase.StartJudgement:
                TurnManager.Instance.TurnPhase = TurnPhase.Judgement;
                break;
            case TurnPhase.Judgement:
                Debug.Log("***********************Judgement");
                TurnManager.Instance.TurnPhase = TurnPhase.EndJudgement;
                break;
            case TurnPhase.EndJudgement:
                TurnManager.Instance.TurnPhase = TurnPhase.SkipDrawCard;
                break;
            case TurnPhase.StartDrawCard:
                break;
            case TurnPhase.DrawCard:
                Debug.Log("***********************DrawCard");
                TurnManager.Instance.TurnPhase = TurnPhase.PlayCard;
                break;
            case TurnPhase.EndDrawCard:
                TurnManager.Instance.TurnPhase = TurnPhase.SkipPlay;
                break;
            case TurnPhase.StartPlay:
                TurnManager.Instance.TurnPhase = TurnPhase.PlayCard;
                break;
            case TurnPhase.PlayCard:
                Debug.Log("***********************PlayCard");
                TurnManager.Instance.TurnPhase = TurnPhase.EndPlayCard;
                break;
            case TurnPhase.EndPlayCard:
                TurnManager.Instance.TurnPhase = TurnPhase.SkipDisCard;
                break;
            case TurnPhase.StartDisCard:
                TurnManager.Instance.TurnPhase = TurnPhase.DisCard;
                break;
            case TurnPhase.DisCard:
                Debug.Log("***********************Discard");
                TurnManager.Instance.TurnPhase = TurnPhase.EndDisCard;
                break;
            case TurnPhase.EndDisCard:
                TurnManager.Instance.TurnPhase = TurnPhase.EndTurn;
                break;
            case TurnPhase.EndTurn:
                OnEndTurn();
                break;
        }
    }

    public void OnEndTurn()
    {
        //重置木流牛马闲置
        CounterManager.Instance.ResetUnderCart();
        //重置杀的次数限制
        CounterManager.Instance.ResetSlashLimit();
        //重置酒的次数限制
        CounterManager.Instance.ResetAnaleptic();
        //重置各种跳过阶段的参数
        SkipJudgementPhase = false;
        SkipDrawCardPhase = false;
        SkipPlayCardPhase = false;
        SkipDisCardPhase = false;
        //所有卡牌高亮去掉、按钮变灰
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        HighlightManager.DisableAllTargetsGlow();
        //重置每轮摸牌数量
        DrawCardLimitPerTurn = 1;

        HeroSkillState.ClearOnceValue();

        TurnManager.Instance.whoseTurn = TurnManager.Instance.whoseTurn.OtherPlayer;
    }



    // wait destination time duration.
    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    // stop timer
    public void StopTheTimer()
    {

    }

    // FOR Restart timer
    public void RestartTimer()
    {

    }

}

