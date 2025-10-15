using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpTrait : Trait
{
    public WeaponData weaponData;
    public int level;
    public override void Effect(WeaponData recivedWeapondata)
    {
        weaponData = recivedWeapondata;
        recivedWeapondata.damage.AddMultiplier(0.2f * level);
    }
}