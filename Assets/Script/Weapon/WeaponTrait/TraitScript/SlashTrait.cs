using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTrait : Trait
{
    public bool canSlash = true;
    public float slashTime = 0.2f;
    public float slashCD = 5f;
    protected override void OnActivate()
    {
    }
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TrySlash();
        }
    }
    public void TrySlash()
    {
        if(canSlash)
        {

        }
    }
    IEnumerator Slash()
    {
        canSlash = false;
        weaponData.rotateSpeed.AddMultiplier(5);
        weaponData.damage.AddMultiplier(0.5f);
        yield return new WaitForSeconds(slashTime);
        weaponData.rotateSpeed.AddMultiplier(-5);
        weaponData.damage.AddMultiplier(-0.5f);
        StartCoroutine(CoolDown(() => canSlash = true,slashCD));
    }
    IEnumerator CoolDown(Action onFinish, float CD)//跑任何功能的cd,第一个参数是bool变量变true的方法
    {
        yield return new WaitForSeconds(CD);
        onFinish?.Invoke();
    }
}
