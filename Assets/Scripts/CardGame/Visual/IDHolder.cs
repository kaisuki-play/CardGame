using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IDHolder : MonoBehaviour
{

    public int UniqueID;
    private static List<IDHolder> _allIDHolders = new List<IDHolder>();

    void Awake()
    {
        _allIDHolders.Add(this);
    }

    public static GameObject GetGameObjectWithID(int ID)
    {
        foreach (IDHolder i in _allIDHolders)
        {
            if (i.UniqueID == ID)
                return i.gameObject;
        }
        return null;
    }

    public static void ClearIDHoldersList()
    {
        _allIDHolders.Clear();
    }
}
