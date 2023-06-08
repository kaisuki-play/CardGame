using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum AreaPosition { Main, Opponent1, Opponent2, Opponent3, Opponent4, Opponent5 }

public class PlayerArea : MonoBehaviour
{
    public AreaPosition Owner;

    public HandVisual HandVisual;
    public JudgementVisual JudgementVisual;
    public EquipmentVisaul EquipmentVisaul;

    public PlayerPortraitVisual Portrait;

    public Transform PortraitPosition;
    public Transform InitialPortraitPosition;

    private void Awake()
    {

    }

    private bool _showEndTurnButton = false;
    public bool ShowEndTurnButton
    {
        get
        {
            return _showEndTurnButton;
        }

        set
        {
            _showEndTurnButton = value;
        }
    }

    private bool _showSelectButton = false;
    public bool ShowSelectButton
    {
        get
        {
            return _showSelectButton;
        }

        set
        {
            _showSelectButton = value;
            Portrait.SelectButton.gameObject.SetActive(value);
        }
    }

    private bool _showSelect2Button = false;
    public bool ShowSelect2Button
    {
        get
        {
            return _showSelect2Button;
        }

        set
        {
            _showSelect2Button = value;
            Portrait.SelectButton2.gameObject.SetActive(value);
        }
    }


}
