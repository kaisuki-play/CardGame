using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSource : MonoBehaviour
{
    public static DeckSource Instance;
    public List<CardAsset> Cards = new List<CardAsset>();

    private void Awake()
    {
        Instance = this;
    }
}
