using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System;
using static UnityEngine.GraphicsBuffer;

public class TargetsManager : MonoBehaviour
{
    public static TargetsManager Instance;
    // 正常目标
    //public List<List<int>> Targets = new List<List<int>>();
    public Dictionary<int, List<int>> TargetsDic = new Dictionary<int, List<int>>();
    // 借刀目标
    public List<int> SpecialTarget = new List<int>();
    // 已经有的目标
    public List<int> DefaultTarget = new List<int>();
    // 出闪目标
    public List<int> NeedToPlayJinkTargets = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetTargets(int cardId, List<int> targets)
    {
        TargetsDic[cardId] = targets;
        //if (Targets.Count < GlobalSettings.Instance.Table.CardsOnTable.Count)
        //{
        //    Targets.Add(targets);
        //}
        //else
        //{
        //    Targets[GlobalSettings.Instance.Table.CardsOnTable.Count - 1] = targets;
        //}
        //Debug.Log("目标数组" + Targets.Count);
        Debug.Log("目标数组" + TargetsDic.Count);
    }

    public void Order(OneCardManager oneCardManager)
    {
        //int targetIndex = GlobalSettings.Instance.Table.CardsOnTable.Count - 1;


        //List<Player> originalPlayerList = Targets[targetIndex].Select(n => GlobalSettings.Instance.FindPlayerByID(n)).ToList();
        List<Player> originalPlayerList = TargetsDic[oneCardManager.UniqueCardID].Select(n => GlobalSettings.Instance.FindPlayerByID(n)).ToList();
        originalPlayerList.Sort((x, y) => y.PArea.Owner.CompareTo(x.PArea.Owner));

        List<int> reverseOrderList = originalPlayerList.Select(n => n.ID).ToList();

        foreach (int id in reverseOrderList)
        {
            Debug.Log("排序前的位置: " + GlobalSettings.Instance.FindPlayerByID(id).PArea.Owner);
        }

        int closetPlayerIndex = FindClosestElement(reverseOrderList, oneCardManager.Owner);
        Debug.Log("离开玩家最近的" + closetPlayerIndex);
        List<int> targets = GenerateNewArray(reverseOrderList, closetPlayerIndex);
        foreach (int id in targets)
        {
            Debug.Log("排序后的位置: " + GlobalSettings.Instance.FindPlayerByID(id).PArea.Owner);
        }
        this.SetTargets(oneCardManager.UniqueCardID, targets);
    }

    public List<int> GenerateNewArray(List<int> originalList, int startIndex)
    {
        List<int> resultList = new List<int>();

        //if (startIndex == 0)
        //{
        //    for (int i = startIndex; i < originalList.Count; i++)
        //    {
        //        resultList.Add(originalList[i]);
        //    }
        //    return resultList;
        //}

        for (int i = startIndex; i < originalList.Count; i++)
        {
            resultList.Add(originalList[i]);
        }

        for (int i = 0; i < startIndex; i++)
        {
            resultList.Add(originalList[i]);
        }

        return resultList;
    }


    public int FindClosestElement(List<int> originalList, Player targetPlayer)
    {
        if (originalList.IndexOf(targetPlayer.ID) != -1)
        {
            return originalList.IndexOf(targetPlayer.ID);
        }
        List<Player> players = originalList.Select(n => GlobalSettings.Instance.FindPlayerByID(n)).ToList();
        AreaPosition closestElement = (AreaPosition)(-1);
        int closestPlayerId = -1;

        foreach (Player element in players)
        {
            if (element.PArea.Owner < targetPlayer.PArea.Owner && element.PArea.Owner > closestElement)
            {
                closestElement = element.PArea.Owner;
                closestPlayerId = element.ID;
            }
        }

        if (closestPlayerId == -1)
        {
            closestElement = targetPlayer.PArea.Owner;
            foreach (Player element in players)
            {
                if (element.PArea.Owner > closestElement)
                {
                    closestElement = element.PArea.Owner;
                    closestPlayerId = element.ID;
                }
            }
        }

        return originalList.IndexOf(closestPlayerId);
    }
}
