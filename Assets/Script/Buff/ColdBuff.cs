using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdBuff : BaseBuff
{
    //球相关
    public float moveSpeedMultiplier;//移动速度减缓的百分比，影响的数值有最大速度，当前速度，加速度
    //武器相关
    public float weaponActionSpeedMultiplier;//武器行动减缓的百分比，要在各个武器跑cd的update里面加一个系数参数

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
        spriteRenderer.color = Color.Lerp(originalColor, Color.blue, 0.5f);//0.5红色

        //修正速度
        BallScript targetBallScript = buffTarget.GetComponent<BallScript>();
        targetBallScript.moveForceStat.AddMultiplier(-moveSpeedMultiplier);//加速度
        targetBallScript.maxSpeedStat.AddMultiplier(-moveSpeedMultiplier);//最大速度
        targetBallScript.GetComponent<Rigidbody2D>().velocity *= moveSpeedMultiplier;//当前速度
        //修正武器速度
        foreach(WeaponScript weaponScript in buffTarget.GetComponentsInChildren<WeaponScript>())
        {
            weaponScript.actionSpeedMultiplier -= weaponActionSpeedMultiplier;
        }
        duration = 200; //4秒
    }
    public override void UpdateEffect(BallScript selfBall)
    {
        //在Init赋值，end删除
        pastTime++;//计时
    }
    public override void Renew()
    {
        duration += 200;
    }
    public override void End()
    {
        BallScript targetBallScript = buffTarget.GetComponent<BallScript>();
        targetBallScript.moveForceStat.AddMultiplier(moveSpeedMultiplier);//加速度
        targetBallScript.maxSpeedStat.AddMultiplier(moveSpeedMultiplier);//最大速度
        //修正武器速度
        foreach (WeaponScript weaponScript in buffTarget.GetComponentsInChildren<WeaponScript>())
        {
            weaponScript.actionSpeedMultiplier += weaponActionSpeedMultiplier;
        }
        spriteRenderer.color = originalColor;
        UnityEngine.Object.Destroy(effectInstance);
    }
}