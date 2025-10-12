using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceScript : WeaponScript
{
    public float thrustCD;//戳刺cd
    public float thrustLength;//戳刺距离
    public float thrustDuration;//每次戳刺所需时间
    public int thrustDamage;//戳刺伤害

    private bool canThrust = true;
    private bool isThrusting = false;//是否正在戳刺
    private float thrustTimer = 0f;//戳刺内置计时器
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

        Vector3 dir = (transform.position - ballPos.position).normalized;//计算戳刺方向
        weaponPos = ballPos.position + dir * radius;//计算戳刺前的位置

        if (isThrusting)
        {
            thrustTimer += Time.fixedDeltaTime * actionSpeedMultiplier;
            float t = thrustTimer / thrustDuration;//作为正弦函数的x变量
            if (t > 1f) t = 1f;
            float thrustOffset = Mathf.Sin(t * Mathf.PI) * thrustLength;//thrustLen gth为振幅
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

        while (thrustTimer < thrustDuration)//检测戳刺是否完成
        {
            yield return null;
        }
        isThrusting = false;
        yield return new WaitForSeconds(thrustCD);//等待cd
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