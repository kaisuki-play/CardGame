using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class IDFactory
{

    private static int _count;

    public static int GetUniqueID()
    {
        // Count++ has to go first, otherwise - unreachable code.
        _count++;
        return _count;
    }

    public static void ResetIDs()
    {
        _count = 0;
    }


}
