using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    /// <summary>
    /// 清理指定牌
    /// </summary>
    /// <param name="cardId"></param>
    public void ClearCards(int cardId)
    {
        int index = CardIndexOnTable(cardId);
        ClearCardsWithIndex(index);
    }

    /// <summary>
    /// 按照索引清理牌
    /// </summary>
    /// <param name="index"></param>
    public void ClearCardsWithIndex(int index)
    {
        GameObject card = CardsOnTable[index];

        TargetsManager.Instance.Targets.RemoveAt(index);
        Debug.Log("结算完后的牌" + TargetsManager.Instance.Targets.Count);

        OneCardManager cardManager = card.GetComponent<OneCardManager>();
        if (cardManager.IsDisguisedCard)
        {
            foreach (int relationCardId in cardManager.RelationRealCardIds)
            {
                GameObject relationCard = IDHolder.GetGameObjectWithID(relationCardId);
                OneCardManager relationCardManager = relationCard.GetComponent<OneCardManager>();
                relationCardManager.Owner.DisACardFromHand(relationCardId);
            }
            Destroy(card);
            CardsOnTable.RemoveAt(index);
            return;
        }

        card.transform.SetParent(null);
        cardManager.CanBePlayedNow = false;
        cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);
        });
    }

    /// <summary>
    /// 清理第一张牌
    /// </summary>
    public void ClearCardsFromFirst()
    {
        ClearCardsWithIndex(0);
    }

    /// <summary>
    /// 清理最后一张牌
    /// </summary>
    public void ClearCardsFromLast()
    {
        if (CardsOnTable.Count == 0)
        {
            return;
        }
        ClearCardsWithIndex(CardsOnTable.Count - 1);
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

    /// <summary>
    /// pending状态下是否有某种类型的牌
    /// </summary>
    /// <param name="cardType"></param>
    /// <returns></returns>
    public (bool, OneCardManager) HasCardOnTable(SubTypeOfCards cardType)
    {
        foreach (GameObject gameObject in GlobalSettings.Instance.Table.CardsOnTable)
        {
            OneCardManager cardManager = gameObject.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.SubTypeOfCard == cardType)
            {
                return (true, cardManager);
            }
        }
        return (false, null);
    }

    /// <summary>
    /// 一张牌在pending状态下的索引
    /// </summary>
    /// <param name="cardId"></param>
    /// <returns></returns>
    public int CardIndexOnTable(int cardId)
    {
        int index = -1;
        for (int i = 0; i < CardsOnTable.Count; i++)
        {
            GameObject tmpcard = CardsOnTable[i];
            OneCardManager tmpcardManager = tmpcard.GetComponent<OneCardManager>();
            if (tmpcardManager.UniqueCardID == cardId)
            {
                index = i;
                break;
            }
        }
        return index;
    }
}


