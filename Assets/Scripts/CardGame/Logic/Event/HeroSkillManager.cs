using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
using DG.Tweening;
using System;

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
        if (playedCard.CardAsset.TypeOfCard != TypesOfCards.Tips)
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
    public static async Task ActiveAthenaSkill2(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        //牌必须是锦囊牌
        if (playedCard.CardAsset.TypeOfCard != TypesOfCards.Tips)
        {
            await TaskManager.Instance.DontAwait();
            return;
        }
        if (mainPlayer.ID != playedCard.Owner.ID)
        {
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
    /// Fenrir技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveFenrirSkill1(Player mainPlayer, OneCardManager playedCard)
    {
        Debug.Log("-----------------------------------------------Fenrir 有技能需要触发");
        if (playedCard.Owner != mainPlayer)
        {
            playedCard.IsUseCardAssetB = false;
        }
        else
        {
            if (playedCard.CardAsset.TypeOfCard == TypesOfCards.Tips)
            {
                playedCard.IsUseCardAssetB = true;
            }
            else
            {
                playedCard.IsUseCardAssetB = false;
            }
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// Fenrir技能2
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <returns></returns>
    public static async Task ActiveFenrirSkill2(Player mainPlayer, OneCardManager playedCard)
    {
        Debug.Log("-----------------------------------------------Fenrir 有技能需要触发");
        if (playedCard.CardAsset.TypeOfCard == TypesOfCards.Tips)
        {
            playedCard.LaunchCardB(GlobalSettings.Instance.PDeck.CardAssetBWithType(SubTypeOfCards.Slash, playedCard.CardAsset));
            playedCard.IsUseCardAssetB = true;
        }
        else
        {
            playedCard.IsUseCardAssetB = false;
        }
        await TaskManager.Instance.DontAwait();
    }


    /// <summary>
    /// Osiris技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <returns></returns>
    public static async Task ActiveOsirisSkill1(Player mainPlayer, OneCardManager playedCard = null)
    {
        if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
        {
            return;
        }
        if (mainPlayer.ID != TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.OsirisSkill2State) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.OsirisSkill2State] == true)
        {
            Debug.Log("发动过木乃伊");
            return;
        }
        if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.OsirisSkill1State) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.OsirisSkill1State] == true)
        {
            if (playedCard == null)
            {
                return;
            }
            if (playedCard != null && playedCard.IsDisguisedCard)
            {
                return;
            }

            GameObject card = playedCard.gameObject;
            if (!playedCard.TargetComponent.activeSelf)
            {
                DragSpellOnTable dragSpellOnTable = card.GetComponent<DragSpellOnTable>();
                dragSpellOnTable.OnCancelDrag();
            }

            List<int> relationIds = new List<int>();
            relationIds.Add(playedCard.UniqueCardID);
            OneCardManager cardManager = GlobalSettings.Instance.PDeck.DisguisedCardAssetWithType(playedCard.Owner, SubTypeOfCards.Wugufengdeng, relationIds, false);
            cardManager.CanBePlayedNow = true;
            await TaskManager.Instance.ReturnException("不走下面了");
        }
        else
        {
            Player player = mainPlayer;

            //HighlightManager.DisableAllOpButtons();
            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动Osiris技能1");
            player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                HighlightManager.DisableAllCards();
                HighlightManager.ShowACards(player);
                HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.OsirisSkill1State] = true;
            });
        }
        await TaskManager.Instance.DontAwait();
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
            player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                HighlightManager.DisableAllCards();
                //本回合发动过木乃伊了
                HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.OsirisSkill2State] = true;
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
        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
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

    /// <summary>
    /// Nephthys技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveNephthysSkill1(Player mainPlayer, OneCardManager playedCard)
    {
        if (TurnManager.Instance.whoseTurn.ID != mainPlayer.ID)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.NephthysSkill1);

        Player player = mainPlayer;

        //已选玩家数量
        List<Player> selectPlayerList = new List<Player>();

        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Nephthys技能1");
        player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.DisableAllCards();
            foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
            {
                targetPlayer.ShowOp1Button = true;
                targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                targetPlayer.PArea.Portrait.ChangeOp1ButtonText("选择");
                targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                {
                    targetPlayer.ShowOp1Button = false;
                    selectPlayerList.Add(targetPlayer);
                    if (selectPlayerList.Count > 1)
                    {
                        mainPlayer.ShowOp2Button = true;
                    }
                    if (selectPlayerList.Count >= 2)
                    {
                        HighlightManager.DisableAllOpButtons();
                        await DrawCardsForSelectPlayers(selectPlayerList);
                        TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill1);
                    }
                });
            }

            mainPlayer.ShowOp2Button = false;
            mainPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
            mainPlayer.PArea.Portrait.ChangeOp1ButtonText("完成");
            mainPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                await DrawCardsForSelectPlayers(selectPlayerList);
                TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill1);
            });

        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Nephthys技能1");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill1);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.NephthysSkill1][0].Task;
    }

    public static async Task DrawCardsForSelectPlayers(List<Player> selectPlayers)
    {
        TurnManager.Instance.DrawCardLimitPerTurn -= 1;
        foreach (Player p in selectPlayers)
        {
            await p.DrawSomeCards(1);
        }
    }

    /// <summary>
    /// Nephthys技能2
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public static async Task ActiveNephthysSkill2(Player mainPlayer, OneCardManager playedCard, int targetId)
    {
        TaskManager.Instance.AddATask(TaskType.NephthysSkill2);

        Player player = mainPlayer;

        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Nephthys技能2");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.DisableAllCards();
            await JudgementForNephthysSkill2(mainPlayer, GlobalSettings.Instance.FindPlayerByID(targetId));
        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Nephthys技能2");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill2);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.NephthysSkill2][0].Task;
    }

    public static async Task JudgementForNephthysSkill2(Player mainPlayer, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.Judgement;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardForJudgementCompletion = async (card) =>
        {
            await mainPlayer.DisACardFromHand(card.GetComponent<OneCardManager>().UniqueCardID);
            DelayTipManager.flopCardManager = card.GetComponent<OneCardManager>();
            if (DelayTipManager.flopCardManager.CardAsset.CardColor == CardColor.Red)
            {
                await ActiveNephthysSkill3(mainPlayer, DelayTipManager.flopCardManager);
            }
            TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill2);
        };

        for (int i = mainPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        await TaskManager.Instance.DontAwait();
    }


    /// <summary>
    /// Nephthys 技能3
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveNephthysSkill3(Player mainPlayer, OneCardManager playedCard)
    {
        //你的回合外，你使用牌
        if (playedCard.Owner.ID != mainPlayer.ID || mainPlayer.ID == TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        if (playedCard.CardAsset.CardColor != CardColor.Red)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.NephthysSkill3);

        Player player = mainPlayer;

        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Nephthys技能3");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.DisableAllCards();
            await player.DrawSomeCards(1);
            TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill3);
        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Nephthys技能3");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.NephthysSkill3);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.NephthysSkill3][0].Task;
    }

    /// <summary>
    /// 普罗米修斯技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="heroSkillActivePhase"></param>
    /// <returns></returns>
    public static async Task ActivePrometheusSkill1(Player mainPlayer, OneCardManager playedCard, HeroSkillActivePhase heroSkillActivePhase)
    {
        if (TurnManager.Instance.whoseTurn.ID != mainPlayer.ID)
        {
            return;
        }

        switch (heroSkillActivePhase)
        {
            case HeroSkillActivePhase.Hook27:
                {
                    TaskManager.Instance.AddATask(TaskType.PrometheusSkill1);

                    Player player = mainPlayer;

                    HighlightManager.DisableAllOpButtons();
                    player.ShowOp2Button = true;
                    player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp2ButtonText("发动Prometheus技能1");
                    player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
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
                                    await ShowOtherCard(mainPlayer, playedCard, targetPlayer);
                                });
                            }
                        }
                    });

                    player.ShowOp3Button = true;
                    player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp3Button2Text("不发动Prometheus技能1");
                    player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        TaskManager.Instance.UnBlockTask(TaskType.PrometheusSkill1);
                        if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
                        {
                            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                        }
                    });

                    await TaskManager.Instance.TaskBlockDic[TaskType.PrometheusSkill1][0].Task;
                }
                break;
            case HeroSkillActivePhase.Hook1:
                {
                    if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
                    {
                        return;
                    }
                    if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.PrometheusSkill1Card) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.PrometheusSkill1Card])
                    {
                        OneCardManager storedCardManager = HeroSkillState.HeroSkillCardDic_Once[HeroSKillStateKey.PrometheusSkill1Card];
                        if (playedCard.CardAsset.TypeOfCard == storedCardManager.CardAsset.TypeOfCard)
                        {
                            Debug.Log("跟之前的牌类型一样");
                            await mainPlayer.DrawSomeCards(1);
                        }
                        await TaskManager.Instance.DontAwait();
                    }
                    else
                    {
                        await TaskManager.Instance.DontAwait();
                    }
                }
                break;
        }


    }

    public static async Task ShowOtherCard(Player mainPlayer, OneCardManager playedCard, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.Judgement;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardForJudgementCompletion = (card) =>
        {
            //存储牌
            HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.PrometheusSkill1Card] = true;
            HeroSkillState.HeroSkillCardDic_Once[HeroSKillStateKey.PrometheusSkill1Card] = card.GetComponent<OneCardManager>();
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            TaskManager.Instance.UnBlockTask(TaskType.PrometheusSkill1);
        };

        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 普罗米修斯技能2
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="heroSkillActivePhase"></param>
    /// <returns></returns>
    public static async Task ActivePrometheusSkill2(Player mainPlayer, OneCardManager playedCard, HeroSkillActivePhase heroSkillActivePhase)
    {
        if (TurnManager.Instance.whoseTurn.ID == mainPlayer.ID)
        {
            return;
        }
        switch (heroSkillActivePhase)
        {
            case HeroSkillActivePhase.Hook27:
                {
                    TaskManager.Instance.AddATask(TaskType.PrometheusSkill2);

                    Player player = mainPlayer;

                    HighlightManager.DisableAllCards();
                    HighlightManager.DisableAllOpButtons();
                    player.ShowOp2Button = true;
                    player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp2ButtonText("发动Prometheus技能2");
                    player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        HighlightManager.DisableAllCards();
                        await mainPlayer.DrawSomeCards(2);
                        await GiveCardsToOther(mainPlayer, playedCard, TurnManager.Instance.whoseTurn);
                    });

                    player.ShowOp3Button = true;
                    player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp3Button2Text("不发动Prometheus技能2");
                    player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        TaskManager.Instance.UnBlockTask(TaskType.PrometheusSkill2);
                        if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
                        {
                            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                        }
                    });

                    await TaskManager.Instance.TaskBlockDic[TaskType.PrometheusSkill2][0].Task;
                }
                break;
            case HeroSkillActivePhase.Hook28:
                if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.PrometheusSkill2Card) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.PrometheusSkill2Card] == true)
                {
                    if (!HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.EnteredDying) || HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.EnteredDying] == false)
                    {
                        await LooseHealthManager.LooseHealth(mainPlayer, mainPlayer, 1);
                    }
                }
                break;
        }
    }

    public static async Task GiveCardsToOther(Player mainPlayer, OneCardManager playedCard, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.GiveCardToOther;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardAsOtherCardCompletion = async () =>
        {
            //存储牌
            HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.PrometheusSkill2Card] = true;
            GlobalSettings.Instance.CardSelectVisual.DisAllCards();
            foreach (int cardId in GlobalSettings.Instance.CardSelectVisual.SelectCardIds)
            {
                GameObject card = IDHolder.GetGameObjectWithID(cardId);
                await mainPlayer.GiveCardToTarget(targetPlayer, card.GetComponent<OneCardManager>());
            }
            GlobalSettings.Instance.CardSelectVisual.SelectCardIds.Clear();
            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            TaskManager.Instance.UnBlockTask(TaskType.PrometheusSkill2);
        };

        for (int i = mainPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = mainPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        await TaskManager.Instance.DontAwait();
    }

    /// <summary>
    /// 刘封技能
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <param name="targetID"></param>
    /// <param name="heroSkillActivePhase"></param>
    /// <returns></returns>
    public static async Task ActiveLiufengSkill1(Player mainPlayer, OneCardManager playedCard, int targetID, HeroSkillActivePhase heroSkillActivePhase)
    {
        switch (heroSkillActivePhase)
        {
            case HeroSkillActivePhase.Hook19:
                {
                    if (TurnManager.Instance.whoseTurn.ID != mainPlayer.ID)
                    {
                        return;
                    }

                    TaskManager.Instance.AddATask(TaskType.LiufengSkill1);

                    Player player = mainPlayer;

                    HighlightManager.DisableAllOpButtons();
                    player.ShowOp2Button = true;
                    player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp2ButtonText("发动刘封技能");
                    player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        //已选玩家数量
                        List<Player> selectPlayerList = new List<Player>();
                        foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
                        {
                            targetPlayer.ShowOp1Button = true;
                            targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                            targetPlayer.PArea.Portrait.ChangeOp1ButtonText("选择");
                            targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                            {
                                targetPlayer.ShowOp1Button = false;
                                selectPlayerList.Add(targetPlayer);
                                if (selectPlayerList.Count > 1)
                                {
                                    mainPlayer.ShowOp2Button = true;
                                }
                                if (selectPlayerList.Count >= 2)
                                {
                                    HighlightManager.DisableAllOpButtons();
                                    await DrawCardsForSelectPlayersLiufengSkill1(mainPlayer, selectPlayerList);
                                    TaskManager.Instance.UnBlockTask(TaskType.LiufengSkill1);
                                }
                            });
                        }

                        mainPlayer.ShowOp2Button = false;
                        mainPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                        mainPlayer.PArea.Portrait.ChangeOp1ButtonText("完成");
                        mainPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                        {
                            HighlightManager.DisableAllOpButtons();
                            await DrawCardsForSelectPlayersLiufengSkill1(mainPlayer, selectPlayerList);
                            TaskManager.Instance.UnBlockTask(TaskType.LiufengSkill1);
                        });
                    });

                    player.ShowOp3Button = true;
                    player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                    player.PArea.Portrait.ChangeOp3Button2Text("不发动刘封技能");
                    player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
                    {
                        HighlightManager.DisableAllOpButtons();
                        TaskManager.Instance.UnBlockTask(TaskType.LiufengSkill1);
                    });

                    await TaskManager.Instance.TaskBlockDic[TaskType.LiufengSkill1][0].Task;
                }
                break;
        }


    }

    /// <summary>
    /// 刘封技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="selectPlayers"></param>
    /// <returns></returns>
    public static async Task DrawCardsForSelectPlayersLiufengSkill1(Player mainPlayer, List<Player> selectPlayers)
    {
        foreach (Player p in selectPlayers)
        {
            await GetCardOnHeroForPlayer(mainPlayer, p);
        }
    }

    public static async Task GetCardOnHeroForPlayer(Player mainPlayer, Player targetPlayer)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.ShowTargetACard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardForJudgementCompletion = async (card) =>
        {
            //存储牌
            //await mainPlayer.GiveCardToTarget(targetPlayer, card.GetComponent<OneCardManager>());
            await targetPlayer.GiveCardOnOtherTarget(mainPlayer, card.GetComponent<OneCardManager>());
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
    /// 刘封技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveLiufengSkill2(Player mainPlayer)
    {
        if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
        {
            return;
        }
        if (mainPlayer.ID == TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        if (CounterManager.Instance.SlashCount >= CounterManager.Instance.MaxSlashLimit)
        {
            return;
        }
        if (mainPlayer.HeroHeadLogic.CardsOnHero.Count < 2)
        {
            return;
        }
        Player player = TurnManager.Instance.whoseTurn;

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("发动刘封技能2");
        player.PArea.Portrait.OpButton3.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.DisableAllCards();
            await SelectCardForLiuFeng(mainPlayer, TurnManager.Instance.whoseTurn);
        });
        await TaskManager.Instance.DontAwait();
    }

    public static async Task SelectCardForLiuFeng(Player mainPlayer, Player targetPlayer)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.SelectSomeCardsToAsACard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = 2;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
            OneCardManager cardManager = GlobalSettings.Instance.PDeck.DisguisedCardAssetWithType(targetPlayer, SubTypeOfCards.Slash, GlobalSettings.Instance.CardSelectVisual.SelectCardIds, false);
            List<int> targets = new List<int>();
            targets.Add(mainPlayer.ID);

            GlobalSettings.Instance.CardSelectVisual.Dismiss();

            await targetPlayer.DragTarget(cardManager, targets);
            tcs.SetResult(true);
        };

        for (int i = mainPlayer.HeroHeadLogic.CardsOnHero.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.HeroHeadLogic.CardsOnHero[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        await tcs.Task;
    }

    /// <summary>
    /// 杨修技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveYangxiuSkill1(Player mainPlayer, OneCardManager playedCard = null)
    {
        if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
        {
            return;
        }
        if (mainPlayer.ID != TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.YangxiuSkill1State) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.YangxiuSkill1State] == true)
        {

        }
        else
        {
            Player player = mainPlayer;

            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动Yangxiu技能1");
            player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                HighlightManager.DisableAllCards();
                HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.YangxiuSkill1State] = true;
                foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
                {
                    if (targetPlayer.Hand.CardsInHand.Count > 0)
                    {
                        targetPlayer.ShowOp1Button = true;
                        targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                        targetPlayer.PArea.Portrait.ChangeOp1ButtonText("选择");
                        targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                        {
                            HighlightManager.DisableAllOpButtons();
                            await GuessCardForPlayer(mainPlayer, targetPlayer);
                        });
                    }
                }
            });
        }
        await TaskManager.Instance.DontAwait();
    }

    public static async Task GuessCardForPlayer(Player mainPlayer, Player targetPlayer)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.ShowTargetACard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardForJudgementCompletion = (card) =>
        {
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CustomButtonsVisual.CustomButtonType = CustomButtonType.ColorAndType;
            GlobalSettings.Instance.CustomButtonsVisual.Show();
            GlobalSettings.Instance.CustomButtonsVisual.AfterClickButtonCompletion = (buttonTxt) =>
            {
                if (buttonTxt == "颜色")
                {
                    GlobalSettings.Instance.CustomButtonsVisual.CustomButtonType = CustomButtonType.Colors;
                    GlobalSettings.Instance.CustomButtonsVisual.Show();
                    GlobalSettings.Instance.CustomButtonsVisual.AfterClickButtonCompletion = async (buttonTxt) =>
                    {
                        Debug.Log(buttonTxt);
                        CardColor selectColor = buttonTxt == "红色" ? CardColor.Red : CardColor.Black;
                        if (cardManager.CardAsset.CardColor == selectColor)
                        {
                            Debug.Log("猜中了");
                            await targetPlayer.GiveCardToTarget(mainPlayer, cardManager);
                        }
                        else
                        {
                            await targetPlayer.DisACardFromHand(cardManager.UniqueCardID);
                        }
                        if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
                        {
                            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                        }
                    };
                }
                else
                {
                    GlobalSettings.Instance.CustomButtonsVisual.CustomButtonType = CustomButtonType.TypeOfCard;
                    GlobalSettings.Instance.CustomButtonsVisual.Show();
                    GlobalSettings.Instance.CustomButtonsVisual.AfterClickButtonCompletion = async (buttonTxt) =>
                    {
                        Debug.Log(buttonTxt);
                        TypesOfCards selectType = buttonTxt == "基本牌" ? TypesOfCards.Base : (buttonTxt == "锦囊牌" ? TypesOfCards.Tips : TypesOfCards.Equipment);
                        if (cardManager.CardAsset.TypeOfCard == selectType)
                        {
                            Debug.Log("猜中了");
                            await targetPlayer.GiveCardToTarget(mainPlayer, cardManager);
                        }
                        else
                        {
                            await targetPlayer.DisACardFromHand(cardManager.UniqueCardID);
                        }
                        if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
                        {
                            HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                        }
                    };
                }
            };
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

    public static async Task ActiveYangxiuSkill2(Player mainPlayer, OneCardManager playedCard = null)
    {
        //如果目标不是杨修不触发
        if (!playedCard.TargetsPlayerIDs.Contains(mainPlayer.ID))
        {
            return;
        }
        //目标必须是多个
        if (playedCard.TargetsPlayerIDs.Count < 2)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.YangxiuSkill2);

        Player player = mainPlayer;

        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动杨修技能2");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            HighlightManager.DisableAllCards();
            playedCard.TargetsPlayerIDs.Remove(mainPlayer.ID);
            await mainPlayer.DrawSomeCards(1);
            TaskManager.Instance.UnBlockTask(TaskType.YangxiuSkill2);
        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动杨修技能2");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.YangxiuSkill2);
            if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
            {
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.YangxiuSkill2][0].Task;
    }

    public static async Task ActiveYangxiuSkill3(Player mainPlayer, int targetId)
    {
        if (mainPlayer.ID != targetId)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.YangxiuSkill3);

        Player player = mainPlayer;

        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动杨修技能3");
        player.PArea.Portrait.OpButton2.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            GlobalSettings.Instance.CustomButtonsVisual.CustomButtonType = CustomButtonType.TypeOfCard;
            GlobalSettings.Instance.CustomButtonsVisual.Show();
            GlobalSettings.Instance.CustomButtonsVisual.AfterClickButtonCompletion = (buttonTxt) =>
            {
                Debug.Log(buttonTxt);
                TypesOfCards selectType = buttonTxt == "基本牌" ? TypesOfCards.Base : (buttonTxt == "锦囊牌" ? TypesOfCards.Tips : TypesOfCards.Equipment);
                if (HeroSkillState.HeroSkillCardTypeDic_Once.ContainsKey(HeroSKillStateKey.YangxiuSkill3State))
                {
                    HeroSkillState.HeroSkillCardTypeDic_Once[HeroSKillStateKey.YangxiuSkill3State].Add(selectType);
                }
                else
                {
                    List<TypesOfCards> typesOfCards = new List<TypesOfCards>();
                    typesOfCards.Add(selectType);
                    HeroSkillState.HeroSkillCardTypeDic_Once[HeroSKillStateKey.YangxiuSkill3State] = typesOfCards;
                }
                TaskManager.Instance.UnBlockTask(TaskType.YangxiuSkill3);
            };
        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动杨修技能3");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.YangxiuSkill3);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.YangxiuSkill3][0].Task;

    }

    /// <summary>
    /// Freyj 技能1
    /// </summary>
    /// <param name="mainPlayer"></param>
    /// <param name="playedCard"></param>
    /// <returns></returns>
    public static async Task ActiveFreyjSkill1(Player mainPlayer, OneCardManager playedCard)
    {
        Debug.Log("-----------------------------------------------Freyj 有技能需要触发");
        if (playedCard.CardAsset.Suits == CardSuits.Spades)
        {
            playedCard.LaunchCardB(GlobalSettings.Instance.PDeck.CardAssetBWithSuite(CardSuits.Hearts, playedCard.CardAsset));
            playedCard.IsUseCardAssetB = true;
        }
        else
        {
            playedCard.IsUseCardAssetB = false;
        }
        await TaskManager.Instance.DontAwait();
    }

    public static async Task ActiveFreyjSkill2(Player mainPlayer)
    {
        if (mainPlayer.ID != TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        TaskManager.Instance.AddATask(TaskType.FreyjSkill2);

        Player player = mainPlayer;

        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        player.ShowOp2Button = true;
        player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp2ButtonText("发动Freyj技能2");
        player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            await ShowAllHandCardsForFreyj(mainPlayer);
        });

        player.ShowOp3Button = true;
        player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        player.PArea.Portrait.ChangeOp3Button2Text("不发动Freyj技能2");
        player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            TaskManager.Instance.UnBlockTask(TaskType.FreyjSkill2);
        });

        await TaskManager.Instance.TaskBlockDic[TaskType.FreyjSkill2][0].Task;
    }

    public static async Task ShowAllHandCardsForFreyj(Player mainPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.ShowTargetACard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);

        //获取第一个颜色
        GameObject firstCard = IDHolder.GetGameObjectWithID(mainPlayer.Hand.CardsInHand[0]);
        OneCardManager firstCardManager = firstCard.GetComponent<OneCardManager>();
        CardColor firstCardColor = firstCardManager.CardAsset.CardColor;

        bool allCardsSameColor = true;

        for (int i = mainPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
            if (firstCardColor != cardManager.CardAsset.CardColor)
            {
                allCardsSameColor = false;
            }
        }

        var tcs = new TaskCompletionSource<bool>();

        Sequence s = DOTween.Sequence();
        s.AppendInterval(1f);
        s.OnComplete(() =>
        {
            tcs.SetResult(true);
        });

        await tcs.Task;

        GlobalSettings.Instance.CardSelectVisual.Dismiss();

        if (allCardsSameColor == false)
        {
            TaskManager.Instance.UnBlockTask(TaskType.FreyjSkill2);
        }
        else
        {
            Debug.Log("丰收女神需要发牌了");
            int cardCount = 0;
            foreach (Player targetPlayer in GlobalSettings.Instance.PlayerInstances)
            {
                targetPlayer.ShowOp1Button = true;
                targetPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
                targetPlayer.PArea.Portrait.ChangeOp1ButtonText("选择");
                targetPlayer.PArea.Portrait.OpButton1.onClick.AddListener(async () =>
                {
                    targetPlayer.ShowOp1Button = false;
                    cardCount++;
                    await targetPlayer.DrawSomeCards(1);
                    if (cardCount == GlobalSettings.Instance.PlayerInstances.Select(n => n.IsDead == false).ToList().Count || cardCount == mainPlayer.Hand.CardsInHand.Count)
                    {
                        TaskManager.Instance.UnBlockTask(TaskType.FreyjSkill2);
                    }
                });
            }

            mainPlayer.ShowOp2Button = true;
            mainPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            mainPlayer.PArea.Portrait.ChangeOp2ButtonText("完成");
            mainPlayer.PArea.Portrait.OpButton2.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TaskManager.Instance.UnBlockTask(TaskType.FreyjSkill2);
            });
        }

    }

    public static async Task ActiveLiruSkill1(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);
        if (targetPlayer.Hand.CardsInHand.Count == 0)
        {
            Debug.Log("李儒技能1需要发动");
            TaskManager.Instance.AddATask(TaskType.LiruSkill1);

            Player player = mainPlayer;

            HighlightManager.DisableAllCards();
            HighlightManager.DisableAllOpButtons();
            player.ShowOp2Button = true;
            player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp2ButtonText("发动Liru技能1");
            player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
            {
                HighlightManager.DisableAllOpButtons();
                await DisCardsForLiru(mainPlayer, targetPlayer);
            });

            player.ShowOp3Button = true;
            player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp3Button2Text("不发动Liru技能1");
            player.PArea.Portrait.OpButton3.onClick.AddListener(() =>
            {
                HighlightManager.DisableAllOpButtons();
                TaskManager.Instance.UnBlockTask(TaskType.LiruSkill1);
            });

            await TaskManager.Instance.TaskBlockDic[TaskType.LiruSkill1][0].Task;
        }
        else
        {
            Debug.Log("李儒技能1不需要发动");
        }
        await TaskManager.Instance.DontAwait();
    }

    public static async Task DisCardsForLiru(Player mainPlayer, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
            OneCardManager cardManager = GlobalSettings.Instance.PDeck.SkillCardWithAsset(mainPlayer, GlobalSettings.Instance.SkillVRCardAsset);
            await SettleManager.Instance.StartSettle(cardManager, SpellAttribute.None, targetPlayer, 1, false);
            TaskManager.Instance.UnBlockTask(TaskType.LiruSkill1);
            if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
            {
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
        };

        for (int i = mainPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
        for (int i = mainPlayer.EquipmentLogic.CardsInEquipment.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.EquipmentLogic.CardsInEquipment[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
        }
    }

    public static async Task ActiveLiruSkill2(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        Debug.Log("李儒技能2");
        if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
        {
            return;
        }
        if (mainPlayer.ID != TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.LiruSkill2State) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.LiruSkill2State] == true)
        {

        }
        else
        {
            if (HeroSkillState.HeroSkillBooleanDic_Once.ContainsKey(HeroSKillStateKey.LiruSkill3State) && HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.LiruSkill3State] == true)
            {

            }
            else
            {
                Player player = mainPlayer;

                Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);

                player.ShowOp2Button = true;
                player.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                player.PArea.Portrait.ChangeOp2ButtonText("发动liru技能2");
                player.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
                {
                    HighlightManager.DisableAllOpButtons();
                    HighlightManager.DisableAllCards();
                    HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.LiruSkill2State] = true;
                    await SelectACardPutOnDeckTop(mainPlayer, targetPlayer);
                });
            }

        }
        await TaskManager.Instance.DontAwait();
    }
    public static async Task SelectACardPutOnDeckTop(Player mainPlayer, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.SelectATypeOfCardPutOnDeckTop;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = () =>
        {
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
                        targetPlayer.ShowOp2Button = true;
                        targetPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
                        targetPlayer.PArea.Portrait.ChangeOp2ButtonText("交出一张锦囊牌");
                        targetPlayer.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
                        {
                            HighlightManager.DisableAllOpButtons();
                            await GetCardForTips(mainPlayer, targetPlayer);
                        });

                        targetPlayer.ShowOp3Button = true;
                        targetPlayer.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
                        targetPlayer.PArea.Portrait.ChangeOp3Button2Text("依次弃置两张非锦囊牌");
                        targetPlayer.PArea.Portrait.OpButton3.onClick.AddListener(async () =>
                        {
                            HighlightManager.DisableAllOpButtons();
                            if (targetPlayer.Hand.CardsInHand.Count >= 2)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    int cId = targetPlayer.Hand.CardsInHand[targetPlayer.Hand.CardsInHand.Count - 1];
                                    GameObject card = IDHolder.GetGameObjectWithID(cId);
                                    OneCardManager cardManager = card.GetComponent<OneCardManager>();
                                    await targetPlayer.DisACardWithCardManager(cardManager);
                                }
                            }

                            if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
                            {
                                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
                            }
                        });
                    });
                }
            }
        };

        for (int i = mainPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(mainPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips && cardManager.CardAsset.CardColor == CardColor.Black)
            {
                GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
            }
        }

        if (GlobalSettings.Instance.CardSelectVisual.CardsOnHand.Count == 0)
        {
            GlobalSettings.Instance.CardSelectVisual.Dismiss();
            if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
            {
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
        }
    }

    public static async Task GetCardForTips(Player mainPlayer, Player targetPlayer)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.ShowTargetACard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.AfterSelectCardForJudgementCompletion = async (card) =>
        {
            //存储牌
            //await mainPlayer.GiveCardToTarget(targetPlayer, card.GetComponent<OneCardManager>());
            await targetPlayer.GiveCardToTarget(mainPlayer, card.GetComponent<OneCardManager>());
            if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
            {
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
        };

        for (int i = targetPlayer.JudgementLogic.CardsInJudgement.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.JudgementLogic.CardsInJudgement[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips)
            {
                GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
            }
        }
        for (int i = targetPlayer.Hand.CardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject card = IDHolder.GetGameObjectWithID(targetPlayer.Hand.CardsInHand[i]);
            OneCardManager cardManager = card.GetComponent<OneCardManager>();
            if (cardManager.CardAsset.TypeOfCard == TypesOfCards.Tips)
            {
                GlobalSettings.Instance.CardSelectVisual.AddHandCardsAtIndex(cardManager);
            }
        }
    }

    public static async Task ActiveLiruSkill3(Player mainPlayer, OneCardManager playedCard, int targetID)
    {
        Debug.Log("李儒技能3");
        if (TurnManager.Instance.TurnPhase != TurnPhase.PlayCard)
        {
            return;
        }
        if (mainPlayer.ID != TurnManager.Instance.whoseTurn.ID)
        {
            return;
        }
        if (HeroSkillState.HeroSkillBooleanDic_AllTheGame.ContainsKey(HeroSKillStateKey.LiruSkill3State) && HeroSkillState.HeroSkillBooleanDic_AllTheGame[HeroSKillStateKey.LiruSkill3State] == true)
        {

        }
        else
        {
            Player player = mainPlayer;

            Player targetPlayer = GlobalSettings.Instance.FindPlayerByID(targetID);

            player.ShowOp3Button = true;
            player.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
            player.PArea.Portrait.ChangeOp3Button2Text("发动liru技能3");
            player.PArea.Portrait.OpButton3.onClick.AddListener(async () =>
            {
                //HighlightManager.DisableAllOpButtons();
                //HighlightManager.DisableAllCards();
                //HeroSkillState.HeroSkillBooleanDic_AllTheGame[HeroSKillStateKey.LiruSkill3State] = true;
                //HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.LiruSkill3State] = true;
                //Player curPlayer = TurnManager.Instance.whoseTurn.OtherPlayer;
                //curPlayer = TurnManager.Instance.whoseTurn.OtherPlayer;
                //await ProcessTasks(mainPlayer, curPlayer);
                // 获取组件所属对象的 RectTransform 组件
                GlobalSettings.Instance.PlayerInstances[4].PArea.transform.DOMove(GlobalSettings.Instance.PlayerInstances[5].PArea.transform.position, 1f);
                GlobalSettings.Instance.PlayerInstances[5].PArea.transform.DOMove(GlobalSettings.Instance.PlayerInstances[4].PArea.transform.position, 1f);
                GlobalSettings.Instance.PlayerInstances[5] = GlobalSettings.Instance.Players[AreaPosition.Opponent4];
                GlobalSettings.Instance.PlayerInstances[4] = GlobalSettings.Instance.Players[AreaPosition.Opponent5];
                GlobalSettings.Instance.PlayerInstances[4].PArea.Owner = AreaPosition.Opponent4;
                GlobalSettings.Instance.PlayerInstances[5].PArea.Owner = AreaPosition.Opponent5;
                GlobalSettings.Instance.PlayerInstances[4].PArea.HandVisual.Owner = AreaPosition.Opponent4;
                GlobalSettings.Instance.PlayerInstances[5].PArea.HandVisual.Owner = AreaPosition.Opponent5;
                foreach (Player player in GlobalSettings.Instance.PlayerInstances)
                {
                    Debug.Log("@@###@@@@@@@@@@@ " + player.PArea.Owner);
                }
            });
        }
        await TaskManager.Instance.DontAwait();
    }

    public static async Task ProcessTasks(Player mainPlayer, Player curPlayer)
    {
        if (curPlayer == TurnManager.Instance.whoseTurn)
        {
            HeroSkillState.HeroSkillBooleanDic_Once[HeroSKillStateKey.LiruSkill3State] = false;
            // 队列中的任务已经全部处理完毕
            if (TaskManager.Instance.TaskBlockDic.Keys.Count == 0)
            {
                HighlightManager.EnableCardsWithType(TurnManager.Instance.whoseTurn);
            }
            return;
        }

        await HandleLiru2(mainPlayer, curPlayer);
        Debug.Log("来到下一个玩家：" + curPlayer.PArea.Owner);

        curPlayer = curPlayer.OtherPlayer;
        await ProcessTasks(mainPlayer, curPlayer);
    }

    public static async Task HandleLiru2(Player mainPlayer, Player curPlayer)
    {

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();


        HighlightManager.DisableAllCards();
        HighlightManager.DisableAllOpButtons();
        curPlayer.ShowOp2Button = true;
        curPlayer.PArea.Portrait.OpButton2.onClick.RemoveAllListeners();
        curPlayer.PArea.Portrait.ChangeOp2ButtonText("弃牌");
        curPlayer.PArea.Portrait.OpButton2.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            await DisCardsForLiru3(mainPlayer, curPlayer, tcs);
        });

        curPlayer.ShowOp3Button = true;
        curPlayer.PArea.Portrait.OpButton3.onClick.RemoveAllListeners();
        curPlayer.PArea.Portrait.ChangeOp3Button2Text("不弃牌");
        curPlayer.PArea.Portrait.OpButton3.onClick.AddListener(async () =>
        {
            HighlightManager.DisableAllOpButtons();
            OneCardManager cardManager = GlobalSettings.Instance.PDeck.SkillCardWithAsset(mainPlayer, GlobalSettings.Instance.SkillVRCardAsset);
            await SettleManager.Instance.StartSettle(cardManager, SpellAttribute.FireSlash, curPlayer, 1, false);
            tcs.SetResult(true);
        });


        await tcs.Task;
    }

    public static async Task DisCardsForLiru3(Player mainPlayer, Player targetPlayer, TaskCompletionSource<bool> tcs)
    {
        GlobalSettings.Instance.CardSelectVisual.PanelType = CardSelectPanelType.DisHandCard;
        GlobalSettings.Instance.CardSelectVisual.gameObject.SetActive(true);
        GlobalSettings.Instance.CardSelectVisual.DisCardNumber = targetPlayer.EquipmentLogic.CardsInEquipment.Count == 0 ? 1 : targetPlayer.EquipmentLogic.CardsInEquipment.Count;
        GlobalSettings.Instance.CardSelectVisual.AfterDisCardCompletion = async () =>
        {
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
    }


    public static async Task TestAsync(Player mainPlayer, string skillName)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        mainPlayer.ShowOp1Button = true;
        mainPlayer.PArea.Portrait.OpButton1.onClick.RemoveAllListeners();
        mainPlayer.PArea.Portrait.ChangeOp1ButtonText("发动" + skillName);
        mainPlayer.PArea.Portrait.OpButton1.onClick.AddListener(() =>
        {
            HighlightManager.DisableAllOpButtons();
            tcs.SetResult(true);
        });
        await tcs.Task;
    }
}
