using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DaggerData", menuName = "Weapon System/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public GameObject weaponPerfab;

    public int baseDamage;
    public IntStat damage;

    public int baseRotateSpeed;
    public IntStat rotateSpeed;

    public float FloatMass;
    public FloatStat mass;

    public float radius;

    public List<Trait> traits;
    public WeaponData()
    {
        damage = new IntStat(baseDamage);
        rotateSpeed = new IntStat(baseRotateSpeed);
        mass = new FloatStat(FloatMass);
     }
}