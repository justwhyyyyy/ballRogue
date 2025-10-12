using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : ProjectileScript
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
                ColdBuff ColdBuff = new ColdBuff();//…œ∫Æ¿‰buff
                ColdBuff.buffSource = owner;
                ColdBuff.buffTarget = hitBall.gameObject;
                hitBall.GetComponent<BuffManager>().AddBuff(ColdBuff);

                hitBall.TakeDamage(8, this.gameObject);//‘Ï≥……À∫¶
                Destroy(gameObject);
            }
        }
    }
}