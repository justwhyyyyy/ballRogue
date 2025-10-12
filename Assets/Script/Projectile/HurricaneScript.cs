using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneScript : ProjectileScript
{
    public float rotateSpeed = 180f;//每秒旋转度数
    public float hurricaneDuration = 3f;

    public float timeCounter;

    public bool stop = false;
    public bool touchedBall = false;
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (touchedBall) return;//
        if (collision.CompareTag("wall"))
        {//TODO:改
            Destroy(gameObject);
        }
        else if (collision.CompareTag("ball"))
        {
            BallScript hitBall = collision.GetComponent<BallScript>();
            if (hitBall.selfTeam != ownerTeam)
            {
                touchedBall = true;
                StartCoroutine(SlowDown());
            }
        }
    }
    IEnumerator SlowDown()
    {
        Rigidbody2D hurricanRb = GetComponent<Rigidbody2D>();
        while(hurricanRb.velocity.magnitude != 0)
        {
            hurricanRb.velocity *= 0.6f;
            if(hurricanRb.velocity.magnitude <= 0.2)
            {
                hurricanRb.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ball"))
        {
            BallScript hitBall = collision.GetComponent<BallScript>();
            if (hitBall.selfTeam != ownerTeam)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= 0.08)
                {
                    hitBall.ApplyDotDamage(2, this.gameObject);
                    timeCounter = 0;
                }
            }
        }
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(hurricaneDuration);
        Destroy(gameObject);
    }
}
