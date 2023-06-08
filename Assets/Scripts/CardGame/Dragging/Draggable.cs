using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// This class enables Drag and Drop Behaviour for the game object it is attached to. 
/// It uses other script - DraggingActions to determine whether we can drag this game object now or not and 
/// whether the drop was successful or not.
/// </summary>

public class Draggable : MonoBehaviour
{

    public enum StartDragBehavior
    {
        OnMouseDown, InAwake
    }

    public enum EndDragBehavior
    {
        OnMouseUp, OnMouseDown
    }

    public StartDragBehavior HowToStart = StartDragBehavior.OnMouseDown;
    public EndDragBehavior HowToEnd = EndDragBehavior.OnMouseUp;

    // PRIVATE FIELDS

    // a flag to know if we are currently dragging this GameObject
    private bool _dragging = false;

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 _pointerDisplacement;

    // distance from camera to mouse on Z axis 
    private float _zDisplacement;

    // reference to DraggingActions script. Dragging Actions should be attached to the same GameObject.
    private DraggingActions _da;

    // STATIC property that returns the instance of Draggable that is currently being dragged
    private static Draggable _draggingThis;

    public static Draggable DraggingThis
    {
        get { return _draggingThis; }
    }

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        _da = GetComponent<DraggingActions>();
        /*if (da != null && da.CanDrag && HowToStart == StartDragBehavior.InAwake)
        {
            StartDragging();
        }*/
    }

    void OnMouseDown()
    {
        if (_da != null && _da.CanDrag && HowToStart == StartDragBehavior.OnMouseDown)
        {
            StartDragging();
        }

        if (_dragging && HowToEnd == EndDragBehavior.OnMouseDown)
        {
            _dragging = false;
            // turn all previews back on
            _draggingThis = null;
            _da.OnEndDrag();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x - _pointerDisplacement.x, mousePos.y - _pointerDisplacement.y, transform.position.z);
            _da.OnDraggingInUpdate();
        }
    }

    void OnMouseUp()
    {
        if (_dragging && HowToEnd == EndDragBehavior.OnMouseUp)
        {
            _dragging = false;
            // turn all previews back on
            _draggingThis = null;
            _da.OnEndDrag();
        }
    }

    public void StartDragging()
    {
        _dragging = true;
        // when we are dragging something, all previews should be off
        _draggingThis = this;
        _da.OnStartDrag();
        _zDisplacement = -Camera.main.transform.position.z + transform.position.z;
        _pointerDisplacement = -transform.position + MouseInWorldCoords();
    }

    public void CancelDrag()
    {
        if (_dragging)
        {
            _dragging = false;
            // turn all previews back on
            _draggingThis = null;
            _da.OnCancelDrag();
        }
    }

    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = _zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

}
