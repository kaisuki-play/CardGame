using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetsManager : MonoBehaviour
{
    public static TargetsManager Instance;
    public List<int> Targets = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetTargets(List<int> targets)
    {
        Targets = targets;
    }

    public void Order(OneCardManager oneCardManager)
    {
        List<int> targets = GenerateNewArray(Targets, FindClosestElement(Targets, oneCardManager.Owner));
        foreach (int id in targets)
        {
            Debug.Log("排序后的位置: " + GlobalSettings.Instance.FindPlayerByID(id).PArea.Owner);
        }
        Targets = targets;
    }

    public List<int> GenerateNewArray(List<int> originalList, int startIndex)
    {
        List<int> resultList = new List<int>();

        for (int i = startIndex; i >= 0; i--)
        {
            resultList.Add(originalList[i]);
        }

        for (int i = originalList.Count - 1; i > startIndex; i--)
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

        return originalList.IndexOf(closestPlayerId);
    }
}
