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
    public TreasureVisual TreasureVisual;

    public PlayerPortraitVisual Portrait;

    public Transform PortraitPosition;
    public Transform InitialPortraitPosition;

    private void Awake()
    {

    }

}
