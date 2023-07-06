using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum CustomButtonType
{
    ColorAndType,
    Colors,
    TypeOfCard
}
public class CustomButtons : MonoBehaviour
{
    public GameObject CardsSlots;
    public CustomButtonType CustomButtonType;
    public List<GameObject> Buttons = new List<GameObject>();
    public System.Action<string> AfterClickButtonCompletion;

    public void Show()
    {
        switch (CustomButtonType)
        {
            case CustomButtonType.ColorAndType:
                {
                    List<string> strlist = new List<string>();
                    strlist.Add("颜色");
                    strlist.Add("类别");
                    AddButtonsWithStrList(strlist);
                }
                break;
            case CustomButtonType.Colors:
                {
                    List<string> strlist = new List<string>();
                    strlist.Add("红色");
                    strlist.Add("黑色");
                    AddButtonsWithStrList(strlist);
                }
                break;
            case CustomButtonType.TypeOfCard:
                {
                    List<string> strlist = new List<string>();
                    strlist.Add("基本牌");
                    strlist.Add("锦囊牌");
                    strlist.Add("装备牌");
                    AddButtonsWithStrList(strlist);
                }
                break;
        }
        this.gameObject.SetActive(true);
    }
    public void Dismiss()
    {
        foreach (GameObject card in Buttons)
        {
            Destroy(card);
        }
        Buttons.Clear();
        this.gameObject.SetActive(false);
    }
    // 顺手牵羊、过河拆桥、寒冰剑 加牌
    public void AddButtonsWithStrList(List<string> strs)
    {
        GameObject slots = CardsSlots;

        foreach (string str in strs)
        {
            // create a new card from prefab
            GameObject button = GameObject.Instantiate(GlobalSettings.Instance.CustomButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            button.GetComponent<Button>().GetComponentInChildren<Text>().text = str;
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Dismiss();
                string buttonTxt = button.GetComponent<Button>().GetComponentInChildren<Text>().text;
                Debug.Log(buttonTxt);
                this.AfterClickButtonCompletion.Invoke(buttonTxt);
            });

            // parent a new creature gameObject to table slots
            button.transform.SetParent(slots.transform);
            button.transform.localScale = new Vector3(1, 1, 1);
            Buttons.Add(button);
        }
    }
}
