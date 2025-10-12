using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image healthImage;
    public Sprite[] healthSprites;

    public BallScript selfBallScript;

    public int currentHP;
    public int MaxHp;

    public void Init(BallScript ballScript)
    {
        //绑定球
        selfBallScript = ballScript;
        //获取组件
        healthImage = GetComponent<Image>();
        //初始化
        MaxHp = selfBallScript.maxHPStat.GetValue();
        currentHP = selfBallScript.currentHP;
        selfBallScript.afterTakeDamage.AddListener(OnBallDamaged);
        SetHealth(currentHP);
    }
    private void OnBallDamaged(DamageInfo damageInfo)
    {
        currentHP = selfBallScript.currentHP;
        SetHealth(currentHP);
    }
    public void SetHealth(int HP)//
    {
        HP = Mathf.Clamp(HP, 0, MaxHp);
        int spriteIndex = Mathf.FloorToInt((float)HP / MaxHp * (healthSprites.Length - 1));//roundTOInt四舍五入
        healthImage.sprite = healthSprites[spriteIndex];
    }
}