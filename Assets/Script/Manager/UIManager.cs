using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Prefab
    public List<GameObject> hpBarPrefabList = new List<GameObject>();
    //UI����
    public List<BallUICtrler> fourUI = new List<BallUICtrler>();
    //����Distribute����Ȼ����ö�ӦUICtrler��Init����
    public void InitNewUI(GameObject ball)
    {
        DistributeBallToUI(ball); 
    }
    //����λ��
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
        Debug.LogError("û��UI��λ");
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