using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreEvaluationVisual : MonoBehaviour
{
    // a referense to a game object that marks positions where we should put new Creatures
    public SameDistanceChildren Slots;

    // list of all the creature cards on the table as GameObjects
    public List<GameObject> CardsOnScoreEvaluation = new List<GameObject>();

    // METHODS

    public async Task<int> AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        CardsOnScoreEvaluation.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(Slots.transform);

        var tcs = new TaskCompletionSource<bool>();

        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOLocalMove(Slots.Children[0].transform.localPosition, 1f));
        s.OnComplete(() =>
        {
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
            tcs.SetResult(true);
        });

        await tcs.Task;

        int sum = CardsOnScoreEvaluation.Sum(item => (int)item.GetComponent<OneCardManager>().CardAsset.CardRank);
        return sum;
    }

    /// <summary>
    /// 所有牌到弃牌堆
    /// </summary>
    /// <returns></returns>
    public async Task DisAllCards()
    {
        while (CardsOnScoreEvaluation.Count > 0)
        {
            GameObject card = CardsOnScoreEvaluation[0];
            OneCardManager cardManager = card.GetComponent<OneCardManager>();

            card.transform.SetParent(Slots.transform);

            CardsOnScoreEvaluation.RemoveAt(0);

            var tcs = new TaskCompletionSource<bool>();

            Sequence s = DOTween.Sequence();
            s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
            s.OnComplete(() =>
            {
                tcs.SetResult(true);
            });
            await tcs.Task;

            cardManager.CanBePlayedNow = false;
            card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

            //位置改为弃牌堆
            await cardManager.ChangeOwnerAndLocation(null, CardLocation.DisDeck);
        }
    }

    public async Task GetAllCardsToHand(Player player)
    {
        while (CardsOnScoreEvaluation.Count > 0)
        {
            GameObject card = CardsOnScoreEvaluation[0];
            OneCardManager cardManager = card.GetComponent<OneCardManager>();

            card.transform.SetParent(Slots.transform);

            CardsOnScoreEvaluation.RemoveAt(0);

            //cardManager.ChangeOwnerAndLocation(this, CardLocation.Hand);

            player.Hand.CardsInHand.Insert(0, cardManager.UniqueCardID);

            // 2) create a command
            await player.PArea.HandVisual.DrawACard(card);
            //var tcs = new TaskCompletionSource<bool>();

            //Sequence s = DOTween.Sequence();
            //s.Append(card.transform.DOMove(GlobalSettings.Instance.DisDeck.MainCanvas.transform.position, 1f));
            //s.OnComplete(() =>
            //{
            //    tcs.SetResult(true);
            //});
            //await tcs.Task;

            //cardManager.CanBePlayedNow = false;
            //card.transform.SetParent(GlobalSettings.Instance.DisDeck.MainCanvas.transform);

            ////位置改为弃牌堆
            //await cardManager.ChangeOwnerAndLocation(cardManager.Owner, CardLocation.DisDeck);
        }
    }

    // returns an index for a new creature based on mousePosition
    // included for placing a new creature to any positon on the table
    public int TablePosForNewCreature(float MouseX)
    {
        return CardUtils.Instance.TablePosForNewCreature(MouseX, Slots, CardsOnScoreEvaluation);
    }


    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CardsOnScoreEvaluation.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[CardsOnScoreEvaluation.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsOnScoreEvaluation)
        {
            g.transform.DOLocalMoveX(Slots.Children[CardsOnScoreEvaluation.IndexOf(g)].transform.localPosition.x, 0.3f);
        }
    }

    void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsOnScoreEvaluation.Count > 0)
            posX = (Slots.Children[0].transform.localPosition.x - Slots.Children[CardsOnScoreEvaluation.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        Slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

}
