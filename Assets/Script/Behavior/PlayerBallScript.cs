using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBallScript : BallScript
{
    public Vector2 velocity;
    public string ballName;

    private Rigidbody2D ballRb;//球的物理组件

    public bool canRoll = true;
    public bool isRolling = false;
    public float rollCD = 5;

    float rollForce = 8f;
    float rollDuration = 0.8f;
    public PlayerBallScript() 
    {
        
    }
    private void Awake()
    {
        maxHPStat.baseValue = 100;
        currentHP = maxHPStat.GetValue();
        selfTeam = team.Player;
        ballRb = GetComponent<Rigidbody2D>();//获取球的物理组件
    }
    void FixedUpdate()
    {
        UpdatePosition();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))//每帧检查是否按下E键
        {
            TryRoll();
        }
    }
    void TryRoll()//检查质量是否低于35，并且检查cd
    {
        if (canRoll&&totalMess.GetValue() <= 35)
        {
            StartCoroutine(RollCoroutine());
        }
    }
    IEnumerator RollCoroutine()
    {
        isRolling = true;
        canRoll = false;
        if (totalMess.GetValue() <= 30)//如果质量低于30，翻滚时无敌
        {
            StartCoroutine(Invincibility(rollDuration));
        }
        Vector2 dir = ballRb.velocity.normalized;
        if(dir == Vector2.zero)
            dir = Vector2.right;

        ballRb.AddForce(dir * rollForce, ForceMode2D.Impulse);//给翻滚的力

        yield return new WaitForSeconds(rollDuration);
        isRolling = false;
        StartCoroutine(CoolDown(() => canRoll = true, rollCD));//开始跑CD
    }
    IEnumerator CoolDown(Action onFinish,float CD)//跑任何功能的cd,第一个参数是bool变量变true的方法
    {
        yield return new WaitForSeconds(CD);
        onFinish?.Invoke();
    }
    private void UpdatePosition()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;//根据方向键的输入创建一个长度为一的方向向量
        if (input != Vector2.zero )
        {
            Vector2 currentVelocity = ballRb.velocity;
            if (ballRb.velocity.magnitude > maxSpeedStat.GetValue())//如果超速，只给与与速度方向垂直的力
            {
                Vector2 velocityDir = currentVelocity.normalized;//速度方向
                float inputAlongVelocityDir = Vector2.Dot(input, velocityDir);//点乘算出投影长度
                Vector2 verticalInput = input - inputAlongVelocityDir * velocityDir;

                ballRb.AddForce(verticalInput * moveForceStat.GetValue());//根据这个方向向量给与球力
            }
            else
            {
                ballRb.AddForce(input * moveForceStat.GetValue());//正常给力
            }
        }
        velocity = ballRb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Vector2 moveDirection = velocity;
            float currentSpeed = velocity.magnitude;
            Vector2 normal = collision.contacts[0].normal;//contacts[0]是碰撞点，.normal表示法线
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;//根据法线反弹
            ballRb.velocity = moveDirection * currentSpeed;
        }
    }
}