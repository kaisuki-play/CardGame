using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//事件类型
public enum EventEnum
{
    SlashNeedToPlayJink,
    SilverMoonNeedToPlayJink
}

//事件自定义传参
public class NeedToPlayJinkEventArgs : EventArgs
{
    public bool UsedAJink { get; }

    public NeedToPlayJinkEventArgs(bool usedAJink)
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
    public static async Task HandleSlashNeedToPlayJinkEvent(object sender, NeedToPlayJinkEventArgs e)
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
    public static async Task HandleSilverMoonJinkEvent(object sender, NeedToPlayJinkEventArgs e)
    {
        bool usedAJink = e.UsedAJink;
        Player player = (Player)sender;
        Debug.Log("触发事件 玩家:" + player.PArea.Owner);
        if (usedAJink)
        {

        }
        else
        {

        }
        await TaskManager.Instance.DontAwait();
    }

}
