using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StaffScript : WeaponScript
{
    [System.Serializable]//��Ҫ����ʹUnity����ʾ��struct
    public struct staffProjectile
    {
        public GameObject Prefab;
        public float offset;
        public float accuracy;
        public float speed;
        public float shootCD;//��������ļ�϶����λ����
        public int needShootTimes;//��������ǰ��Ҫ�Ĵ���

        public System.Action<StaffScript> powerShootAction;
    }//TODO:�������Ҫд�����
    //�����ӵ�Ԥ����
    public staffProjectile[] projectiles = new staffProjectile[3];
    private staffProjectile nowProjecttileType;//��ǰ���ӵ�����
    int ProjectileIndex;
    
    public GameObject hurricanPrefab;

    int shootTimeCounter = 0;//�Ѿ�����Ĵ���
    float shootCDCouter = 0f;//���������ʱ��

    bool isShooting = false;


    protected override void InitWeaponStats()//����
    {
        damage = 5;
        radius = 0.5f;
        rotateSpeed = 80f;
        //��ʼ���ӵ�����
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
                //ǿ�����
                SwitchPoweredShoot(ProjectileIndex);
                //��������ӵ���ǿ�������ʵ��
                shootTimeCounter = 0;
            }
            else
            {
                simplyShoot();
                //��ͨ���
                shootTimeCounter++;
            }
            shootCDCouter = 0f;
        }
    }
    private ProjectileScript simplyShoot()
    {
        Vector2 shootDir = (transform.position - selfball.transform.position).normalized;//���㷽��
        //�������ƫ����
        float spread = nowProjecttileType.accuracy;
        float randomAngle = UnityEngine.Random.Range(-spread - 25, spread - 25);
        shootDir = (Vector2)(Quaternion.Euler(0, 0, randomAngle) * shootDir);
        //�����ӵ�
        quaternion projectileRotation = transform.rotation * nowProjecttileType.Prefab.transform.rotation;//������ͼ����
        GameObject bullet = Instantiate(nowProjecttileType.Prefab, 
            (Vector2)transform.position + shootDir * nowProjecttileType.offset,
            projectileRotation);
        //��ʼ��
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
        
        Vector2 shootDir = (transform.position - selfball.transform.position).normalized;//���㷽��

        //����쫷�
        GameObject hurricane = Instantiate(hurricanPrefab,
            (Vector2)transform.position + shootDir * nowProjecttileType.offset,
            transform.rotation);
        //��ʼ��
        ProjectileScript proj = hurricane.GetComponent<ProjectileScript>();

        proj.Init(selfball, weaponTeam, 2, shootDir);
        yield return null;
        isShooting = false;
        SetProjectileIndex(ProjectileIndex);
    }
    public void UpdatePosition()
    {
        Rotate();//��ת
        Vector3 dir = (transform.position - ballPos.position).normalized;//���㷽��
        weaponPos = ballPos.position + dir * radius;//����λ��
        transform.position = weaponPos;//����λ��
    }
    private void SetProjectileIndex(int index)
    {
        //��֤�±�Ϸ�����ģ�����Զ�ѭ��
        ProjectileIndex = (index + 1) % projectiles.Length;
        nowProjecttileType = projectiles[ProjectileIndex];
    }
}