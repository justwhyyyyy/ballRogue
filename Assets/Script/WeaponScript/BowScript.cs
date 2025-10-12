using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BowScript : WeaponScript
{
    public int maxArrowSpeed;//���������ٶ�
    public int minArrowSpeed;//��С���ٶ�

    public float shootCD;//���CD
    public float chargeDuration;//�����ʱ��

    public bool isCharging;//�Ƿ���������
    public bool canShoot;//�Ƿ���Կ�ʼ����
    public bool shootNow;//Ϊtrueʱ�����

    private float chargingTime;//���ü�ʱ�������˶��

    private Animator animator;

    float rayLength;//ai���˼�����

    public GameObject arrowPrefab;//��Ԥ����
    public ArrowScript arrowScript;//���ű�

    public LayerMask ballLayerMask;//Ball��layerMask

    public Vector2 bowPosition;
    public override void Start()
    {
        base.Start();
        ballLayerMask = LayerMask.GetMask("ball");
        if(!isPlayer)
        {
            StartCoroutine(AutoShoot());
        }
    }
    protected override void InitWeaponStats()
    {
        //������ֵ
        damage = 0;
        rotateSpeed = 100f;
        radius = 0.4f;
        //������ֵ
        shootCD = 1f;
        chargeDuration = 2f;
        maxArrowSpeed = 15;
        minArrowSpeed = 5;
        rayLength = 8f;
        //������ֵ
        canShoot = true;
        isCharging = false;
        shootNow = false;
        chargingTime = 0f;

        animator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), selfball.GetComponent<Collider2D>());
    }
    public void Update()
    {
        if( isPlayer && canShoot && Input.GetKeyDown(KeyCode.Space))//���¿�ʼ����
        {
            StartCoroutine(ShootCharging());
        }
        else if(Input.GetKeyUp(KeyCode.Space) && isPlayer&& isCharging)//�ɿ�����
        {
            shootNow = true;
        }
    }
    void FixedUpdate()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        Rotate();//��ת
        Vector3 dir = (transform.position - ballPos.position).normalized;//���㷽��
        bowPosition = ballPos.position + dir * radius;//����λ��
        transform.position = bowPosition;//����λ��
    }
    IEnumerator ShootCharging()//��������뷢���߼�
    {
        isCharging = true;
        canShoot = false;
        float chargeRatio;
        while(!shootNow)
        {
            int stage = 0;//�ֶβ��Ŷ���
            chargeRatio = Mathf.Clamp01(chargingTime / chargeDuration);//������������
            chargingTime += Time.deltaTime * actionSpeedMultiplier;//�����ٶ�Ϊ��ʵʱ����ж�����
            if(chargingTime >= chargeDuration)//�����������ͻص�������
            {
                chargingTime = chargeDuration;
            }
            if (chargeRatio >= 0.75f) stage = 4;//�������������Ѷ����ֳ��ķ�
            else if(chargeRatio >=0.5f) stage = 3;
            else if (chargeRatio >=0.25f) stage=2;
            else stage=1;
            animator?.SetInteger("chargeState", stage);
            yield return null;
        }
        animator?.SetInteger("chargeState", 0);
        
        chargeRatio = Mathf.Clamp01(chargingTime / chargeDuration);//������������
        //������ֵ
        Vector3 initPos = transform.position;
        Vector2 shootDir = (transform.position - selfball.transform.position).normalized;
        Quaternion initRot = transform.rotation;
        float arrowSpeed = chargeRatio * maxArrowSpeed;
        if(arrowSpeed <= minArrowSpeed)
        {
            arrowSpeed = minArrowSpeed;
        }

        GameObject arrowObj = Instantiate(arrowPrefab, initPos, initRot);
        ArrowScript arrowScript = arrowObj.GetComponent<ArrowScript>();
        
        arrowScript.Init(this.gameObject,this.weaponTeam,arrowSpeed,shootDir);//��ʼ����������

        chargingTime = 0f;//���ñ���
        shootNow = false;
        isCharging = false;
        yield return new WaitForSeconds(shootCD);//�ȴ�cd
        canShoot = true;
    }

    IEnumerator AutoShoot()
    {
        while(true)
        {
            if(canShoot)
            {
                StartCoroutine (ShootCharging());
                
                while(isCharging && !shootNow)//���ǰ�����޵���
                {
                    Vector2 rayCastDir = (transform.position - selfball.transform.position).normalized;//���߷���

                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayCastDir, rayLength,ballLayerMask);
                    foreach(RaycastHit2D hit in hits )
                    {
                        if (hit.collider.gameObject.GetComponent<BallScript>().selfTeam != weaponTeam)
                        {
                            shootNow = true;
                            break;
                        }
                    }
                    yield return null;
                }
                yield return new WaitForSeconds(shootCD);
            }
            else
            {
                yield return null;
            }
        }
    }
}