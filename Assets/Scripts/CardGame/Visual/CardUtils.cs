using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CardUtils : MonoBehaviour
{
    public static CardUtils Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    public void ShiftSlotsGameObjectAccordingToNumberOfCreatures(SameDistanceChildren slots, List<GameObject> cardList)
    {
        float posX;
        if (cardList.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[cardList.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    public void PlaceCreaturesOnNewSlots(SameDistanceChildren slots, List<GameObject> cardList)
    {
        foreach (GameObject g in cardList)
        {
            g.transform.DOLocalMoveX(slots.Children[cardList.IndexOf(g)].transform.localPosition.x, 0.3f);
        }
    }

    // returns an index for a new creature based on mousePosition
    // included for placing a new creature to any positon on the table
    public int TablePosForNewCreature(float MouseX, SameDistanceChildren slots, List<GameObject> cardList)
    {
        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.
        if (cardList.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[cardList.Count - 1].transform.position.x) // cursor on the left relative to all creatures on the table
            return cardList.Count;
        for (int i = 0; i < cardList.Count; i++)
        {
            if (MouseX < slots.Children[i].transform.position.x && MouseX > slots.Children[i + 1].transform.position.x)
                return i + 1;
        }
        return 0;
    }

}

