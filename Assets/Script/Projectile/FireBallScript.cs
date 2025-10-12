using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : ProjectileScript
{
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
                BurnedBuff burnedBuff = new BurnedBuff();
                burnedBuff.buffSource = owner;
                burnedBuff.buffTarget = hitBall.gameObject;
                hitBall.GetComponent<BuffManager>().AddBuff(burnedBuff);
                Destroy(gameObject);
            }
        }
    }
}