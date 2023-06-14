using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DragSpellOnTable : DraggingActions
{

    private int _savedHandSlot;
    private WhereIsTheCardOrCreature _whereIsCard;
    private OneCardManager _manager;
    private GameObject _target;

    public override bool CanDrag
    {
        get
        {
            return _manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        _whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        _manager = GetComponent<OneCardManager>();
    }

    private void Update()
    {

    }

    public override void OnStartDrag()
    {
        _savedHandSlot = _whereIsCard.Slot;
    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        if (DragSuccessful())
        {
            List<int> targets = new List<int>();
            _playerOwner.DragCard(_manager, targets);
        }
        else
        {
            OnCancelDrag();
        }
    }

    public override void OnCancelDrag()
    {
        // Set old sorting order 
        _whereIsCard.Slot = _savedHandSlot;
        // Move this card back to its slot position
        HandVisual playerHand = _playerOwner.PArea.HandVisual;
        Vector3 oldCardPos = playerHand.Slots.Children[_savedHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 1f);
    }

    protected override bool DragSuccessful()
    {
        return TableVisual.CursorOverSomeTable;
    }

    private (bool, int) isDragOnTarget()
    {
        HandVisual PlayerHand = _playerOwner.PArea.HandVisual;
        _target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);
        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag.Contains("Player"))
            {
                // selected a Player
                _target = h.transform.gameObject;
            }
        }
        if (_target != null)
        {
            return (true, _target.GetComponent<IDHolder>().UniqueID);
        }
        else
        {
            return (false, -1);
        }
    }
}
