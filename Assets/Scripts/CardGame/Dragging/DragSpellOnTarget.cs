﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DragSpellOnTarget : DraggingActions
{

    private TargetingOptions _targetType = TargetingOptions.AllCharacters;
    private SpriteRenderer _sr;
    private LineRenderer _lr;
    private WhereIsTheCardOrCreature _whereIsThisCard;
    private Transform _triangle;
    private SpriteRenderer _triangleSR;
    private GameObject _target;
    private OneCardManager _manager;

    private CurvedLinePoint[] _linePoints = new CurvedLinePoint[0];
    private Vector3[] _linePositions = new Vector3[0];
    private Vector3[] _linePositionsOld = new Vector3[0];

    public override bool CanDrag
    {
        get
        {
            // TEST LINE: this is just to test playing creatures before the game is complete
            //return true;

            // TODO : include full field check
            //return _manager.CanBePlayedNow;
            return true;
        }
    }

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _lr = GetComponentInChildren<LineRenderer>();
        _lr.sortingLayerName = "AboveEverything";
        _triangle = transform.Find("Triangle");
        _triangleSR = _triangle.GetComponent<SpriteRenderer>();

        _manager = GetComponentInParent<OneCardManager>();
        _whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    private void Update()
    {

    }

    public override void OnStartDrag()
    {
        _sr.enabled = true;
        _lr.enabled = true;

        _whereIsThisCard.SetHandSortingOrder();
    }

    private int _tmpTargetId = 0;

    public override void OnDraggingInUpdate()
    {
        if (_playerOwner == null)
        {
            return;
        }
        // This code only draws the arrow
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            //find curved points in children
            _linePoints = _lr.gameObject.GetComponentsInChildren<CurvedLinePoint>();

            //add positions
            Vector3 midPoint = Vector3.Lerp(transform.parent.position, transform.position - direction * 2.3f, 0.5f);
            // draw a line between the creature and the target
            _linePositions = new Vector3[_linePoints.Length];

            midPoint += new Vector3(0, 0, -_lr.positionCount * 0.1f);
            if (midPoint.z > 0)
                midPoint.z = 0;
            if (midPoint.z < -10f)
                midPoint.z = -10f;

            if (_lr.positionCount < 2)
                midPoint.z = 0;

            _linePoints[0].transform.position = transform.parent.position;
            _linePoints[1].transform.position = midPoint;
            _linePoints[2].transform.position = transform.position - direction * 1.5f;

            _linePositions[0] = _linePoints[0].transform.position;
            _linePositions[1] = _linePoints[1].transform.position;
            _linePositions[2] = _linePoints[2].transform.position;

            // Vector3 triangleSizeY = triangle.localScale;
            // triangleSizeY.y = notNormalized.magnitude * 0.04f;

            //triangle.localScale = new Vector3( triangle.localScale.x, Mathf.Clamp(triangleSizeY.y, 0.3f, 0.4f), triangle.localScale.z);

            //create old positions if they dont match
            if (_linePositionsOld.Length != _linePositions.Length)
            {
                _linePositionsOld = new Vector3[_linePositions.Length];
            }

            //get smoothed values
            Vector3[] smoothedPoints = LineSmoother.SmoothLine(_linePositions, 2);

            //set line settings
            _lr.positionCount = smoothedPoints.Length;
            _lr.SetPositions(smoothedPoints);

            _lr.enabled = true;

            // position the end of the arrow between near the target.
            _triangleSR.enabled = true;
            _triangleSR.transform.position = transform.position - 1.35f * direction;

            // proper rotarion of arrow end
            float rotZ = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            _triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            _lr.enabled = false;
            _triangleSR.enabled = false;
        }

        HandVisual PlayerHand = _playerOwner.PArea.HandVisual;
        _target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);


        bool hasTarget = false;
        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag.Contains("Player"))
            {
                // selected a Player
                hasTarget = true;
                _target = h.transform.gameObject;
            }
            else if (h.transform.tag.Contains("Creature"))
            {
                // hit a creature, save parent transform
                _target = h.transform.parent.gameObject;
            }
        }

        if (hasTarget)
        {
            if (_target != null)
            {
                // check of we should play this spell depending on targeting options
                _tmpTargetId = _target.GetComponent<IDHolder>().UniqueID;

            }
        }
        else
        {
            if (_tmpTargetId != 0)
            {
                GlobalSettings.Instance.FindPlayerByID(_tmpTargetId).PArea.Portrait.Highlighted = false;
                _tmpTargetId = 0;
            }
        }

    }


    public override async void OnEndDrag()
    {
        if (!_manager.CanBePlayedNow)
        {
            ResetTriangleTargetComponent();
            await CheckCart();
            return;
        }

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

        bool targetValid = false;

        if (_target != null)
        {
            // check of we should play this spell depending on targeting options
            int targetID = _target.GetComponent<IDHolder>().UniqueID;

            GlobalSettings.Instance.FindPlayerByID(targetID).PArea.Portrait.Highlighted = false;
            _targetType = _manager.CardAsset.Targets;
            Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~" + _targetType);
            ResetTriangleTargetComponent();
            switch (_targetType)
            {
                //TODO 加个hook哪些角色不能成为目标
                case TargetingOptions.AllCharacters:
                    if (ValidateHandCards(targetID))
                    {
                        await UseSpellCard(targetValid, targetID);
                    }
                    break;
                case TargetingOptions.EnemyCharacters:
                    if (!ValidateHandCards(targetID) || !ValidateDelayTipCards(targetID))
                    {
                        return;
                    }
                    // 有目标限制的
                    await SkillManager.AfterDragOnTarget(_manager, targetID);

                    if (targetID != _playerOwner.ID && _manager.Owner.CanAttack(targetID))
                    {
                        if (GlobalSettings.Instance.Table.HasCardOnTable(SubTypeOfCards.Jiedaosharen).Item1)
                        {
                            if (TargetsManager.Instance.SpecialTarget[0] == targetID && _manager.Owner.CanAttack(targetID))
                            {
                                await UseSpellCard(targetValid, targetID);
                            }
                        }
                        else
                        {
                            await UseSpellCard(targetValid, targetID);
                        }
                    }
                    break;
                case TargetingOptions.YourCharacters:
                    // had to check that target is not a card
                    if (targetID != _playerOwner.ID)
                    {
                        await UseSpellCard(targetValid, targetID);
                    }
                    break;
                case TargetingOptions.EnemyNoDistanceLimit:
                    if (targetID != _playerOwner.ID && ValidateDelayTipCards(targetID))
                    {
                        await UseSpellCard(targetValid, targetID);
                    }
                    break;
                default:
                    Debug.LogWarning("Reached default case in DragSpellOnTarget! Suspicious behaviour!!");
                    break;
            }
            if (!targetValid)
            {
                // not a valid target, return
                _whereIsThisCard.SetHandSortingOrder();

                // Move this card back to its slot position
                PlayerHand.PlaceCardsOnNewSlots();
            }
        }
        else
        {
            ResetTriangleTargetComponent();
            await CheckCart();
        }
    }

    public async Task UseSpellCard(bool targetValid, int targetID)
    {
        List<int> targets = new List<int>();
        targets.Add(targetID);

        await _playerOwner.DragTarget(_manager, targets);
    }

    /// <summary>
    /// 判断是否符合有手牌
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool ValidateHandCards(int targetID)
    {
        if (_manager.CardAsset.SubTypeOfCard == SubTypeOfCards.Huogong || _manager.CardAsset.SubTypeOfCard == SubTypeOfCards.Guohechaiqiao || _manager.CardAsset.SubTypeOfCard == SubTypeOfCards.Shunshouqianyang)
        {
            Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);
            if (targetPlayer.Hand.CardsInHand.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 判断是否符合延时锦囊不重复
    /// </summary>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public bool ValidateDelayTipCards(int targetID)
    {
        if (_manager.CardAsset.TypeOfCard == TypesOfCards.Tips && _manager.CardAsset.SubTypeOfTip != TypesOfTip.DelayTips)
        {
            return true;
        }
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);
        if (DelayTipManager.HasDelayTips(targetPlayer, _manager.CardAsset.SubTypeOfCard))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ResetTriangleTargetComponent()
    {
        // return target and arrow to original position
        // this position is special for spell cards to show the arrow on top
        transform.localPosition = new Vector3(0f, 0f, -1f);
        _sr.enabled = false;
        _lr.enabled = false;
        _triangleSR.enabled = false;
    }

    public async Task CheckCart()
    {
        if (TurnManager.Instance.whoseTurn == null)
        {
            return;
        }
        if (TurnManager.Instance.whoseTurn.PArea.TreasureVisual.CursorOverTreasure)
        {
            if (_manager.Owner.HasTreasure && CounterManager.Instance.UnderCartCount < CounterManager.Instance.UnderCartLimit)
            {
                Debug.Log("木流牛马的专柜");
                CounterManager.Instance.UnderCartCount++;
                //把指定的牌给木流牛马
                await _manager.Owner.GiveAssignCardToTreasure(_manager.UniqueCardID);
            }
            else
            {
                Debug.Log("你没有木流牛马");
            }
        }
        else
        {
            Debug.Log("木流牛马的专柜以外的其他地方");
        }
    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }

    public override void OnCancelDrag()
    {

    }
}