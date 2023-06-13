using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TableVisual : MonoBehaviour
{
    // a referense to a game object that marks positions where we should put new Creatures
    public SameDistanceChildren Slots;

    // list of all the creature cards on the table as GameObjects
    public List<GameObject> CardsOnTable = new List<GameObject>();

    // are we hovering over this table`s collider with a mouse
    private bool _cursorOverThisTable = false;

    // A 3D collider attached to this game object
    private BoxCollider _col;

    // PROPERTIES

    // returns true if we are hovering over any player`s table collider
    public static bool CursorOverSomeTable
    {
        get
        {
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            if (bothTables.Length > 0)
            {
                return (bothTables[0].CursorOverThisTable);
            }
            else
            {
                return false;
            }

        }
    }

    // returns true only if we are hovering over this table`s collider
    public bool CursorOverThisTable
    {
        get { return _cursorOverThisTable; }
    }

    // METHODS

    // MONOBEHAVIOUR METHODS (mouse over collider detection)
    void Awake()
    {
        _col = GetComponent<BoxCollider>();
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        // create an array of RaycastHits
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == _col)
                passedThroughTableCollider = true;
        }
        _cursorOverThisTable = passedThroughTableCollider;
    }

    public void ClearCards()
    {
        GameObject card = CardsOnTable[0];
        card.transform.SetParent(null);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            //Command.CommandExecutionComplete();
            //Destroy(card);
            card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            cardManager.CanBePlayedNow = false;
            cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
        });
        //foreach (GameObject card in CardsOnTable)
        //{
        //    card.transform.SetParent(null);

        //    Sequence s = DOTween.Sequence();
        //    s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        //    s.OnComplete(() =>
        //    {
        //        //Command.CommandExecutionComplete();
        //        //Destroy(card);
        //        card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

        //        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        //        cardManager.CanBePlayedNow = false;
        //        cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
        //    });
        //}
    }


    // method to create a new creature and add it to the table
    public void AddCardsAtIndex(OneCardManager card, int index)
    {
        // TODO 将pending状态的牌添加到桌面
    }


    // returns an index for a new creature based on mousePosition
    // included for placing a new creature to any positon on the table
    public int TablePosForNewCreature(float MouseX)
    {
        return CardUtils.Instance.TablePosForNewCreature(MouseX, Slots, CardsOnTable);
    }

    // Destroy a card
    public void RemoveCardWithID(int IDToRemove)
    {
        // TODO 从数组中移除 然后重新排列
        //_cardsOnTable.Remove(creatureToRemove);


        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        Command.CommandExecutionComplete();
    }


    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CardsOnTable.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[CardsOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    void PlaceCreaturesOnNewSlots()
    {
        foreach (GameObject g in CardsOnTable)
        {
            g.transform.DOLocalMoveX(Slots.Children[CardsOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
        }
    }
}


