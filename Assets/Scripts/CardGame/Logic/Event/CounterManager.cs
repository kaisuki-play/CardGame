using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance;

    //木流牛马次数
    public int UnderCartLimit = 1;
    public int UnderCartCount = 0;

    //杀的限制
    public int MaxSlashLimit = 4;

    // 当前回合杀的限制
    private int _slashLimit = 1;
    public int SlashLimit
    {
        get
        {
            return _slashLimit;
        }
        set
        {
            _slashLimit = value;
        }
    }

    // 当前回合杀的使用次数
    public int SlashCount = 0;

    // 酒的限制
    private bool _usedAnalepticThisTurn;
    public bool UsedAnalepticThisTurn
    {
        get { return _usedAnalepticThisTurn; }
        set
        {
            _usedAnalepticThisTurn = value;
            if (!_usedAnalepticThisTurn)
            {
                TurnManager.Instance.whoseTurn.ChangePortraitColor(Color.green);
            }
        }
    }

    private int _analepticWorkCount;
    public int AnalepticWorkCount
    {
        get { return _analepticWorkCount; }
        set
        {
            _analepticWorkCount = value;
            if (_analepticWorkCount > 0)
            {
                TurnManager.Instance.whoseTurn.ChangePortraitColor(Color.green);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 增加已使用杀的次数
    /// </summary>
    /// <param name="playedCard"></param>
    public void AddSlashUsedCount(OneCardManager playedCard)
    {
        if (playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.Slash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.ThunderSlash
            || playedCard.CardAsset.SubTypeOfCard == SubTypeOfCards.FireSlash)
        {
            if (playedCard.Owner.ID == TurnManager.Instance.whoseTurn.ID)
            {
                CounterManager.Instance.SlashCount++;
            }
        }
    }


    public void ResetSlashLimit()
    {
        this.MaxSlashLimit = 4;
        this.SlashLimit = 1;
        this.SlashCount = 0;
    }

    public void ResetAnaleptic()
    {
        this.UsedAnalepticThisTurn = false;
        this.AnalepticWorkCount = 0;
    }

    public void ResetUnderCart()
    {
        this.UnderCartCount = 0;
        this.UnderCartLimit = 1;
    }
}
