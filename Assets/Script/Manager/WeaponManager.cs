using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public List<WeaponData> weapons = new List<WeaponData>();
    public List<Trait> traits = new List<Trait>();
    public void Awake()
    {
        instance = this;
    }
    public WeaponComponent InitWeapon(WeaponData weaponType, List<Trait> itsTraits)
    {
        GameObject weapon = Instantiate(weaponType.weaponPerfab);
        WeaponComponent addedWeaponComponent = weapon.AddComponent<WeaponComponent>();
        addedWeaponComponent.weaponData = weaponType;
        addedWeaponComponent.traits = new List<Trait>(itsTraits);
        return addedWeaponComponent;
    }
    public WeaponData NameToData(string seekedWeaponName)
    {
        foreach(WeaponData weaponData in weapons)
        {
            if(weaponData.weaponName == seekedWeaponName)
            {
                return weaponData;
            }
        }
        return null;
    }
}