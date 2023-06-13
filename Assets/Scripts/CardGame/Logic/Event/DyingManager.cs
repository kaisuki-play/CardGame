using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DyingInquirePhase
{
    NonMedicalSkill,
    MedicalSkill,
    InquiryPeach
}

public class DyingManager : MonoBehaviour
{
    public static DyingManager Instance;
    //是否有玩家濒死
    public bool IsInDyingInquiry = false;
    //当前濒死玩家
    public Player DyingPlayer;
    //当前询问的对象
    public int InquireTargetId;
    //当前濒死阶段记录
    public DyingInquirePhase DyingInquirePhase;
    private void Awake()
    {
        Instance = this;
    }

    public void EnterDying(Player dyingPlayer)
    {
        Debug.Log("进入濒死状态");
        DyingManager.Instance.IsInDyingInquiry = true;
        DyingManager.Instance.DyingPlayer = dyingPlayer;
        DyingManager.Instance.InquireTargetId = TurnManager.Instance.whoseTurn.ID;
        InquiryNonMedicalSkills();
    }

    public void ClickCancel()
    {
        Player inquirePlayer = GlobalSettings.Instance.FindPlayerByID(InquireTargetId);
        //如果摁了不出无懈的当前玩家的下一个顺位玩家是当前回合人，则进入对当前目标的结算
        if (inquirePlayer.OtherPlayer.ID == TurnManager.Instance.whoseTurn.ID)
        {
            DyingManager.Instance.InquireTargetId = TurnManager.Instance.whoseTurn.ID;
            switch (DyingInquirePhase)
            {
                case DyingInquirePhase.NonMedicalSkill:
                    DyingManager.Instance.InquiryMedicalSkills();
                    break;
                case DyingInquirePhase.MedicalSkill:
                    DyingManager.Instance.InquiryPeachs();
                    break;
                case DyingInquirePhase.InquiryPeach:
                    Debug.Log("Real Die");
                    DyingManager.Instance.RealDie();
                    break;
            }
        }
        else
        {
            InquireNextPlayer();
        }
    }

    public void InquiryNonMedicalSkills()
    {
        DyingInquirePhase = DyingInquirePhase.NonMedicalSkill;

        HighlightManager.DisableAllCards();

        Player InquirePlayer = GlobalSettings.Instance.FindPlayerByID(DyingManager.Instance.InquireTargetId);

        // TODO 玩家非救治技能高亮

        InquirePlayer.ShowOp1Button = true;
        InquirePlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        InquirePlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            InquirePlayer.ShowOp1Button = false;
            DyingManager.Instance.ClickCancel();
        });
    }
    public void InquiryMedicalSkills()
    {
        DyingInquirePhase = DyingInquirePhase.MedicalSkill;

        HighlightManager.DisableAllCards();

        Player InquirePlayer = GlobalSettings.Instance.FindPlayerByID(DyingManager.Instance.InquireTargetId);

        // TODO 玩家救治技能高亮

        InquirePlayer.ShowOp1Button = true;
        InquirePlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        InquirePlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            InquirePlayer.ShowOp1Button = false;
            DyingManager.Instance.ClickCancel();
        });
    }

    public void InquiryPeachs()
    {
        DyingInquirePhase = DyingInquirePhase.InquiryPeach;

        HighlightManager.DisableAllCards();

        Player InquirePlayer = GlobalSettings.Instance.FindPlayerByID(DyingManager.Instance.InquireTargetId);

        // 求桃高亮、例如急救也是在这里高亮
        HighlightManager.EnableCardWithCardType(InquirePlayer, SubTypeOfCards.Peach);

        InquirePlayer.ShowOp1Button = true;
        InquirePlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        InquirePlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            InquirePlayer.ShowOp1Button = false;
            DyingManager.Instance.ClickCancel();
        });
    }

    // 开始询问下一个人是否要无懈
    public void InquireNextPlayer()
    {
        Player inquirePlayer = GlobalSettings.Instance.FindPlayerByID(InquireTargetId);
        this.InquireTargetId = inquirePlayer.OtherPlayer.ID;
        switch (DyingInquirePhase)
        {
            case DyingInquirePhase.NonMedicalSkill:
                DyingManager.Instance.InquiryNonMedicalSkills();
                break;
            case DyingInquirePhase.MedicalSkill:
                DyingManager.Instance.InquiryMedicalSkills();
                break;
            case DyingInquirePhase.InquiryPeach:
                DyingManager.Instance.InquiryPeachs();
                break;
        }

    }

    public void Healing()
    {
        HealthManager.Instance.HealingEffect(1, this.DyingPlayer);
        if (this.DyingPlayer.Health >= 1)
        {
            Rescued();
        }
    }

    public void Rescued()
    {
        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        //清空濒死玩家
        DyingManager.Instance.DyingPlayer = null;
        //重置濒死状态
        DyingManager.Instance.IsInDyingInquiry = false;
        //进入伤害后的流程
        SettleManager.Instance.AfterDamage();
    }

    public void RealDie()
    {
        //设置濒死人为死亡
        DyingManager.Instance.DyingPlayer.IsDead = true;
        //全局移除玩家
        GlobalSettings.Instance.RemoveDiePlayer();
        //TODO 玩家所有牌进入弃牌堆
        DyingManager.Instance.DyingPlayer.DisAllCards();
        //清空濒死玩家
        DyingManager.Instance.DyingPlayer = null;
        //重置濒死状态
        DyingManager.Instance.IsInDyingInquiry = false;
        //进入铁索流程
        SettleManager.Instance.HandleIronChain();
    }
}
