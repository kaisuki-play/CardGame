using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public List<int> CardsInHand = new List<int>();

    public void AddCard(int cardId)
    {
        CardsInHand.Add(cardId);
    }

    public void DisCard(int cardId)
    {
        CardsInHand.Remove(cardId);
    }
}
