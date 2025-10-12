using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff
{
    public string buffName;
    public int duration;//0.02*duration秒,所有buff用FixedUpdate管理buff时间
    public int pastTime;//计算已经经过的时间
    public int stack;//层数
    public GameObject buffSource;
    public GameObject buffTarget;
    public GameObject effectInstance;
    public abstract void Init();//初始化数值,参数是buff施加者,override它
    public abstract void UpdateEffect(BallScript selfBall);//buff的具体效果,override它
    public abstract void Renew();//再次获得该buff的处理方式，override
    public abstract void End();

    public bool ifEnd()//记得调用这个检测buff是否结束
    {
        return pastTime >= duration;
    }
}