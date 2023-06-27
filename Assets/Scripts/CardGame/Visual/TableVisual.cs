using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
    public async Task ClearCards(int cardId)
    {
        int index = CardIndexOnTable(cardId);
        await ClearCardsWithIndex(index);
    }

    /// <summary>
    /// 按照索引清理牌
    /// </summary>
    /// <param name="index"></param>
    public async Task ClearCardsWithIndex(int index)
    {
        GameObject card = CardsOnTable[index];

        OneCardManager cardManager = card.GetComponent<OneCardManager>();

        TargetsManager.Instance.TargetsDic.Remove(cardManager.UniqueCardID);

        Debug.Log("CardsOnTable count : " + CardsOnTable.Count);
        Debug.Log(TargetsManager.Instance.TargetsDic.Count + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + index);

        if (cardManager.IsDisguisedCard)
        {
            foreach (int relationCardId in cardManager.RelationRealCardIds)
            {
                GameObject relationCard = IDHolder.GetGameObjectWithID(relationCardId);
                OneCardManager relationCardManager = relationCard.GetComponent<OneCardManager>();
                await relationCardManager.Owner.DisACardFromHand(relationCardId);
            }
            Destroy(card);
            CardsOnTable.RemoveAt(index);
            return;
        }

        card.transform.SetParent(null);

        var tcs = new TaskCompletionSource<bool>();
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });
        await tcs.Task;

        card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);
        cardManager.CanBePlayedNow = false;
        //到弃牌堆
        await cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
    }

    /// <summary>
    /// 清理所有没有目标的卡牌
    /// </summary>
    /// <returns></returns>
    public async Task ClearAllCardsWithNoTargets()
    {
        int i = 0;
        while (i < TargetsManager.Instance.TargetsDic.Keys.Count)
        {
            int cardId = TargetsManager.Instance.TargetsDic.Keys.ElementAt(i);
            if (TargetsManager.Instance.TargetsDic[cardId] == null || TargetsManager.Instance.TargetsDic[cardId].Count == 0)
            {
                await ClearCards(cardId);
            }
            i++;
        }
    }

    ///// <summary>
    ///// 清理第一张牌
    ///// </summary>
    //public async Task ClearCardsFromFirst()
    //{
    //    await ClearCardsWithIndex(0);
    //}

    ///// <summary>
    ///// 清理最后一张牌
    ///// </summary>
    //public async Task ClearCardsFromLast()
    //{
    //    if (CardsOnTable.Count == 0)
    //    {
    //        return;
    //    }
    //    await ClearCardsWithIndex(CardsOnTable.Count - 1);
    //}

    /// <summary>
    /// 获取最后一张牌
    /// </summary>
    /// <returns></returns>
    //public OneCardManager LastTableCard()
    //{
    //    return CardsOnTable[CardsOnTable.Count - 1].GetComponent<OneCardManager>();
    //}


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


