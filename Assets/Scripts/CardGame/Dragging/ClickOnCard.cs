using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnCard : ClickActions
{
    private int _savedHandSlot;
    private WhereIsTheCardOrCreature _whereIsCard;
    private OneCardManager _manager;

    void Awake()
    {
        _whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        _manager = GetComponent<OneCardManager>();
    }

    public override bool CanClick
    {
        get
        {
            return true;
        }
    }

    private void Update()
    {

    }

    public override void OnCardClick()
    {
        Debug.Log("On Card Click");
    }
}
