using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAxeScript : WeaponScript
{
    public float slashCD;//横扫CD
    public float slashSpeed;//横扫速度
    public float slashDuration;//横扫持续时间
    public int slashDamage;//横扫伤害

    public bool isSlashing;//是否正在横扫
    public bool canSlash;//是否可以横扫
    public float slashTimer;//内置计时器

    public override void Start()
    {
        base.Start();
        if (isPlayer)
        {
        }
        else
        {
            StartCoroutine(AutoSlashRoutine());
        }
    }
    protected override void InitWeaponStats()
    {
        canSlash = true;
        damage = 15;
        radius = 1f;
        rotateSpeed = 150f;
        slashCD = 5f;
        slashDamage = 30;
        slashSpeed = 1000f;
        slashDuration = 0.2f;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSlash && isPlayer)
        {
            StartCoroutine(SlashOnce());
        }
    }
    void FixedUpdate()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        Rotate();

        Vector3 dir = (transform.position - ballPos.position).normalized;//计算方向
        weaponPos = ballPos.position + dir * radius;//计算位置
        transform.position = weaponPos;//更新位置
    }
    IEnumerator SlashOnce()
    {
        canSlash = false;
        isSlashing = true;//关闭接口
        slashTimer = 0;//重置计时
        originRotateSpeed = rotateSpeed;//调整参数实现挥砍效果
        originDamage = damage;
        rotateSpeed = slashSpeed;
        damage = slashDamage;
        yield return new WaitForSeconds(slashDuration);//时间结束改回去
        rotateSpeed = originRotateSpeed;
        damage = originDamage;
        isSlashing = false;
        yield return new WaitForSeconds(slashCD);//跑cd
        canSlash = true;
    }
    IEnumerator AutoSlashRoutine()
    {
        while (true)
        {
            if (canSlash)
            {
                StartCoroutine(SlashOnce());
            }
            else
            {
                yield return null;
            }
        }
    }
}