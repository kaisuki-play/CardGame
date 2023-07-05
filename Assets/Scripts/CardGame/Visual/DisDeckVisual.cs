using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisDeckVisual : MonoBehaviour
{
    public Canvas MainCanvas;
    public List<GameObject> DisDeckCards = new List<GameObject>();
    public void ChangeAllDisDeckUnOwner()
    {
        foreach (GameObject card in DisDeckCards)
        {
            OneCardManager oneCardManager = card.GetComponent<OneCardManager>();
            oneCardManager.Owner = null;
            oneCardManager.IsUseCardAssetB = false;
        }
    }
}
