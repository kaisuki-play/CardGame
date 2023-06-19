using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class DragOnTarget : DraggingActions
{

    public OneCardManager CardManager;
    private SpriteRenderer _sr;
    private LineRenderer _lr;
    //private WhereIsTheCardOrCreature _whereIsThisCard;
    private Transform _triangle;
    private SpriteRenderer _triangleSR;
    private GameObject _target;


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

        //_whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    private void Update()
    {

    }

    public override void OnStartDrag()
    {
        _sr.enabled = true;
        _lr.enabled = true;

        //_whereIsThisCard.SetHandSortingOrder();
    }

    private int _tmpTargetId = 0;

    public override void OnDraggingInUpdate()
    {
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

    }


    public override void OnEndDrag()
    {
        HandVisual PlayerHand = TurnManager.Instance.whoseTurn.PArea.HandVisual;
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
            Debug.Log("有目标了");
            // check of we should play this spell depending on targeting options
            int targetID = _target.GetComponent<IDHolder>().UniqueID;

            GlobalSettings.Instance.FindPlayerByID(targetID).PArea.Portrait.Highlighted = false;
            if (GlobalSettings.Instance.FindPlayerByID(CardManager.TargetsPlayerIDs[0]).CanAttack(targetID))
            {
                UseSpellCard(targetValid, targetID);
            }
        }
        else
        {
            Debug.Log("无目标");
        }

        if (!targetValid)
        {
            // not a valid target, return
            //_whereIsThisCard.SetHandSortingOrder();

            // Move this card back to its slot position
            PlayerHand.PlaceCardsOnNewSlots();
        }

        transform.localPosition = new Vector3(0f, 0f, -1f);
        //_sr.enabled = false;
        _lr.enabled = false;
        _triangleSR.enabled = false;

    }

    public void UseSpellCard(bool targetValid, int targetID)
    {
        Debug.Log("使用一张牌");
        GlobalSettings.Instance.FindPlayerByID(CardManager.TargetsPlayerIDs[0]).ShowJiedaosharenTarget = false;

        TargetsSelectManager.CompleteSpecialTargetSelection(CardManager, targetID);
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