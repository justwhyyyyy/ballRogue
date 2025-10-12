using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff
{
    public string buffName;
    public int duration;//0.02*duration��,����buff��FixedUpdate����buffʱ��
    public int pastTime;//�����Ѿ�������ʱ��
    public int stack;//����
    public GameObject buffSource;
    public GameObject buffTarget;
    public GameObject effectInstance;
    public abstract void Init();//��ʼ����ֵ,������buffʩ����,override��
    public abstract void UpdateEffect(BallScript selfBall);//buff�ľ���Ч��,override��
    public abstract void Renew();//�ٴλ�ø�buff�Ĵ���ʽ��override
    public abstract void End();

    public bool ifEnd()//�ǵõ���������buff�Ƿ����
    {
        return pastTime >= duration;
    }
}