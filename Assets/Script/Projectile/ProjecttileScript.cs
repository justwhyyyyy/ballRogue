using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //����ʱ��ȡ������
    public Vector2 shootDir;
    public GameObject owner;
    public team ownerTeam;
    public float shootSpeed;
    //rb
    public Rigidbody2D projectileRb;
    public void Init(GameObject owner, team ownerTeam, float shootSpeed, Vector2 shootDir)
    {
        //��ֵ
        this.owner = owner;
        this.ownerTeam = ownerTeam;
        this.shootSpeed = shootSpeed;
        this.shootDir = shootDir;
        //����
        projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.velocity = shootDir * shootSpeed;
    }

    private void Awake()
    {
        projectileRb = GetComponent<Rigidbody2D>();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //override��
    }
}
