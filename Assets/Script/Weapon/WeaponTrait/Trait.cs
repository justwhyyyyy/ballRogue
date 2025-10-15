using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trait : MonoBehaviour
{
    public bool isPassive;
    public abstract void Effect(WeaponData recivedWeapondata);
}