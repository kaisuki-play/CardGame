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
            switch (this.TurnPhase)
            {
                case TurnPhase.StartTurn:
                    StatusText.text = "Start Turn";
                    TurnManager.Instance.TurnPhase = TurnPhase.Judgement;
                    break;
                case TurnPhase.Judgement:
                    StatusText.text = "Judgement Phase";
                    TurnManager.Instance.EndPhase();
                    break;
                case TurnPhase.DrawCard:
                    StatusText.text = "DrawCard Phase";
                    break;
                case TurnPhase.SkipPlay:
                    StatusText.text = "PrePlay Phase";
                    break;
                case TurnPhase.PlayCard:
                    StatusText.text = "PlayCard Phase";
                    HighlightManager.EnableCardsWithType(whoseTurn);
                    break;
                case TurnPhase.DisCard:
                    StatusText.text = "DisCard Phase";
                    break;
                case TurnPhase.EndTurn:
                    StatusText.text = "End Turn Phase";
                    TurnManager.Instance.whoseTurn = TurnManager.Instance.whoseTurn.OtherPlayer;
                    break;
            }
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

        Sequence s = DOTween.Sequence();
        s.Append(Players[0].PArea.Portrait.transform.DOMove(Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[5].PArea.Portrait.transform.DOMove(Players[5].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[4].PArea.Portrait.transform.DOMove(Players[4].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[3].PArea.Portrait.transform.DOMove(Players[3].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[2].PArea.Portrait.transform.DOMove(Players[2].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[1].PArea.Portrait.transform.DOMove(Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(DrawCards);
    }

    void DrawCards()
    {
        // draw 4 cards for first player and 5 for second player
        int initDraw = 4;
        for (int i = 0; i < initDraw; i++)
        {
            for (int j = Players.Length - 1; j >= 0; j--)
            {
                Players[j].DrawACard(true);
            }
        }
        TurnManager.Instance.whoseTurn = Players[5];
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
                TurnManager.Instance.TurnPhase = TurnPhase.Judgement;
                break;
            case TurnPhase.Judgement:
                Debug.Log("***********************Judgement");
                TurnManager.Instance.TurnPhase = TurnPhase.DrawCard;
                whoseTurn.DrawACard();
                TurnManager.Instance.TurnPhase = TurnPhase.PlayCard;
                break;
            case TurnPhase.DrawCard:
                Debug.Log("***********************DrawCard");
                TurnManager.Instance.TurnPhase = TurnPhase.PlayCard;
                break;
            case TurnPhase.SkipPlay:
                Debug.Log("***********************PrePlay");
                TurnManager.Instance.TurnPhase = TurnPhase.PlayCard;
                break;
            case TurnPhase.PlayCard:
                Debug.Log("***********************PlayCard");
                TurnManager.Instance.TurnPhase = TurnPhase.DisCard;
                break;
            case TurnPhase.DisCard:
                Debug.Log("***********************Discard");
                TurnManager.Instance.TurnPhase = TurnPhase.EndTurn;
                break;
            case TurnPhase.EndTurn:
                TurnManager.Instance.whoseTurn = TurnManager.Instance.whoseTurn.OtherPlayer;
                break;
        }
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

