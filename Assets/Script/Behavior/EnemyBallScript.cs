using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallScript : BallScript
{

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Awake()
    {
        selfTeam = team.Enemy;
        maxHPStat.baseValue = 100;
        currentHP = maxHPStat.GetValue();
        rb = GetComponent<Rigidbody2D>();

        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        rb.velocity = moveDirection * maxSpeedStat.GetValue();
    }
    private void FixedUpdate()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        if(rb.velocity.magnitude < maxSpeedStat.GetValue())
        {
            rb.AddForce(rb.velocity.normalized * moveForceStat.GetValue());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Vector2 normal = collision.contacts[0].normal;//contacts[0]����ײ�㣬.normal��ʾ����
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;//���ݷ��߷���
            rb.velocity = moveDirection * maxSpeedStat.GetValue();
        }
    }
}
