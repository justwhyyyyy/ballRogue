using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trait : MonoBehaviour
{
    [HideInInspector] public TraitInfo info;
    protected bool isActive = false;
    public WeaponData weaponData;
    public virtual void Init(TraitInfo traitInfo,WeaponData recivedWeapondata)
    {
        info = traitInfo;
        weaponData = recivedWeapondata;
    }
    public virtual void Activate()
    {
        if (isActive) return;
        isActive = true;
        OnActivate();

        if (info.useCoroutine)
            StartCoroutine(TraitCoroutine());

        if (info.useEvent)
            RegisterEvents();
    }

    public virtual void Deactivate()
    {
        if (!isActive) return;
        isActive = false;

        StopAllCoroutines();
        UnregisterEvents();
        OnDeactivate();
    }

    protected virtual void Update()
    {
        if (isActive && info != null && info.useUpdate)
            OnUpdate();
    }

    // 可由子类重写
    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }
    protected virtual void OnUpdate() { }
    protected virtual IEnumerator TraitCoroutine() { yield break; }
    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }
}