using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public List<int> CardsInTreasure = new List<int>();

    public void AddCard(int cardId)
    {
        CardsInTreasure.Add(cardId);
    }

    public void DisCard(int cardId)
    {
        CardsInTreasure.Remove(cardId);
    }
}
