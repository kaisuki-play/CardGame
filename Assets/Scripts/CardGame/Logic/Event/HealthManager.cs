using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 伤害效果
    /// </summary>
    /// <param name="amount"></param>
    public async Task DamageEffect(int amount, Player player)
    {
        player.Health -= amount;
        player.PArea.Portrait.TakeDamage(amount);
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 流失体力
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="player"></param>
    /// TODO 濒死判断
    public void LooseHealth(int amount, Player player)
    {
        player.Health -= amount;
        player.PArea.Portrait.TakeDamage(amount);
    }

    /// <summary>
    /// 救治效果
    /// </summary>
    /// <param name="amount"></param>
    public void HealingEffect(int amount, Player player)
    {
        player.Health += amount;
        player.PArea.Portrait.TakeHealing(amount);
    }

    /// <summary>
    /// 治疗后的结算
    /// </summary>
    public void SettleAfterHealing()
    {
        if (DyingManager.Instance.IsInDyingInquiry)
        {
            if (DyingManager.Instance.DyingPlayer.Health >= 1)
            {
                DyingManager.Instance.Rescued();
            }
        }
        else
        {
            UseCardManager.Instance.FinishSettle();
        }
    }
}
