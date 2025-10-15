using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntStat : Stat
{
    // ====== ����ֵ������䶯����ɫ�����ԭʼ���ԣ� ======
    public float baseValue;
    // ====== ����ֵ��ÿ֡���¼��㣩 ======
    private int finalValue;
    // ====== ����ֵ ======
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
        finalValue = Mathf.RoundToInt(value);//��������
        if (finalValue < 1) finalValue = 1;//������1
    }
    public int GetValue()
    {
        Recalculate();
        return finalValue; 
    }
}
