using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsManager : MonoBehaviour
{
    public static TargetsManager Instance;
    public List<int> Targets = new List<int>();

    private void Awake()
    {
        Instance = this;
    }
}
