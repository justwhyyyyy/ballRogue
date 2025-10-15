using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public GameObject thisWeapon;
    public Vector3 weaponPos;
    public short rotateDir;//旋转方向，1是顺时针，-1是逆时针f

    public float actionSpeedMultiplier = 1f;//动作速度系数，通用接口
    
    public WeaponData weaponData;//武器数据

    public List<Trait> traits;

    public Transform ballTrans;
    public GameObject selfball;
    public void attachToBall(BallScript ballScript)
    {
        ballTrans = ballScript.gameObject.transform;
        selfball = ballScript.gameObject;
    }
    public void addTrait(Trait trait)
    {
        traits.Add(trait);
        trait.Effect(weaponData);
    }
    public void FixedUpdate()
    {
        if (selfball != null)
        {
            Rotate();
        }
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)//碰撞逻辑
    {
        if (collision.collider.CompareTag("wall"))
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
            selfball.GetComponent<BallScript>().WeaponCauseDamage(weaponData.damage.GetValue(), collision.gameObject, this.gameObject);
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
        Vector3 toMouse = (mousePos - ballTrans.position).normalized;
        Vector3 toWeapon = (transform.position - ballTrans.position).normalized;
        float cross = toWeapon.x * toMouse.y - toWeapon.y * toMouse.x;
        if (cross < -0.02)
        {
            transform.RotateAround(ballTrans.position, Vector3.back, weaponData.rotateSpeed.GetValue() * Time.fixedDeltaTime * actionSpeedMultiplier);
        }
        else if (cross > 0.02)
        {
            transform.RotateAround(ballTrans.position, -Vector3.back, weaponData.rotateSpeed.GetValue() * Time.fixedDeltaTime * actionSpeedMultiplier);
        }
        Vector3 dir = (transform.position - ballTrans.position).normalized;//计算方向
        weaponPos = ballTrans.position + dir * weaponData.radius;//计算位置
        transform.position = weaponPos;//更新位置
    }
}