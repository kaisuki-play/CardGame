using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Print2LevelList(List<List<int>> twoDList)
    {
        for (int i = 0; i < twoDList.Count; i++)
        {
            for (int j = 0; j < twoDList[i].Count; j++)
            {
                Console.Write(twoDList[i][j] + " ");
            }
            Console.WriteLine();
        }
    }
}
