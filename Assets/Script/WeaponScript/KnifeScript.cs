using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : WeaponScript
{
    public float skillCD;//技能cd
    public float shorterCD;//闪避成功后的CD
    public float addSkillSpeed;
    public float skillDuration;
    public float skillMoveForce;

    public int enhancedDamage;//增强的伤害

    public bool isShadowing;//是否技能正在使用

    public bool canUseSkill;//是否可以使用技能
    public bool ifDodge;//是否闪避了攻击

    private SpriteRenderer knifeRenderer;

    public GameObject knifeEffectPrefab;
    public override void Start()
    {
        base.Start();
        selfball.GetComponent<BallScript>().beforeTakeDamage.AddListener(OnBallDamaged);
    }
    protected override void InitWeaponStats()
    {
        //基本属性
        originDamage = 15;
        damage = originDamage;
        radius = 0.55f;
        rotateSpeed = 400f;
        enhancedDamage = 45;
        //特殊属性
        addSkillSpeed = 3f;
        skillCD = 10f;
        shorterCD = 4f;
        skillDuration = 0.5f;
        skillMoveForce = 15f;
        canUseSkill = true;
        ifDodge = false;
        //组件
        knifeRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayer && canUseSkill)
        {
            StartCoroutine(ShadowRetribution());
        }
    }
    void FixedUpdate()
    {
        UpdatePosition();
    }
    IEnumerator ShadowRetribution()
    {
        BallScript selfBallScript = selfball.GetComponent<BallScript>();
        
        canUseSkill = false;
        isShadowing = true;
        selfBallScript.maxSpeedStat.AddFlat(addSkillSpeed);//改最大速度
        selfBallScript.moveForceStat.AddFlat(skillMoveForce);//改加速度
        StartCoroutine(selfBallScript.Invincibility(skillDuration));//无敌闪烁
        StartCoroutine(FadeWeaponAndCollision(skillDuration));//武器消失，取消与弹射物的碰撞

        yield return new WaitForSeconds(skillDuration);
        isShadowing = false;
        selfBallScript.maxSpeedStat.AddFlat(-addSkillSpeed);//改回来
        selfBallScript.moveForceStat.AddFlat(-skillMoveForce);//改回来
        if(ifDodge)
        {
            damage = enhancedDamage;

            GameObject effect = Instantiate(knifeEffectPrefab,transform.position,Quaternion.identity);//粒子效果
            effect.transform.SetParent(transform);//设置匕首为父物体

            Destroy(effect,4f);
            yield return new WaitForSeconds(shorterCD);
            damage = originDamage;
            canUseSkill = true;
            ifDodge = false;
        }
        else
        {
            yield return new WaitForSeconds(skillCD);
            canUseSkill = true;
            ifDodge = false;
        }
    }
    private void OnBallDamaged(DamageInfo damageInfo)
    {
        if (!isShadowing) return;
        if (damageInfo.wasDodged == true && damageInfo.isProjectile == true)
        {
            ifDodge = true;
        }
    }
    IEnumerator FadeWeaponAndCollision(float skillDuration)
    {
        float endTime = Time.time+ skillDuration;
        Color c = knifeRenderer.color;
        while (isShadowing)
        {
            //武器变透明
            c.a = (endTime - Time.time) / skillDuration;
            knifeRenderer.color = c;
            yield return null;

            //取消武器碰撞和球体对弹射物的碰撞
            GetComponent<Collider2D>().enabled = false;

        }
        c.a = 1;
        //变回来
        knifeRenderer.color = c;
        GetComponent<Collider2D>().enabled = true;
    }
    public void UpdatePosition()
    {
        Rotate();//旋转
        Vector3 dir = (transform.position - ballPos.position).normalized;//计算方向
        weaponPos = ballPos.position + dir * radius;//计算位置
        transform.position = weaponPos;//更新位置
    }
}