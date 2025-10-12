using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedBuff : BaseBuff
{
    public int damagePerTick;//ÿ����ɵ��˺�
    public int frameCounter;//��֡��
    public int tickInterval;//50֡�£�����֡һ��
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
        spriteRenderer = buffTarget.GetComponent<SpriteRenderer>();//��������ɫ
        originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.Lerp(originalColor, Color.red, 0.5f);//0.5��ɫ

        duration = 100;//����
        stack = 1;
        pastTime = 0;
        damagePerTick = 1;
        tickInterval = 10;
    }
    public override void UpdateEffect(BallScript selfBall)
    {
        frameCounter++;//��֡
        pastTime++;//��ʱ
        if (frameCounter >= tickInterval)
        {
            //TODO:�˺����߼����и��õ��뷨,��Ч�ĳɽ����
            //����˺�
            selfBall.ApplyDotDamage(damagePerTick,buffSource);
            //�˺�UI
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