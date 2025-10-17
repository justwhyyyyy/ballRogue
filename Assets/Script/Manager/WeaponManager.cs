using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public List<WeaponData> weapons = new List<WeaponData>();
    public List<TraitInfo> traits = new List<TraitInfo>();
    public void Awake()
    {
        instance = this;
    }
    public WeaponComponent InitWeapon(int spawnWeaponIndex, List<TraitInfo> itsTraitsInfo)//根据名字和词条生成武器
    {
        WeaponData weaponType = NameToData(spawnWeaponIndex);
        GameObject weapon = Instantiate(weaponType.weaponPerfab);
        WeaponComponent addedWeaponComponent = weapon.AddComponent<WeaponComponent>();
        addedWeaponComponent.weaponData = weaponType;
        if (itsTraitsInfo == null)//防报错
            addedWeaponComponent.traits = new List<Trait>();
        else
        {
            foreach (TraitInfo traitInfo in itsTraitsInfo)
            {
                AddTraitByInfo(traitInfo, addedWeaponComponent);
            }
        }
        return addedWeaponComponent;
    }
    public WeaponData NameToData(int seekedWeaponNum)//根据序号查找武器类型
    {
        foreach (WeaponData weaponData in weapons)
        {
            if(weaponData.weaponIndex == seekedWeaponNum)
            {
                Debug.Log("findit");
                return weaponData;
            }
        }
        Debug.Log("no");
        return null;
    }
    public Trait AddTraitByInfo(TraitInfo info, WeaponComponent weaponComponent)//通过Info找Trait
    {
        Type traitType = null;

        // 从所有程序集查找 Trait 类
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            traitType = asm.GetType(info.traitName);
            if (traitType != null) break;
        }

        if (traitType == null)
        {
            Debug.LogError($"⚠️ 未找到 Trait 类：{info.traitName}");
            return null;
        }

        Trait newTrait = (Trait)weaponComponent.gameObject.AddComponent(traitType);
        newTrait.Init(info, weaponComponent.weaponData);
        newTrait.Activate();

        weaponComponent.traits.Add(newTrait);
        return newTrait;
    }
}