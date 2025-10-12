using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public GameObject source;
    public bool wasDodged;
    public bool isProjectile;
    public int damage;
    public DamageInfo(GameObject source, bool wasDodged, bool isProjectile, int damage)
    {
        this.source = source;
        this.wasDodged = wasDodged;
        this.isProjectile = isProjectile;
        this.damage = damage;
    }
}