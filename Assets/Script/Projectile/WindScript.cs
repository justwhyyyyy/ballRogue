using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : ProjectileScript
{
    float timeCounter = 0;
    public int timeInterval = 4;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("ball"))
        {
            BallScript hitBall = collision.GetComponent<BallScript>();
            if (hitBall.selfTeam != ownerTeam)
            {
                if (hitBall.selfTeam != ownerTeam)
                {
                    hitBall.ApplyDotDamage(3, this.gameObject);
                }
            }
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ball"))
        {
            BallScript hitBall = collision.GetComponent<BallScript>();
            if (hitBall.selfTeam != ownerTeam)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= 0.1)
                {
                    hitBall.ApplyDotDamage(2, this.gameObject);
                    timeCounter = 0;
                }
            }
        }
    }
} 