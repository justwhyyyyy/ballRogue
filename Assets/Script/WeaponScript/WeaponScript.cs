using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public string weaponName;//������
    //��������
    public bool isPlayer;
    public team weaponTeam;
    //��������
    public Vector3 weaponPos;
    public int damage;
    public float radius;//��ת�뾶
    public float rotateSpeed;//��ת�ٶ�
    public short rotateDir;//��ת����1��˳ʱ�룬-1����ʱ��f
        
    public float actionSpeedMultiplier = 1f;//�����ٶ�ϵ����ͨ�ýӿ�

    public float originRotateSpeed;
    public int originDamage;
    
    public Transform ballPos;
    public GameObject selfball;

    protected abstract void InitWeaponStats();
    //����damage��radius��rotateSpeed;
    public virtual void Start()
    {
        ballPos = transform.parent;
        selfball = transform.parent.gameObject;
        weaponTeam = selfball.GetComponent<BallScript>().selfTeam;//������������
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


    public virtual void OnCollisionEnter2D(Collision2D collision)//��ײ�߼�
    {
        if(collision.collider.CompareTag("wall"))
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
            selfball.GetComponent<BallScript>().WeaponCauseDamage(damage, collision.gameObject,this.gameObject);
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
