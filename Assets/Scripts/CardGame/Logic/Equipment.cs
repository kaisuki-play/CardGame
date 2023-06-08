using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public List<int> CardsInEquipment = new List<int>();

    public void AddEquipment(int cardId)
    {
        CardsInEquipment.Add(cardId);
    }

    public void RemoveEquipment(int cardId)
    {
        CardsInEquipment.Remove(cardId);
    }
}
