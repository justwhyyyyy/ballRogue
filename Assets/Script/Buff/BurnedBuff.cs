using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedBuff : BaseBuff
{
    public int damagePerTick;//每跳造成的伤害
    public int frameCounter;//数帧数
    public int tickInterval;//50帧下，多少帧一跳
    SpriteRenderer spriteRenderer;
    private Color originalColor;

    public GameObject effectPrefab;
    public GameObject burnedEffect;
    public BurnedBuff()
    {
        buffName = "Burned";
    }
    public override void Init()
    {
        Debug.Log("Burned!");
        spriteRenderer = buffTarget.GetComponent<SpriteRenderer>();//改球体颜色
        originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.Lerp(originalColor, Color.red, 0.5f);//0.5红色

        duration = 100;//两秒
        stack = 1;
        pastTime = 0;
        damagePerTick = 1;
        tickInterval = 10;
    }
    public override void UpdateEffect(BallScript selfBall)
    {
        frameCounter++;//数帧
        pastTime++;//计时
        if (frameCounter >= tickInterval)
        {
            //TODO:伤害的逻辑我有更好的想法,特效改成渐变的
            //造成伤害
            selfBall.ApplyDotDamage(damagePerTick,buffSource);
            //伤害UI
            frameCounter = 0;
        }
    }
    public override void Renew()
    {
        duration += 80;
    }
    public override void End()
    {
        spriteRenderer.color = originalColor;
        UnityEngine.Object.Destroy(effectInstance);
    }
}