using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCardManager : MonoBehaviour
{
    public static TipCardManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void SkipTipCard()
    {
        OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();
        if (cardManager != null)
        {
            TargetsManager.Instance.Targets.RemoveAt(0);
            //cardManager.TargetsPlayerIDs.RemoveAt(0);
            if (TargetsManager.Instance.Targets.Count != 0)
            {
                PlayCardManager.Instance.ActiveEffect(cardManager);
            }
            else
            {
                PlayCardManager.Instance.BackToWhoseTurn();
            }
        }
    }

    /// <summary>
    /// 无懈可击询问完毕，触发锦囊效果阶段
    /// </summary>
    public void ActiveTipCard()
    {
        OneCardManager cardManager = GlobalSettings.Instance.FirstOneCardOnTable();
        if (cardManager != null)
        {
            switch (cardManager.CardAsset.SubTypeOfCard)
            {
                case SubTypeOfCards.Nanmanruqin:
                    PlayCardManager.Instance.NeedToPlaySlash();
                    break;
                case SubTypeOfCards.Wanjianqifa:
                    PlayCardManager.Instance.NeedToPlayJink();
                    break;
                case SubTypeOfCards.Wugufengdeng:
                case SubTypeOfCards.Shunshouqianyang:
                case SubTypeOfCards.Guohechaiqiao:
                default:
                    Debug.Log("锦囊");
                    SkipTipCard();
                    break;
            }
        }
    }
}
