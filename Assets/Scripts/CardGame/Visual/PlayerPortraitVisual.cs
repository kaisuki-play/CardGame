using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour
{

    public CharacterAsset charAsset;

    [Header("Health References")]
    public GameObject Hearts;
    [Header("Image References")]
    public Image PortraitImage;
    public Image PortraitBackgroundImage;
    public GameObject PortraitGlowImage;
    public GameObject TargetGlowImage;
    public Image DieImage;
    public Text TiesuoText;
    public Text SkillText;
    public Button EndTurnButton;
    public Button SelectButton;
    public Button SelectButton2;
    [Header("Skills")]
    public VerticalLayoutGroup SkillGroup;

    void Awake()
    {

    }

    private bool _highlighted = false;
    public bool Highlighted
    {
        get { return _highlighted; }

        set
        {
            _highlighted = value;
            PortraitGlowImage.SetActive(_highlighted);
        }
    }

    private int _totalHealth;
    public int TotalHealth
    {
        get { return _totalHealth; }

        set
        {
            _totalHealth = value;

            foreach (Health image in Hearts.GetComponentsInChildren<Health>())
            {
                image.transform.SetParent(null);
                Destroy(image);
            }
            for (int i = 0; i < value; i++)
            {
                GameObject heart = GameObject.Instantiate(GlobalSettings.Instance.HealthPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                heart.transform.SetParent(Hearts.transform);
            }
        }
    }

    private int _leftHeath;
    public int LeftHeath
    {
        get { return _leftHeath; }

        set
        {
            if (value > _totalHealth)
                _leftHeath = _totalHealth;
            else if (value < 0)
                _leftHeath = 0;
            else
                _leftHeath = value;

            Debug.Log("Changed mana this turn to: " + _leftHeath);
            Image[] healthImages = Hearts.GetComponentsInChildren<Image>();
            for (int i = 0; i < _totalHealth; i++)
            {
                if (i < _leftHeath)
                {
                    healthImages[i].color = Color.white;
                }
                else
                {
                    healthImages[i].color = Color.gray;
                }
            }

        }
    }

    public void ApplyLookFromAsset()
    {

        TotalHealth = charAsset.MaxHealth;
        LeftHeath = charAsset.MaxHealth;
        PortraitImage.sprite = charAsset.AvatarImage;
        PortraitBackgroundImage.sprite = charAsset.AvatarBGImage;
        PortraitBackgroundImage.color = charAsset.AvatarBGTint;
    }

    public void TakeDamage(int amount, int healthAfter)
    {
        int maxHealth = (TotalHealth > 0 ? TotalHealth : charAsset.MaxHealth);
        if (healthAfter <= maxHealth)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            this.LeftHeath = healthAfter;
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() =>
        {
            if (GlobalSettings.Instance.PlayerInstances.Length == 1)
            {
                TurnManager.Instance.StopTheTimer();
                GlobalSettings.Instance.GameOverPanel.SetActive(true);
            }
        });
    }

    public void ChangeEndTurnButtonText(string text)
    {
        EndTurnButton.GetComponentInChildren<Text>().text = text;
    }

    public void ChangeEndTurnButtonColor(Color color)
    {
        EndTurnButton.GetComponentInChildren<Image>().color = color;
    }

    public void ChangeSelectButtonText(string text)
    {
        SelectButton.GetComponentInChildren<Text>().text = text;
    }

    public void ChangeSelectButtonColor(Color color)
    {
        SelectButton.GetComponentInChildren<Image>().color = color;
    }

    public void ChangeSelectButton2Text(string text)
    {
        SelectButton2.GetComponentInChildren<Text>().text = text;
    }

    public void ChangeSelectButton2Color(Color color)
    {
        SelectButton2.GetComponentInChildren<Image>().color = color;
    }

}
