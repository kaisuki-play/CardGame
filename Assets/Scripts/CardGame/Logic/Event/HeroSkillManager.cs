using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

public class HeroSkillManager : MonoBehaviour
{
    /// <summary>
    /// 雅典娜技能1
    /// </summary>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveAthenaSkill1(OneCardManager playedCard)
    {
        //牌必须是锦囊牌
        if (playedCard.CardAsset.TypeOfCard != TypesOfCards.Tips && playedCard.CardAsset.TypeOfCard != TypesOfCards.DelayTips)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        if (HeroSkillRegister.SkillRegister.ContainsKey(playedCard.Owner.ID))//&& HeroSkillRegister.SkillRegister[playedCard.Owner.ID].Contains(HeroSkillType.AthenaSkill1)
        {
            TaskManager.Instance.AddATask(TaskType.AthenaSkill1);

            Player player = playedCard.Owner;

            HighlightManager.DisableAllOpButtons();
            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动技能1");
            player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                await player.DrawSomeCards(1);
                TaskManager.Instance.UnBlockTask(TaskType.AthenaSkill1);
            });

            List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
            if (canAttackTargets.Count == 0)
            {
                player.PArea.Portrait.OpButton2.enabled = false;
            }

            player.ShowOp3Button = true;
            player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp3Button2Text("不发动技能1");
            player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TaskManager.Instance.UnBlockTask(TaskType.AthenaSkill1);
            });

            await TaskManager.Instance.TaskBlockDic[TaskType.AthenaSkill1][0].Task;
        }
    }

    /// <summary>
    /// 雅典娜技能2
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public static async Task ActiveAthenaSkill2(OneCardManager playedCard, int targetID)
    {
        //牌必须是锦囊牌
        if (playedCard.CardAsset.TypeOfCard != TypesOfCards.Tips && playedCard.CardAsset.TypeOfCard != TypesOfCards.DelayTips)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        if (HeroSkillRegister.SkillRegister.ContainsKey(playedCard.Owner.ID)) //HeroSkillRegister.SkillRegister[playedCard.Owner.ID].Contains(HeroSkillType.AthenaSkill2)
        {
            TaskManager.Instance.AddATask(TaskType.AthenaSkill2);

            Player player = playedCard.Owner;

            HighlightManager.DisableAllOpButtons();
            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动技能2");
            player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                List<int> targets = new List<int>();
                targets.Add(targetID);

                await playedCard.Owner.DragTarget(playedCard, targets);
                TaskManager.Instance.ExceptionBlockTask(TaskType.AthenaSkill2, "发动技能2");
            });

            List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
            if (canAttackTargets.Count == 0)
            {
                player.PArea.Portrait.OpButton2.enabled = false;
            }

            player.ShowOp3Button = true;
            player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp3Button2Text("不发动技能2");
            player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TaskManager.Instance.UnBlockTask(TaskType.AthenaSkill2);
            });

            await TaskManager.Instance.TaskBlockDic[TaskType.AthenaSkill2][0].Task;
        }
    }

    /// <summary>
    /// Maat技能1
    /// </summary>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public static async Task ActiveMaatSkill1(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        //Player maatPlayer = null;
        //foreach (Player p in GlobalSettings.Instance.PlayerInstances)
        //{
        //    if (HeroSkillRegister.SkillRegister.ContainsKey(p.ID))//&& HeroSkillRegister.SkillRegister[p.ID].Contains(HeroSkillType.MattSkill1)
        //    {
        //        maatPlayer = p;
        //        break;
        //    }
        //}
        //if (maatPlayer == null)
        //{
        //    return;
        //}
        //牌必须是杀
        if (playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.Slash
            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.FireSlash
            && playedCard.CardAsset.SubTypeOfCard != SubTypeOfCards.ThunderSlash)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }

        TaskManager.Instance.AddATask(TaskType.MaatSkill1);

        Player player = mainPlayer;

        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Maat技能1");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            await HeroSkillManager.DiscardCardForMaatSkill1(playedCard, GlobalSettings.Instance.FindPlayerByID(targetID), player);
        });

        List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
        if (canAttackTargets.Count == 0)
        {
            player.PArea.Portrait.OpButton2.enabled = false;
        }

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Maat技能1");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.MaatSkill1);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.MaatSkill1][0].Task;
    }

    public static async Task DiscardCardForMaatSkill1(OneCardManager playedCard, Player targetPlayer, Player showCardOriginPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = TargetCardsPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 1;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            await FlopCardForMaatSkill1(playedCard, targetPlayer, showCardOriginPlayer);
        };

        for (int i = showCardOriginPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(showCardOriginPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = showCardOriginPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(showCardOriginPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        await TaskManager.Instance.DontAwait();
    }

    public static async Task FlopCardForMaatSkill1(OneCardManager playedCard, Player targetPlayer, Player showCardOriginPlayer)
    {
        OneCardManager flopedCard = await TurnManager.Instance.whoseTurn.FlopCard();
        switch (flopedCard.CardAsset.Suits)
        {
            case CardSuits.Spades:
                await LooseHealthManager.LooseHealth(showCardOriginPlayer, playedCard.Owner, 1);
                break;
            case CardSuits.Hearts:
                HealthManager.Instance.HealingEffect(1, targetPlayer);
                break;
            case CardSuits.Clubs:
                await DiscardSomeCards(playedCard.Owner, 2);
                break;
            case CardSuits.Diamonds:
                await targetPlayer.DrawSomeCards(2);
                break;
        }
        TaskManager.Instance.UnBlockTask(TaskType.MaatSkill1);
    }

    public static async Task DiscardSomeCards(Player targetPlayer, int numberOfCards)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        GlobalSettings.Instance.CardSelectVisual.PanelType = TargetCardsPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = numberOfCards;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            tcs.SetResult(true);
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = targetPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        await tcs.Task;
    }

    /// <summary>
    /// Maat技能2
    /// </summary>
    /// <param name="deadPlayerID"></param>
    /// <param name="damageOriginalPlayerID"></param>
    /// <returns></returns>
    public static async Task ActiveMaatSkill2(int damageOriginId, int targetId)
    {
        if (HeroSkillRegister.SkillRegister.ContainsKey(targetId)) //&& HeroSkillRegister.SkillRegister[targetId].Contains(HeroSkillType.MattSkill2)
        {
            HeroSkillRegister.SkillRegister.Remove(damageOriginId);
        }
        await TaskManager.Instance.DontAwait();
    }
}
