using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpellAttribute
{
    None, FireSlash, ThunderSlash
}

public class SpellEffect
{
    public Player Owner;

    public virtual void ActivateEffect(OneCardManager oneCard, List<int> targets)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }
}
