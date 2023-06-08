using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

// this class should be attached to the deck
// generates new cards and places them into the hand
public class PlayerDeckVisual : MonoBehaviour
{
    public Canvas ChildCanvas;
    public List<GameObject> DeckCards = new List<GameObject>();

}
