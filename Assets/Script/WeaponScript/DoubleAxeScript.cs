using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAxeScript : WeaponScript
{
    public float slashCD;//��ɨCD
    public float slashSpeed;//��ɨ�ٶ�
    public float slashDuration;//��ɨ����ʱ��
    public int slashDamage;//��ɨ�˺�

    public bool isSlashing;//�Ƿ����ں�ɨ
    public bool canSlash;//�Ƿ���Ժ�ɨ
    public float slashTimer;//���ü�ʱ��

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

        Vector3 dir = (transform.position - ballPos.position).normalized;//���㷽��
        weaponPos = ballPos.position + dir * radius;//����λ��
        transform.position = weaponPos;//����λ��
    }
    IEnumerator SlashOnce()
    {
        canSlash = false;
        isSlashing = true;//�رսӿ�
        slashTimer = 0;//���ü�ʱ
        originRotateSpeed = rotateSpeed;//��������ʵ�ֻӿ�Ч��
        originDamage = damage;
        rotateSpeed = slashSpeed;
        damage = slashDamage;
        yield return new WaitForSeconds(slashDuration);//ʱ������Ļ�ȥ
        rotateSpeed = originRotateSpeed;
        damage = originDamage;
        isSlashing = false;
        yield return new WaitForSeconds(slashCD);//��cd
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