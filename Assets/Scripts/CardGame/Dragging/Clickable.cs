using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Draggable;

public class Clickable : MonoBehaviour
{
    private ClickActions _ca;

    private void Awake()
    {
        _ca = GetComponent<ClickActions>();
    }
    void OnMouseDown()
    {
        if (_ca != null && _ca.CanClick)
        {
            _ca.OnCardClick();
        }
    }

    void Update()
    {

    }
}
