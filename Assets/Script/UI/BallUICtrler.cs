using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallUICtrler : MonoBehaviour
{
    public int count; //Ӳ����
    public Vector3 hpBarPos;//Ӳ����
    //��������
    public BallScript ballScript;
    //UI�б�
    public HealthBarScript hpBar;
    //Ѫ��Ԥ����
    public List<GameObject> hpBarList;//Ӳ����
    private void Start()
    {
        hpBarPos = new Vector3(-10, 70, 0);
    }
    public void Init(GameObject ownerBall)
    {
        ballScript = ownerBall.GetComponent<BallScript>();
        GameObject hpBarPrefab = hpBarList[count];//����λ�û�ȡ��ӦԤ����
        //���ɲ���������
        GameObject spawnHpBar = Instantiate(hpBarPrefab, transform);
        spawnHpBar.transform.localPosition = hpBarPos;
        hpBar = spawnHpBar.GetComponentInChildren<HealthBarScript>();
        hpBar.Init(ballScript);//�����Դ��ĳ�ʼ������
    }
    public void ReleaseBall()
    {

    }
}
