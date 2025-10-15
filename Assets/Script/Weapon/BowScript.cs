using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BowScript : WeaponScript
{
    public int maxArrowSpeed;//蓄满力的速度
    public int minArrowSpeed;//最小的速度

    public float shootCD;//射箭CD
    public float chargeDuration;//蓄力最长时间

    public bool isCharging;//是否正在蓄力
    public bool canShoot;//是否可以开始蓄力
    public bool shootNow;//为true时射出箭

    private float chargingTime;//内置计时，蓄力了多久

    private Animator animator;

    float rayLength;//ai敌人检测距离

    public GameObject arrowPrefab;//箭预制体
    public ArrowScript arrowScript;//箭脚本

    public LayerMask ballLayerMask;//Ball的layerMask

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
        //基础数值
        damage = 0;
        rotateSpeed = 100f;
        radius = 0.4f;
        //特殊数值
        shootCD = 1f;
        chargeDuration = 2f;
        maxArrowSpeed = 15;
        minArrowSpeed = 5;
        rayLength = 8f;
        //归零数值
        canShoot = true;
        isCharging = false;
        shootNow = false;
        chargingTime = 0f;

        animator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), selfball.GetComponent<Collider2D>());
    }
    public void Update()
    {
        if( isPlayer && canShoot && Input.GetKeyDown(KeyCode.Space))//按下开始蓄力
        {
            StartCoroutine(ShootCharging());
        }
        else if(Input.GetKeyUp(KeyCode.Space) && isPlayer&& isCharging)//松开发射
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
        Rotate();//旋转
        Vector3 dir = (transform.position - ballPos.position).normalized;//计算方向
        bowPosition = ballPos.position + dir * radius;//计算位置
        transform.position = bowPosition;//更新位置
    }
    IEnumerator ShootCharging()//射击蓄力与发射逻辑
    {
        isCharging = true;
        canShoot = false;
        float chargeRatio;
        while(!shootNow)
        {
            int stage = 0;//分段播放动画
            chargeRatio = Mathf.Clamp01(chargingTime / chargeDuration);//计算蓄力比例
            chargingTime += Time.deltaTime * actionSpeedMultiplier;//蓄力速度为真实时间乘行动速率
            if(chargingTime >= chargeDuration)//超过满蓄力就回到满蓄力
            {
                chargingTime = chargeDuration;
            }
            if (chargeRatio >= 0.75f) stage = 4;//根据蓄力比例把动画分成四份
            else if(chargeRatio >=0.5f) stage = 3;
            else if (chargeRatio >=0.25f) stage=2;
            else stage=1;
            animator?.SetInteger("chargeState", stage);
            yield return null;
        }
        animator?.SetInteger("chargeState", 0);
        
        chargeRatio = Mathf.Clamp01(chargingTime / chargeDuration);//计算蓄力比例
        //弓箭数值
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
        
        arrowScript.Init(this.gameObject,this.weaponTeam,arrowSpeed,shootDir);//初始化箭的数据

        chargingTime = 0f;//重置变量
        shootNow = false;
        isCharging = false;
        yield return new WaitForSeconds(shootCD);//等待cd
        canShoot = true;
    }

    IEnumerator AutoShoot()
    {
        while(true)
        {
            if(canShoot)
            {
                StartCoroutine (ShootCharging());
                
                while(isCharging && !shootNow)//检测前方有无敌人
                {
                    Vector2 rayCastDir = (transform.position - selfball.transform.position).normalized;//射线方向

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