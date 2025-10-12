using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StaffScript : WeaponScript
{
    [System.Serializable]//需要这行使Unity能显示该struct
    public struct staffProjectile
    {
        public GameObject Prefab;
        public float offset;
        public float accuracy;
        public float speed;
        public float shootCD;//单次射击的间隙，单位：秒
        public int needShootTimes;//蓄力发射前需要的次数

        public System.Action<StaffScript> powerShootAction;
    }//TODO:后面可能要写成类吧
    //三种子弹预制体
    public staffProjectile[] projectiles = new staffProjectile[3];
    private staffProjectile nowProjecttileType;//当前的子弹类型
    int ProjectileIndex;
    
    public GameObject hurricanPrefab;

    int shootTimeCounter = 0;//已经发射的次数
    float shootCDCouter = 0f;//单次射击计时器

    bool isShooting = false;


    protected override void InitWeaponStats()//参数
    {
        damage = 5;
        radius = 0.5f;
        rotateSpeed = 80f;
        //初始化子弹类型
        nowProjecttileType = projectiles[2];
        ProjectileIndex = 2;
    }
    private void FixedUpdate()
    {
        UpdatePosition();
    }
    private void Update()
    {
        if (isShooting)
        {
            return;
        }
        shootCDCouter += Time.deltaTime;
        if (shootCDCouter >= nowProjecttileType.shootCD)
        {
            if (shootTimeCounter >= nowProjecttileType.needShootTimes)
            {
                //强化射击
                SwitchPoweredShoot(ProjectileIndex);
                //射击后切子弹在强化射击里实现
                shootTimeCounter = 0;
            }
            else
            {
                simplyShoot();
                //普通射击
                shootTimeCounter++;
            }
            shootCDCouter = 0f;
        }
    }
    private ProjectileScript simplyShoot()
    {
        Vector2 shootDir = (transform.position - selfball.transform.position).normalized;//计算方向
        //加上随机偏移量
        float spread = nowProjecttileType.accuracy;
        float randomAngle = UnityEngine.Random.Range(-spread - 25, spread - 25);
        shootDir = (Vector2)(Quaternion.Euler(0, 0, randomAngle) * shootDir);
        //生成子弹
        quaternion projectileRotation = transform.rotation * nowProjecttileType.Prefab.transform.rotation;//修正贴图方向
        GameObject bullet = Instantiate(nowProjecttileType.Prefab, 
            (Vector2)transform.position + shootDir * nowProjecttileType.offset,
            projectileRotation);
        //初始化
        ProjectileScript proj = bullet.GetComponent<ProjectileScript>();
        proj.Init(selfball,weaponTeam,nowProjecttileType.speed,shootDir);
        return proj;
    }
    private void SwitchPoweredShoot(int Index)
    {
        switch (Index)
        {
            case 0:StartCoroutine(FireBallPoweredShoot());break;
            case 1:StartCoroutine(IcePoweredShoot()); break;
            case 2:StartCoroutine(WindPoweredShoot()); break;
        }
    }
    IEnumerator FireBallPoweredShoot()
    {
        isShooting = true;
        for (int i = 0; i < 10; i++)
        {
            simplyShoot();
            yield return new WaitForSeconds(0.02f);
        }
        SetProjectileIndex(ProjectileIndex);
        isShooting = false;
    }
    IEnumerator IcePoweredShoot()
    {
        isShooting = true;
        for (int i = 0; i < 3; i++)
        {
            ProjectileScript bullet= simplyShoot();
            yield return new WaitForSeconds(0.1f);
            bullet.gameObject.AddComponent<HomingProjectile>();
        }
        SetProjectileIndex(ProjectileIndex);
        isShooting = false;
        yield return null;
    }
    IEnumerator WindPoweredShoot()
    {
        isShooting = true;
        
        Vector2 shootDir = (transform.position - selfball.transform.position).normalized;//计算方向

        //生成飓风
        GameObject hurricane = Instantiate(hurricanPrefab,
            (Vector2)transform.position + shootDir * nowProjecttileType.offset,
            transform.rotation);
        //初始化
        ProjectileScript proj = hurricane.GetComponent<ProjectileScript>();

        proj.Init(selfball, weaponTeam, 2, shootDir);
        yield return null;
        isShooting = false;
        SetProjectileIndex(ProjectileIndex);
    }
    public void UpdatePosition()
    {
        Rotate();//旋转
        Vector3 dir = (transform.position - ballPos.position).normalized;//计算方向
        weaponPos = ballPos.position + dir * radius;//计算位置
        transform.position = weaponPos;//更新位置
    }
    private void SetProjectileIndex(int index)
    {
        //保证下标合法，用模运算自动循环
        ProjectileIndex = (index + 1) % projectiles.Length;
        nowProjecttileType = projectiles[ProjectileIndex];
    }
}