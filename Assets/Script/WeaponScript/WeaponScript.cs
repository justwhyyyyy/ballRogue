using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public string weaponName;//武器名
    //队伍属性
    public bool isPlayer;
    public team weaponTeam;
    //基本参数
    public Vector3 weaponPos;
    public int damage;
    public float radius;//旋转半径
    public float rotateSpeed;//旋转速度
    public short rotateDir;//旋转方向，1是顺时针，-1是逆时针f
        
    public float actionSpeedMultiplier = 1f;//动作速度系数，通用接口

    public float originRotateSpeed;
    public int originDamage;
    
    public Transform ballPos;
    public GameObject selfball;

    protected abstract void InitWeaponStats();
    //包括damage，radius，rotateSpeed;
    public virtual void Start()
    {
        ballPos = transform.parent;
        selfball = transform.parent.gameObject;
        weaponTeam = selfball.GetComponent<BallScript>().selfTeam;//武器所属队伍
        rotateDir = 1;
        InitWeaponStats();
        if (selfball.GetComponent<BallScript>().selfTeam == team.Player)
        {
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
        }
    }
    public void Init(float speed,GameObject owner,team weaponTeam,bool IfPlayer)
    {
        
    }


    public virtual void OnCollisionEnter2D(Collision2D collision)//碰撞逻辑
    {
        if(collision.collider.CompareTag("wall"))
        {
            return;
        }
        else if (collision.collider.CompareTag("weapon"))
        {
            Vector2 normal = collision.contacts[0].normal;//法线方向
            Rigidbody2D ballRb = selfball.GetComponent<Rigidbody2D>();

            float bounceForce = 5f;//施加力
            ballRb.AddForce(normal * bounceForce, ForceMode2D.Impulse);
            return;
        }
        else if (collision.collider.gameObject.GetComponent<BallScript>()?.selfTeam != selfball.GetComponent<BallScript>().selfTeam)
        {
            selfball.GetComponent<BallScript>().WeaponCauseDamage(damage, collision.gameObject,this.gameObject);
            //施加击退效果
            Vector2 hitDir = collision.gameObject.transform.position - selfball.transform.position;
            Rigidbody2D enemyBallRb = collision.gameObject.GetComponent<Rigidbody2D>();

            float bounceForce = 5f;//施加力
            enemyBallRb.AddForce(hitDir * bounceForce, ForceMode2D.Impulse);

            return;
        }
    }
    public virtual void Rotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 toMouse = (mousePos - ballPos.position).normalized;
        Vector3 toWeapon = (transform.position - ballPos.position).normalized;
        float cross = toWeapon.x * toMouse.y - toWeapon.y * toMouse.x;
        if (cross < -0.02)
        {
            transform.RotateAround(ballPos.position, Vector3.back, rotateSpeed * Time.fixedDeltaTime * actionSpeedMultiplier);
        }
        else if (cross > 0.02)
        {
            transform.RotateAround(ballPos.position, -Vector3.back, rotateSpeed * Time.fixedDeltaTime * actionSpeedMultiplier);
        }
    }
    
}
