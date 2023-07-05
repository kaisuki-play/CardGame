using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHead : MonoBehaviour
{
    public List<int> CardsOnHero = new List<int>();

    public void AddCard(int cardId)
    {
        CardsOnHero.Add(cardId);
    }

    public void DisCard(int cardId)
    {
        CardsOnHero.Remove(cardId);
    }
}
