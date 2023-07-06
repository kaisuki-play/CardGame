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
    public async Task LooseHealth(int amount, Player player)
    {
        player.Health -= amount;
        player.PArea.Portrait.TakeDamage(amount);
        await TaskManager.Instance.DontAwait();
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
    public async void SettleAfterHealing()
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
            await UseCardManager.Instance.FinishSettle();
        }
    }
}
