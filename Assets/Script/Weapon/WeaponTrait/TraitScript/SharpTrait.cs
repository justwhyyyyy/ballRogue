using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpTrait : Trait
{
    protected override void OnActivate()
    {
        weaponData.damage.AddMultiplier(info.level * 0.2f);
    }
}