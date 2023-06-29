using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class LooseHealthManager : MonoBehaviour
{
    public static async Task LooseHealth(Player damageOriginPlayer, Player player, int looseHealth)
    {
        //结算伤害
        await HealthManager.Instance.LooseHealth(looseHealth, player);

        //是否进入濒死流程
        if (player.Health <= 0)
        {
            Debug.Log("中断流程，进入濒死流程");
            DyingManager.Instance.EnterDying(damageOriginPlayer, player);
            //阻塞不往下执行
            await TaskManager.Instance.BlockTask(TaskType.DyingTask);
        }
    }
}
