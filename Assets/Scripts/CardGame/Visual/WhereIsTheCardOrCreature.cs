using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhereIsTheCardOrCreature : MonoBehaviour
{

    // reference to a canvas on this object to set sorting order
    private Canvas _canvas;

    // a value for canvas sorting order when we want to show this object above everything
    private int _topSortingOrder = 500;

    // PROPERTIES
    private int _slot = -1;
    public int Slot
    {
        get { return _slot; }

        set
        {
            _slot = value;
            /*if (value != -1)
            {
                canvas.sortingOrder = HandSortingOrder(slot);
            }*/
        }
    }


    void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
    }

    public void BringToFront()
    {
        _canvas.sortingOrder = _topSortingOrder;
        _canvas.sortingLayerName = "AboveEverything";
        //canvas.transform.localPosition = new Vector3(0f, 0f, -1f);
    }

    // not setting sorting order inside of VisualStaes property because when the card is drawn, 
    // we want to set an index first and set the sorting order only when the card arrives to hand. 
    public void SetHandSortingOrder()
    {
        //if (_slot != -1)
        //    _canvas.sortingOrder = HandSortingOrder(_slot);
        _canvas.sortingLayerName = "Cards";
        //canvas.transform.localPosition = Vector3.zero;
    }

    public void SetTableSortingOrder()
    {
        _canvas.sortingOrder = 0;
        _canvas.sortingLayerName = "Creatures";
        //canvas.transform.localPosition = Vector3.zero;
    }

    private int HandSortingOrder(int placeInHand)
    {
        return (-(placeInHand + 1) * 10);
    }


}
