using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : ProjectileScript
{
    //��������ж��ּ���дһ�����ĳ�����
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("weapon") && collision.gameObject != owner)
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("ball"))
        {
            BallScript hitBall = collision.GetComponent<BallScript>();
            if (hitBall.selfTeam != ownerTeam)
            {
                hitBall.TakeDamage(damageFunction(projectileRb.velocity.magnitude), this.gameObject);
                Destroy(gameObject);
            }
        }
    }
    public int damageFunction(float speed)
    {
        int damage = (int)speed;
        return damage;
    }
}