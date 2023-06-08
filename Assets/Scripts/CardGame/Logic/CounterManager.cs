using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance;

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
        }
    }

    private void Awake()
    {
        Instance = this;
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
    }
}
