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
    public static async Task ActiveAthenaSkill1(Player mainPlayer, OneCardManager playedCard)
    {
        //牌必须是锦囊牌
        if (playedCard.CardAsset.TypeOfCard != TypesOfCards.Tips && playedCard.CardAsset.TypeOfCard != TypesOfCards.DelayTips)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        if (mainPlayer.ID != playedCard.Owner.ID)
        {
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
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
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

        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
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

    /// <summary>
    /// Anubis技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public static async Task ActiveAnubisSkill1(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        if (targetID == mainPlayer.ID)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.AnubisSkill1);

        Player player = mainPlayer;

        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Anubis技能");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            await HeroSkillManager.DiscardCardForAnubisSkill1(playedCard, GlobalSettings.Instance.FindPlayerByID(targetID), player);
        });

        List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
        if (canAttackTargets.Count == 0)
        {
            player.PArea.Portrait.OpButton2.enabled = false;
        }

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Anubis技能");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.AnubisSkill1);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.AnubisSkill1][0].Task;
    }

    public static async Task DiscardCardForAnubisSkill1(OneCardManager playedCard, Player targetPlayer, Player showCardOriginPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 1;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
            GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = null;
            await FlopCardForAnubisSkill1(playedCard, targetPlayer, showCardOriginPlayer);
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

    public static async Task FlopCardForAnubisSkill1(OneCardManager playedCard, Player targetPlayer, Player showCardOriginPlayer)
    {
        int randSum = await TurnManager.Instance.whoseTurn.ScoreEvaluationCard();
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + randSum);
        if (randSum <= 21)
        {
            HighlightManager.DisableAllOpButtons();
            targetPlayer.ShowOp2Button = true;
            targetPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            targetPlayer.PArea.Portrait.ChangeOp2ButtonText("获得所有牌");
            targetPlayer.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                await GlobalSettings.Instance.ScoreEvaluation.GetAllCardsToHand(targetPlayer);
                GlobalSettings.Instance.ScoreEvaluation.gameObject.SetActive(false);
                TaskManager.Instance.UnBlockTask(TaskType.AnubisSkill1);
            });

            targetPlayer.ShowOp3Button = true;
            targetPlayer.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            targetPlayer.PArea.Portrait.ChangeOp3Button2Text("继续展示牌");
            targetPlayer.PArea.Portrait.OpButton3.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                await FlopCardForAnubisSkill1(playedCard, targetPlayer, showCardOriginPlayer);
            });
        }
        else
        {
            await GlobalSettings.Instance.ScoreEvaluation.DisAllCards();
            TaskManager.Instance.UnBlockTask(TaskType.AnubisSkill1);
        }
    }



    /// <summary>
    /// Fenrir技能2
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <returns></returns>
    public static async Task ActiveFenrirSkill2(Player mainPlayer)
    {
        Debug.Log("-----------------------------------------------Fenrir 有技能需要触发");
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// Osiris技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public static async Task ActiveOsirisSkill1(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        if (playedCard.IsDisguisedCard)
        {
            return;
        }
        if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
        {
            return;
        }
        if (mainPlayer.ID != TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.OsirisSkill1);

        Player player = mainPlayer;

        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Osiris技能1");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.DisableAllCards();

            GameObject card = playedCard.gameObject;
            if (!playedCard.TargetComponent.activeSelf)
            {
                DragSpellOnTable dragSpellOnTable = card.GetComponent<DragSpellOnTable>();
                dragSpellOnTable.OnCancelDrag();
            }
            else
            {
                DragSpellOnTarget dragSpellOnTarget = playedCard.TargetComponent.GetComponent<DragSpellOnTarget>();
                dragSpellOnTarget.ResetTriangleTargetComponent();
            }

            List<int> relationIds = new List<int>();
            relationIds.Add(playedCard.UniqueCardID);
            OneCardManager cardManager = GlobalSettings.Instance.PDeck.DisguisedCardAssetWithType(playedCard.Owner, SubTypeOfCards.Wugufengdeng, relationIds, false);
            cardManager.CanBePlayedNow = true;

            TaskManager.Instance.ExceptionBlockTask(TaskType.OsirisSkill1, "手牌当五谷丰登");
        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Osiris技能1");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.OsirisSkill1);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.OsirisSkill1][0].Task;
    }


    /// <summary>
    /// Osiris技能2
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public static async Task ActiveOsirisSkill2(Player mainPlayer)
    {
        if (DyingManager.Instance.IsInDyingInquiry && DyingManager.Instance.DyingPlayer.ID == mainPlayer.ID)
        {
            TaskManager.Instance.AddATask(TaskType.OsirisSkill2);

            Player player = mainPlayer;

            HighlightManager.DisableAllOpButtons();
            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动Osiris技能2");
            player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                HighlightManager.DisableAllCards();
                foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
                {
                    if (targetPlayer.ID != mainPlayer.ID)
                    {
                        targetPlayer.ShowOp1Button = true;
                        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                        targetPlayer.PArea.Portrait.ChangeOp1ButtonText("选择");
                        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                        {
                            HighlightManager.DisableAllOpButtons();
                            await targetPlayer.DrawSomeCards(2);
                            SelectCardForOsiris(targetPlayer, mainPlayer);
                        });
                    }
                }

            });

            List<int> canAttackTargets = player.TargetsCanAttackForDistance(1);
            if (canAttackTargets.Count == 0)
            {
                player.PArea.Portrait.OpButton2.enabled = false;
            }

            player.ShowOp3Button = true;
            player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp3Button2Text("不发动Osiris技能2");
            player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TaskManager.Instance.UnBlockTask(TaskType.OsirisSkill2);
            });

            await TaskManager.Instance.TaskBlockDic[TaskType.OsirisSkill2][0].Task;
        }

    }

    //贯石斧弃两张牌造成伤害
    public static void SelectCardForOsiris(Player targetPlayer, Player mainPlayer)
    {
        targetPlayer.ShowOp1Button = true;
        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        targetPlayer.PArea.Portrait.ChangeOp1ButtonText("取消");
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            GlobalSettings.Instance.CardSelectVisual.Dismiss();
            TaskManager.Instance.UnBlockTask(TaskType.OsirisSkill2);
        });

        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisSomeCardForDestNumber;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        List<int> rankList = new List<int>();
        rankList.Add(13);
        rankList.Add(14);
        GlobalSettings.Instance.CardSelectVisual.RankSumList = rankList;
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardAsOtherCardCompletion = async () =>
        {
            Debug.Log("Osiris复活");
            if (GlobalSettings.Instance.CardSelectVisual.RankSum == 13)
            {
                HealthManager.Instance.HealingEffect(1 - mainPlayer.Health, mainPlayer);
                HealthManager.Instance.SettleAfterHealing();
            }
            else if (GlobalSettings.Instance.CardSelectVisual.RankSum == 14)
            {
                HealthManager.Instance.HealingEffect(1 - mainPlayer.Health, mainPlayer);
                await mainPlayer.DrawSomeCards(1);
                HealthManager.Instance.SettleAfterHealing();
            }
            TaskManager.Instance.UnBlockTask(TaskType.OsirisSkill2);
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }
}
