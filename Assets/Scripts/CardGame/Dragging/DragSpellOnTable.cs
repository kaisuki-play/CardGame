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
        if (_manager.CardAsset.SubTypeOfCard == SubTypeOfCards.Tiesuolianhuan)
        {
            (bool isDragOnPlayer, int targetPlayerId) = isDragOnTarget();
            if (isDragOnPlayer)
            {
                List<int> targets = new List<int>();
                targets.Add(targetPlayerId);
                _playerOwner.DragCard(_manager, targets);
            }
            else
            {
                if (DragSuccessful())
                {
                    //弃掉铁索 摸一张牌
                    _manager.Owner.DisACardFromHand(_manager.UniqueCardID);
                    _manager.Owner.DrawACard();
                }
                else
                {
                    OnCancelDrag();
                }
            }
        }
        else
        {
            if (DragSuccessful())
            {
                (bool hasHuogong, OneCardManager cardManager) = GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Huogong);
                if (hasHuogong)
                {
                    HandleFireAttack(cardManager, _manager);
                }
                else
                {
                    List<int> targets = new List<int>();
                    _playerOwner.DragCard(_manager, targets);
                }
            }
            else
            {
                OnCancelDrag();
            }
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

    private void HandleFireAttack(OneCardManager huogongCard, OneCardManager otherCard)
    {
        if (huogongCard.ShownCard)
        {
            otherCard.Owner.DisACardFromHand(otherCard.UniqueCardID);
            if (otherCard.CardAsset.Suits == huogongCard.ShownCardSuit)
            {
                SettleManager.Instance.StartSettle();
            }
            else
            {
                UseCardManager.Instance.FinishSettle();
            }
        }
        else
        {
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(TurnManager.Instance.whoseTurn.PArea.HandVisual.PlayPreviewSpot.position, 1f));
            s.Insert(0f, transform.DORotate(Vector3.zero, 1f));
            s.AppendInterval(2f);
            s.OnComplete(() =>
            {
                huogongCard.ShownCard = true;
                huogongCard.ShownCardSuit = otherCard.CardAsset.Suits;
                OnCancelDrag();
                TipCardManager.Instance.ActiveTipCard();
            });
        }
    }
}
