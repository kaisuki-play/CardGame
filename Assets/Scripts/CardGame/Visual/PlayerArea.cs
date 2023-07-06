using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading.Tasks;
using DG.Tweening;

public enum AreaPosition { Main, Opponent1, Opponent2, Opponent3, Opponent4, Opponent5 }

public class PlayerArea : MonoBehaviour
{
    public AreaPosition Owner;

    public HandVisual HandVisual;
    public JudgementVisual JudgementVisual;
    public EquipmentVisaul EquipmentVisaul;
    public TreasureVisual TreasureVisual;
    public HeroHeadVisual HeroHeadVisual;

    public PlayerPortraitVisual Portrait;

    public Transform PortraitPosition;
    public Transform InitialPortraitPosition;

    private void Awake()
    {

    }

    public async Task UseASpellFromADisguisedCard(OneCardManager playedCard)
    {
        CardAsset cardAsset = playedCard.CardAsset;
        GameObject card = playedCard.gameObject;

        switch (cardAsset.TypeOfCard)
        {
            case TypesOfCards.Equipment:
                {
                    card.transform.SetParent(null);

                    Player player = GlobalSettings.Instance.Players[Owner];

                    await EquipmentManager.Instance.AddOrReplaceEquipment(player, card.GetComponent<OneCardManager>());
                }
                break;
            case TypesOfCards.Base:
                {
                    var tcs = new TaskCompletionSource<bool>();

                    card.transform.SetParent(null);

                    Player player = GlobalSettings.Instance.Players[Owner];
                    int index = GlobalSettings.Instance.Table.CardsOnTable.Count;

                    Sequence s = DOTween.Sequence();
                    s.Append(card.transform.DOMove(this.HandVisual.PlayPreviewSpot.position, 1f));
                    s.Insert(0f, card.transform.DORotate(Vector3.zero, 1f));
                    s.AppendInterval(1f);
                    s.Append(card.transform.DOMove(GlobalSettings.Instance.Table.Slots.Children[index].transform.position, 1f));
                    s.OnComplete(() =>
                    {
                        tcs.SetResult(true);
                    });

                    await tcs.Task;

                    card.transform.SetParent(GlobalSettings.Instance.Table.Slots.transform);
                    GlobalSettings.Instance.Table.CardsOnTable.Add(card);

                    playedCard.CanBePlayedNow = false;
                    //改为pending状态
                    await playedCard.ChangeOwnerAndLocation(player, CardLocation.Table);
                }
                break;
        }


    }

}
