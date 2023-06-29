using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//事件类型
public enum EventEnum
{
    SlashNeedToPlayJink,//需要应对杀的闪
    SilverMoonNeedToPlayJink,//需要应对银月枪的闪
    NeedToPlaySlash,//需要出杀
    JiedaoSharenNeedToPlaySlash,//借刀杀人的杀
    JuedouNeedToPlaySlash,//决斗杀
    QinglongyanyuedaoNeedToPlaySlash,//青龙偃月刀追杀
}

//事件自定义传参
public class BoolTypeEventArgs : EventArgs
{
    public bool UsedAJink { get; }

    public BoolTypeEventArgs(bool usedAJink)
    {
        UsedAJink = usedAJink;
    }
}

//事件管理器
public class EventManager : MonoBehaviour
{
    /// <summary>
    /// 需要出闪的事件注册
    /// </summary>
    /// <param name="player"></param>
    /// <param name="eventEnum"></param>
    public static void RegisterNeedToPlayJinkEvent(Player player, EventEnum eventEnum)
    {
        switch (eventEnum)
        {
            case EventEnum.SlashNeedToPlayJink:
                player.NeedToPlayJinkEvent += HandleSlashNeedToPlayJinkEvent;
                break;
            case EventEnum.SilverMoonNeedToPlayJink:
                player.NeedToPlayJinkEvent += HandleSilverMoonJinkEvent;
                break;
        }
    }

    //需要应对出杀的、当然也适用于南蛮入侵，按照功能分类
    public static async Task HandleSlashNeedToPlayJinkEvent(object sender, BoolTypeEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("-----------------------------------------------触发事件 玩家:" + player.PArea.Owner);

        if (usedAJink)
        {
            UseCardManager.Instance.FinishSettle();
        }
        else
        {
            SettleManager.Instance.StartSettle();
        }
        await TaskManager.Instance.DontAwait();
    }

    //银月枪需要出闪
    public static async Task HandleSilverMoonJinkEvent(object sender, BoolTypeEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner + usedAJink);
        if (usedAJink)
        {
            TaskManager.Instance.UnBlockTask(TaskType.SilverMoonTask);
        }
        else
        {
            await LooseHealthManager.LooseHealth(player, 1);
            TaskManager.Instance.UnBlockTask(TaskType.SilverMoonTask);
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 给指定的玩家注册需要出杀事件的
    /// </summary>
    /// <param name="player"></param>
    /// <param name="eventEnum"></param>
    public static void RegisterNeedToPlaySlashEvent(Player player, EventEnum eventEnum)
    {
        switch (eventEnum)
        {
            case EventEnum.NeedToPlaySlash:
                player.NeedToPlaySlashEvent += HandleNeedToPlaySlashEvent;
                break;
            case EventEnum.JiedaoSharenNeedToPlaySlash:
                player.NeedToPlaySlashEvent += HandleJiedaosharenNeedToPlaySlashEvent;
                break;
            case EventEnum.JuedouNeedToPlaySlash:
                player.NeedToPlaySlashEvent += HandleJuedouNeedToPlaySlashEvent;
                break;
            case EventEnum.QinglongyanyuedaoNeedToPlaySlash:
                player.NeedToPlaySlashEvent += HandleQinglongyanyuedaoNeedToPlaySlashEvent;
                break;
        }
    }

    //需要应对出杀的、当然也适用于南蛮入侵，按照功能分类
    public static async Task HandleNeedToPlaySlashEvent(object sender, BoolTypeEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
        if (usedAJink)
        {
            UseCardManager.Instance.FinishSettle();
        }
        else
        {
            SettleManager.Instance.StartSettle();
        }
        await TaskManager.Instance.DontAwait();
    }

    //借刀杀人不出杀的话需要给刀
    public static async Task HandleJiedaosharenNeedToPlaySlashEvent(object sender, BoolTypeEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
        if (usedAJink)
        {
            UseCardManager.Instance.FinishSettle();
        }
        else
        {
            TipCardManager.Instance.GiveJiedaoSharenWeapon();
        }
        await TaskManager.Instance.DontAwait();
    }

    //决斗出杀
    public static async Task HandleJuedouNeedToPlaySlashEvent(object sender, BoolTypeEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("决斗 触发事件 玩家:" + player.PArea.Owner);
        if (usedAJink)
        {
            TipCardManager.Instance.PlayCardOwner = player;
            TipCardManager.Instance.ActiveTipCard();
        }
        else
        {
            SettleManager.Instance.StartSettle(null, SpellAttribute.None, player);
        }
        await TaskManager.Instance.DontAwait();
    }

    //决斗出杀
    public static async Task HandleQinglongyanyuedaoNeedToPlaySlashEvent(object sender, BoolTypeEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("决斗 触发事件 玩家:" + player.PArea.Owner);
        if (usedAJink)
        {
            TaskManager.Instance.UnBlockTask(TaskType.QinglongyanyueTask);
        }
        else
        {
            HighlightManager.DisableAllOpButtons();
            UseCardManager.Instance.BackToWhoseTurn();
            TaskManager.Instance.UnBlockTask(TaskType.QinglongyanyueTask);
        }
        await TaskManager.Instance.DontAwait();
    }

}
