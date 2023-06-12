using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    public List<int> CardsInJudgement = new List<int>();

    public void AddCard(int cardId)
    {
        CardsInJudgement.Add(cardId);
    }

    public void RemoveCard(int cardId)
    {
        CardsInJudgement.Remove(cardId);
    }
}
