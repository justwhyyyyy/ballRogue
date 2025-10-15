using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceScript : WeaponScript
{
    public float thrustCD;//����cd
    public float thrustLength;//���̾���
    public float thrustDuration;//ÿ�δ�������ʱ��
    public int thrustDamage;//�����˺�

    private bool canThrust = true;
    private bool isThrusting = false;//�Ƿ����ڴ���
    private float thrustTimer = 0f;//�������ü�ʱ��
    public override void Start()
    {
        base.Start();
        if (isPlayer)
        {
        }
        else
        {
            StartCoroutine(AutoThrustRoutine());
        }
    }
    protected override void InitWeaponStats()
    {
        damage = 10;
        radius = 1.3f;
        rotateSpeed = 150f;

        thrustDamage = 20;
        thrustCD = 3f;
        thrustLength = 1f;
        thrustDuration = 0.3f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canThrust && isPlayer)
        {
            StartCoroutine(ThrustOnce());
        }
    }
    void FixedUpdate()
    {
        UpdatePosition();

    }
    public void UpdatePosition()
    {
        Rotate();

        Vector3 dir = (transform.position - ballPos.position).normalized;//������̷���
        weaponPos = ballPos.position + dir * radius;//�������ǰ��λ��

        if (isThrusting)
        {
            thrustTimer += Time.fixedDeltaTime * actionSpeedMultiplier;
            float t = thrustTimer / thrustDuration;//��Ϊ���Һ�����x����
            if (t > 1f) t = 1f;
            float thrustOffset = Mathf.Sin(t * Mathf.PI) * thrustLength;//thrustLen gthΪ���
            transform.position = weaponPos + dir * thrustOffset;
        }
        else
        {
            transform.position = weaponPos;
        }
    }
    IEnumerator ThrustOnce()
    {
        canThrust = false;
        isThrusting = true;
        thrustTimer = 0f;

        while (thrustTimer < thrustDuration)//�������Ƿ����
        {
            yield return null;
        }
        isThrusting = false;
        yield return new WaitForSeconds(thrustCD);//�ȴ�cd
        canThrust = true;
    }
    IEnumerator AutoThrustRoutine()
    {
        while (true)
        {
            if (canThrust)
            {
                StartCoroutine(ThrustOnce());
            }
            else
            {
                yield return null;
            }
        }
    }
}