using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Threading.Tasks;

public class ImpeccableManager : MonoBehaviour
{
    public static ImpeccableManager Instance;
    //当前询问的对象
    public int InquireTargetId;
    //锦囊是否有效
    public bool TipWillWork = true;

    private void Awake()
    {
        Instance = this;
    }

    public async Task ClickCancelInquireImpeccable()
    {
        Player inquirePlayer = GlobalSettings.Instance.FindPlayerByID(InquireTargetId);
        //如果摁了不出无懈的当前玩家的下一个顺位玩家是当前回合人，则进入对当前目标的结算
        if (inquirePlayer.OtherPlayer.ID == TurnManager.Instance.whoseTurn.ID)
        {
            //延时锦囊的情况
            if (GlobalSettings.Instance.Table.CardsOnTable.Count == 0)
            {
                if (TipWillWork == true)
                {
                    await DelayTipManager.HandleDelayTip(TurnManager.Instance.whoseTurn);
                }
                else
                {
                    await DelayTipManager.DisDelayTip(TurnManager.Instance.whoseTurn);
                }
            }
            else
            {
                if (TipWillWork == true)
                {
                    OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
                    UseCardManager.Instance.ActiveEffect(cardManager);
                }
                else
                {
                    Debug.Log("锦囊是否生效：" + (TipWillWork ? "是" : "否"));
                    OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
                    TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID].RemoveAt(0);
                    if (TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID].Count == 0)
                    {
                        //去掉所有目标高亮
                        HighlightManager.DisableAllTargetsGlow();
                        //移除pending卡牌
                        await GlobalSettings.Instance.Table.ClearAllCardsWithNoTargets();
                        //回到当前回合人
                        UseCardManager.Instance.BackToWhoseTurn();
                    }
                    else
                    {
                        StartInquireNextTarget();
                    }

                }
            }

        }
        else
        {
            InquireNextPlayerToPlayImpeccable();
        }
    }

    // 开始询问下一个目标人
    public void StartInquireNextTarget()
    {
        //开始下一个目标的询问前，需要重置锦囊是否生效的值为True
        TipWillWork = true;
        RestartInquireTarget();
    }

    /// <summary>
    /// 从头询问
    /// </summary>
    public void RestartInquireTarget()
    {
        HighlightManager.DisableAllOpButtons();

        int curTargetPlayerId = -1;

        if (GlobalSettings.Instance.Table.CardsOnTable.Count == 0)
        {
            curTargetPlayerId = TurnManager.Instance.whoseTurn.ID;
        }
        else
        {
            OneCardManager cardManager = GlobalSettings.Instance.LastOneCardOnTable();
            curTargetPlayerId = TargetsManager.Instance.TargetsDic[cardManager.UniqueCardID][0];
        }

        //将目标高亮
        HighlightManager.DisableAllTargetsGlow();
        Player curTargetPlayer = GlobalSettings.Instance.FindPlayerByID(curTargetPlayerId);
        curTargetPlayer.ShowTargetGlow = true;

        Player inquirePlayer = TurnManager.Instance.whoseTurn;
        InquireTargetId = inquirePlayer.ID;
        ImpeccableManager.Instance.NeedToPlayImpeccable();
    }

    // 开始询问下一个人是否要无懈
    public void InquireNextPlayerToPlayImpeccable()
    {
        Player inquirePlayer = GlobalSettings.Instance.FindPlayerByID(InquireTargetId);
        this.InquireTargetId = inquirePlayer.OtherPlayer.ID;
        ImpeccableManager.Instance.NeedToPlayImpeccable();
    }

    /// <summary>
    /// 需要出无懈
    /// </summary>
    public void NeedToPlayImpeccable()
    {
        HighlightManager.DisableAllCards();

        Player InquirePlayer = GlobalSettings.Instance.FindPlayerByID(ImpeccableManager.Instance.InquireTargetId);

        HighlightManager.EnableCardWithCardType(InquirePlayer, SubTypeOfCards.Impeccable);
        InquirePlayer.ShowOp1Button = true;
        InquirePlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        InquirePlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
        {
            InquirePlayer.ShowOp1Button = false;
            await ImpeccableManager.Instance.ClickCancelInquireImpeccable();
        });
    }

    /// <summary>
    /// 需要开始无懈循环
    /// </summary>
    public void NeedToReStartInquireImpeccable()
    {
        HighlightManager.DisableAllCards();
        ImpeccableManager.Instance.InquireTargetId = TurnManager.Instance.whoseTurn.ID;

    }
}
