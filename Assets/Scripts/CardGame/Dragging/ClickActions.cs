using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickActions : MonoBehaviour
{
    public abstract void OnCardClick();

    public virtual bool CanClick
    {
        get
        {
            return true;
        }
    }

    protected virtual Player _playerOwner
    {
        get
        {
            return GetComponentInParent<OneCardManager>().Owner;
        }
    }
}

