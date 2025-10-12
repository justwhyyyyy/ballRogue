using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdBuff : BaseBuff
{
    //�����
    public float moveSpeedMultiplier;//�ƶ��ٶȼ����İٷֱȣ�Ӱ�����ֵ������ٶȣ���ǰ�ٶȣ����ٶ�
    //�������
    public float weaponActionSpeedMultiplier;//�����ж������İٷֱȣ�Ҫ�ڸ���������cd��update�����һ��ϵ������

    SpriteRenderer spriteRenderer;

    Color originalColor;

    public GameObject effectPrefab;
    public ColdBuff()
    {
        buffName = "Cold";
        moveSpeedMultiplier = 0.5f;
        weaponActionSpeedMultiplier = 0.5f;
    }
    public override void Init()
    {
        spriteRenderer = buffTarget.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.Lerp(originalColor, Color.blue, 0.5f);//0.5��ɫ

        //�����ٶ�
        BallScript targetBallScript = buffTarget.GetComponent<BallScript>();
        targetBallScript.moveForceStat.AddMultiplier(-moveSpeedMultiplier);//���ٶ�
        targetBallScript.maxSpeedStat.AddMultiplier(-moveSpeedMultiplier);//����ٶ�
        targetBallScript.GetComponent<Rigidbody2D>().velocity *= moveSpeedMultiplier;//��ǰ�ٶ�
        //���������ٶ�
        foreach(WeaponScript weaponScript in buffTarget.GetComponentsInChildren<WeaponScript>())
        {
            weaponScript.actionSpeedMultiplier -= weaponActionSpeedMultiplier;
        }
        duration = 200; //4��
    }
    public override void UpdateEffect(BallScript selfBall)
    {
        //��Init��ֵ��endɾ��
        pastTime++;//��ʱ
    }
    public override void Renew()
    {
        duration += 200;
    }
    public override void End()
    {
        BallScript targetBallScript = buffTarget.GetComponent<BallScript>();
        targetBallScript.moveForceStat.AddMultiplier(moveSpeedMultiplier);//���ٶ�
        targetBallScript.maxSpeedStat.AddMultiplier(moveSpeedMultiplier);//����ٶ�
        //���������ٶ�
        foreach (WeaponScript weaponScript in buffTarget.GetComponentsInChildren<WeaponScript>())
        {
            weaponScript.actionSpeedMultiplier += weaponActionSpeedMultiplier;
        }
        spriteRenderer.color = originalColor;
        UnityEngine.Object.Destroy(effectInstance);
    }
}