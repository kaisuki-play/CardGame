using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public (bool, OneCardManager) HasEquipmentWithType(Player player, TypeOfEquipment typeOfEquipment)
    {
        foreach (int cardId in player.EquipmentLogic.CardsInEquipment)
        {
            GameObject card = IDHolder.GetGameObjectWithID(cardId);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfEquipment == typeOfEquipment)
            {
                return (true, cardManager);
            }
        }
        return (false, null);
    }
}
