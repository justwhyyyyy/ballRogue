using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallUICtrler : MonoBehaviour
{
    public int count; //硬编码
    public Vector3 hpBarPos;//硬编码
    //所属的球
    public BallScript ballScript;
    //UI列表
    public HealthBarScript hpBar;
    //血条预制体
    public List<GameObject> hpBarList;//硬编码
    private void Start()
    {
        hpBarPos = new Vector3(-10, 70, 0);
    }
    public void Init(GameObject ownerBall)
    {
        ballScript = ownerBall.GetComponent<BallScript>();
        GameObject hpBarPrefab = hpBarList[count];//根据位置获取对应预制体
        //生成并调整参数
        GameObject spawnHpBar = Instantiate(hpBarPrefab, transform);
        spawnHpBar.transform.localPosition = hpBarPos;
        hpBar = spawnHpBar.GetComponentInChildren<HealthBarScript>();
        hpBar.Init(ballScript);//调用自带的初始化函数
    }
    public void ReleaseBall()
    {

    }
}
