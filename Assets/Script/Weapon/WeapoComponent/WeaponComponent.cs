using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public GameObject thisWeapon;
    public Vector3 weaponPos;
    public short rotateDir;//��ת����1��˳ʱ�룬-1����ʱ��f

    public float actionSpeedMultiplier = 1f;//�����ٶ�ϵ����ͨ�ýӿ�
    
    public WeaponData weaponData;//��������

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
    public virtual void OnCollisionEnter2D(Collision2D collision)//��ײ�߼�
    {
        if (collision.collider.CompareTag("wall"))
        {
            return;
        }
        else if (collision.collider.CompareTag("weapon"))
        {
            Vector2 normal = collision.contacts[0].normal;//���߷���
            Rigidbody2D ballRb = selfball.GetComponent<Rigidbody2D>();

            float bounceForce = 5f;//ʩ����
            ballRb.AddForce(normal * bounceForce, ForceMode2D.Impulse);
            return;
        }
        else if (collision.collider.gameObject.GetComponent<BallScript>()?.selfTeam != selfball.GetComponent<BallScript>().selfTeam)
        {
            selfball.GetComponent<BallScript>().WeaponCauseDamage(weaponData.damage.GetValue(), collision.gameObject, this.gameObject);
            //ʩ�ӻ���Ч��
            Vector2 hitDir = collision.gameObject.transform.position - selfball.transform.position;
            Rigidbody2D enemyBallRb = collision.gameObject.GetComponent<Rigidbody2D>();

            float bounceForce = 5f;//ʩ����
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
        Vector3 dir = (transform.position - ballTrans.position).normalized;//���㷽��
        weaponPos = ballTrans.position + dir * weaponData.radius;//����λ��
        transform.position = weaponPos;//����λ��
    }
}