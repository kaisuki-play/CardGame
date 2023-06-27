using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//事件类型
public enum EventEnum
{
    SlashNeedToPlayJink,
    SilverMoonNeedToPlayJink,
    NeedToPlaySlash,
    JiedaoSharenNeedToPlaySlash
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
    //给指定的玩家注册事件管理器
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

}
