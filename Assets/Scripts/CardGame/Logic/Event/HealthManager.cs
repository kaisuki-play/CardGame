using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void DamageEffect(int amount, Player player)
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
