using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float invincibleTime = 1f;//受击无敌时间长度
    public bool isInvincible = false;//是否无敌
    public bool ifDodgeByInvincible = false;//是否在无敌时间内躲过了伤害
    //基础属性
    public IntStat maxHPStat;
    public int currentHP;

    public FloatStat maxSpeedStat;
    public FloatStat moveForceStat;

    public FloatStat totalMess;
    //武器系统
    public int maxWeaponCount = 4;
    public List<WeaponScript> weapons = new List<WeaponScript>();
    //队伍
    public team selfTeam;

    private SpriteRenderer sr;

    public GameObject damageTextPrefab;

    public DamageEvent beforeTakeDamage = new DamageEvent();
    public DamageEvent afterTakeDamage = new DamageEvent();
    
    public BallScript(int baseHP = 100, float baseSpeed = 3f, float baseMoveForce = 15f,float mess = 20f)//参数
    {
        maxHPStat = new IntStat(baseHP);
        maxSpeedStat = new FloatStat(baseSpeed); // 用整数表示速度，后面除以100
        moveForceStat = new FloatStat(baseMoveForce);
        totalMess = new FloatStat(mess);
    }
    public virtual void WeaponCauseDamage(int damage, GameObject damageTaker,GameObject theAttackWeapon)
    {
        damageTaker.GetComponent<BallScript>().TakeDamage(damage,theAttackWeapon);
    }
    public virtual void TakeDamage(int damage,GameObject attackThing)//attackThing是攻击的具体GameObject
        //TODO:Renderer可能有问题，无敌时间和冰冻的spriteRenderer叠加出现问题
    {
        bool ifProjectile;
        if(attackThing.CompareTag("projectile"))
            ifProjectile = true;
        else
            ifProjectile= false;
        if(isInvincible)//如果无敌，则return
        {
            beforeTakeDamage.Invoke(new DamageInfo(attackThing,true,ifProjectile,damage));
            return;
        }
        beforeTakeDamage.Invoke(new DamageInfo(attackThing, false, ifProjectile, damage));
        currentHP -= damage;//扣血

        afterTakeDamage.Invoke(new DamageInfo(attackThing,false,ifProjectile,damage));
        //受伤后事件
        showFloatingDamage(damage);//飘数字
        if(damage >= 15) TimeManager.Instance.SlowTime(0.1f, 0.4f);//伤害大于等于15，时间减速

        ifDie();//检测死亡

        StartCoroutine(Invincibility(invincibleTime));//触发无敌
    }
    public void ApplyDotDamage(int damage, GameObject attackThing)
    {
        currentHP -= damage;

        showFloatingDamage(damage);
        
        afterTakeDamage.Invoke(new DamageInfo(attackThing, false, false, damage));

        ifDie();
    }
    void showFloatingDamage(int damage)//飘伤害数字
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.8f;//数字产生位置
        GameObject damageText = Instantiate(damageTextPrefab,spawnPos,Quaternion.identity);

        FloatingTextScript ft = damageText.GetComponent<FloatingTextScript>();
        ft.SetText(damage , Color.red);
    }
    public void ifDie()
    {
        if (currentHP <= 0)//检测死亡
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
    public IEnumerator Invincibility(float duration)//无敌协程以及无敌逻辑
    {
        isInvincible = true;
        StartCoroutine(FlashDuringInvincibility(duration));
        yield return new WaitForSeconds(duration);
        //重置参数
        isInvincible = false;
        ifDodgeByInvincible = false;
    }
    public IEnumerator FlashDuringInvincibility(float duration)//无敌闪烁协程
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