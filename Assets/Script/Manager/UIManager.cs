using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Prefab
    public List<GameObject> hpBarPrefabList = new List<GameObject>();
    //UI区域
    public List<BallUICtrler> fourUI = new List<BallUICtrler>();
    //调用Distribute函数然后调用对应UICtrler的Init函数
    public void InitNewUI(GameObject ball)
    {
        DistributeBallToUI(ball); 
    }
    //分配位置
    public void DistributeBallToUI(GameObject ball)
    {
        foreach(BallUICtrler uiCtrler in fourUI)
        {
            if(uiCtrler.ballScript == null)
            {
                uiCtrler.Init(ball);
                return;
            }
        }
        Debug.LogError("没有UI槽位");
    }

    public void ReleaseUI(BallUICtrler uiCtrler)
    {
        int index = fourUI.IndexOf(uiCtrler);
        if(index >= 0)
        {
            uiCtrler.ReleaseBall();
        }
    }
}