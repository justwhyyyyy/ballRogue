using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : WeaponScript
{
    public float skillCD;//����cd
    public float shorterCD;//���ܳɹ����CD
    public float addSkillSpeed;
    public float skillDuration;
    public float skillMoveForce;

    public int enhancedDamage;//��ǿ���˺�

    public bool isShadowing;//�Ƿ�������ʹ��

    public bool canUseSkill;//�Ƿ����ʹ�ü���
    public bool ifDodge;//�Ƿ������˹���

    private SpriteRenderer knifeRenderer;

    public GameObject knifeEffectPrefab;
    public override void Start()
    {
        base.Start();
        selfball.GetComponent<BallScript>().beforeTakeDamage.AddListener(OnBallDamaged);
    }
    protected override void InitWeaponStats()
    {
        //��������
        originDamage = 15;
        damage = originDamage;
        radius = 0.55f;
        rotateSpeed = 400f;
        enhancedDamage = 45;
        //��������
        addSkillSpeed = 3f;
        skillCD = 10f;
        shorterCD = 4f;
        skillDuration = 0.5f;
        skillMoveForce = 15f;
        canUseSkill = true;
        ifDodge = false;
        //���
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
        selfBallScript.maxSpeedStat.AddFlat(addSkillSpeed);//������ٶ�
        selfBallScript.moveForceStat.AddFlat(skillMoveForce);//�ļ��ٶ�
        StartCoroutine(selfBallScript.Invincibility(skillDuration));//�޵���˸
        StartCoroutine(FadeWeaponAndCollision(skillDuration));//������ʧ��ȡ���뵯�������ײ

        yield return new WaitForSeconds(skillDuration);
        isShadowing = false;
        selfBallScript.maxSpeedStat.AddFlat(-addSkillSpeed);//�Ļ���
        selfBallScript.moveForceStat.AddFlat(-skillMoveForce);//�Ļ���
        if(ifDodge)
        {
            damage = enhancedDamage;

            GameObject effect = Instantiate(knifeEffectPrefab,transform.position,Quaternion.identity);//����Ч��
            effect.transform.SetParent(transform);//����ذ��Ϊ������

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
            //������͸��
            c.a = (endTime - Time.time) / skillDuration;
            knifeRenderer.color = c;
            yield return null;

            //ȡ��������ײ������Ե��������ײ
            GetComponent<Collider2D>().enabled = false;

        }
        c.a = 1;
        //�����
        knifeRenderer.color = c;
        GetComponent<Collider2D>().enabled = true;
    }
    public void UpdatePosition()
    {
        Rotate();//��ת
        Vector3 dir = (transform.position - ballPos.position).normalized;//���㷽��
        weaponPos = ballPos.position + dir * radius;//����λ��
        transform.position = weaponPos;//����λ��
    }
}