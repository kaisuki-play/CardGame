using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour
{

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public abstract void OnCancelDrag();

    public virtual bool CanDrag
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

    protected abstract bool DragSuccessful();
}
