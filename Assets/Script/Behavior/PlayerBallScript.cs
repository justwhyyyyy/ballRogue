using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBallScript : BallScript
{
    public Vector2 velocity;
    public string ballName;

    private Rigidbody2D ballRb;//����������

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
        ballRb = GetComponent<Rigidbody2D>();//��ȡ����������
    }
    void FixedUpdate()
    {
        UpdatePosition();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))//ÿ֡����Ƿ���E��
        {
            TryRoll();
        }
    }
    void TryRoll()//��������Ƿ����35�����Ҽ��cd
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
        if (totalMess.GetValue() <= 30)//�����������30������ʱ�޵�
        {
            StartCoroutine(Invincibility(rollDuration));
        }
        Vector2 dir = ballRb.velocity.normalized;
        if(dir == Vector2.zero)
            dir = Vector2.right;

        ballRb.AddForce(dir * rollForce, ForceMode2D.Impulse);//����������

        yield return new WaitForSeconds(rollDuration);
        isRolling = false;
        StartCoroutine(CoolDown(() => canRoll = true, rollCD));//��ʼ��CD
    }
    IEnumerator CoolDown(Action onFinish,float CD)//���κι��ܵ�cd,��һ��������bool������true�ķ���
    {
        yield return new WaitForSeconds(CD);
        onFinish?.Invoke();
    }
    private void UpdatePosition()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;//���ݷ���������봴��һ������Ϊһ�ķ�������
        if (input != Vector2.zero )
        {
            Vector2 currentVelocity = ballRb.velocity;
            if (ballRb.velocity.magnitude > maxSpeedStat.GetValue())//������٣�ֻ�������ٶȷ���ֱ����
            {
                Vector2 velocityDir = currentVelocity.normalized;//�ٶȷ���
                float inputAlongVelocityDir = Vector2.Dot(input, velocityDir);//������ͶӰ����
                Vector2 verticalInput = input - inputAlongVelocityDir * velocityDir;

                ballRb.AddForce(verticalInput * moveForceStat.GetValue());//�����������������������
            }
            else
            {
                ballRb.AddForce(input * moveForceStat.GetValue());//��������
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
            Vector2 normal = collision.contacts[0].normal;//contacts[0]����ײ�㣬.normal��ʾ����
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;//���ݷ��߷���
            ballRb.velocity = moveDirection * currentSpeed;
        }
    }
}