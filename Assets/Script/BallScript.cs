using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float invincibleTime = 1f;//�ܻ��޵�ʱ�䳤��
    public bool isInvincible = false;//�Ƿ��޵�
    public bool ifDodgeByInvincible = false;//�Ƿ����޵�ʱ���ڶ�����˺�
    //��������
    public IntStat maxHPStat;
    public int currentHP;

    public FloatStat maxSpeedStat;
    public FloatStat moveForceStat;

    public FloatStat totalMess;
    //����ϵͳ
    public int maxWeaponCount = 4;
    public List<WeaponScript> weapons = new List<WeaponScript>();
    //����
    public team selfTeam;

    private SpriteRenderer sr;

    public GameObject damageTextPrefab;

    public DamageEvent beforeTakeDamage = new DamageEvent();
    public DamageEvent afterTakeDamage = new DamageEvent();
    
    public BallScript(int baseHP = 100, float baseSpeed = 3f, float baseMoveForce = 15f,float mess = 20f)//����
    {
        maxHPStat = new IntStat(baseHP);
        maxSpeedStat = new FloatStat(baseSpeed); // ��������ʾ�ٶȣ��������100
        moveForceStat = new FloatStat(baseMoveForce);
        totalMess = new FloatStat(mess);
    }
    public virtual void WeaponCauseDamage(int damage, GameObject damageTaker,GameObject theAttackWeapon)
    {
        damageTaker.GetComponent<BallScript>().TakeDamage(damage,theAttackWeapon);
    }
    public virtual void TakeDamage(int damage,GameObject attackThing)//attackThing�ǹ����ľ���GameObject
        //TODO:Renderer���������⣬�޵�ʱ��ͱ�����spriteRenderer���ӳ�������
    {
        bool ifProjectile;
        if(attackThing.CompareTag("projectile"))
            ifProjectile = true;
        else
            ifProjectile= false;
        if(isInvincible)//����޵У���return
        {
            beforeTakeDamage.Invoke(new DamageInfo(attackThing,true,ifProjectile,damage));
            return;
        }
        beforeTakeDamage.Invoke(new DamageInfo(attackThing, false, ifProjectile, damage));
        currentHP -= damage;//��Ѫ

        afterTakeDamage.Invoke(new DamageInfo(attackThing,false,ifProjectile,damage));
        //���˺��¼�
        showFloatingDamage(damage);//Ʈ����
        if(damage >= 15) TimeManager.Instance.SlowTime(0.1f, 0.4f);//�˺����ڵ���15��ʱ�����

        ifDie();//�������

        StartCoroutine(Invincibility(invincibleTime));//�����޵�
    }
    public void ApplyDotDamage(int damage, GameObject attackThing)
    {
        currentHP -= damage;

        showFloatingDamage(damage);
        
        afterTakeDamage.Invoke(new DamageInfo(attackThing, false, false, damage));

        ifDie();
    }
    void showFloatingDamage(int damage)//Ʈ�˺�����
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.8f;//���ֲ���λ��
        GameObject damageText = Instantiate(damageTextPrefab,spawnPos,Quaternion.identity);

        FloatingTextScript ft = damageText.GetComponent<FloatingTextScript>();
        ft.SetText(damage , Color.red);
    }
    public void ifDie()
    {
        if (currentHP <= 0)//�������
        {
            Die();
        }
    }
    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        Destroy(gameObject);
    }
    public IEnumerator Invincibility(float duration)//�޵�Э���Լ��޵��߼�
    {
        isInvincible = true;
        StartCoroutine(FlashDuringInvincibility(duration));
        yield return new WaitForSeconds(duration);
        //���ò���
        isInvincible = false;
        ifDodgeByInvincible = false;
    }
    public IEnumerator FlashDuringInvincibility(float duration)//�޵���˸Э��
    {
        sr = GetComponent<SpriteRenderer>();
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
            yield return new WaitForSeconds(0.1f);
            c.a = 1f;
            sr.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    } 
}