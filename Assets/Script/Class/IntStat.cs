using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntStat : Stat
{
    // ====== 基础值（不会变动，角色本身的原始属性） ======
    public float baseValue;
    // ====== 最终值（每帧重新计算） ======
    private int finalValue;
    // ====== 修正值 ======
    public float addModifier;
    public float mulModifier;

    public IntStat(int baseValue)
    {
        this.baseValue = baseValue;
        this.addModifier = 0;
        this.mulModifier = 0f;
        Recalculate();
    }
    public void AddMultiplier(float value)
    {
        mulModifier += value;
        Recalculate();
    }
    public void AddFlat(float value)
    {
        addModifier += value;
        Recalculate();
    }
    public void Recalculate()
    {
        float value = baseValue;
        value *= (1f + mulModifier);//x
        value += addModifier;//+
        finalValue = Mathf.RoundToInt(value);//四舍五入
        if (finalValue < 1) finalValue = 1;//不够凑1
    }
    public int GetValue()
    {
        Recalculate();
        return finalValue; 
    }
}
