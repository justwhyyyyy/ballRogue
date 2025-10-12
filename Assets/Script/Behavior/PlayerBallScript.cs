using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBallScript : BallScript
{

    public Vector2 velocity;

    public string ballName;

    private Rigidbody2D ballRb;//����������
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
    private void UpdatePosition()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;//���ݷ���������봴��һ������Ϊһ�ķ�������
        if (input != Vector2.zero)
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