using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<BaseBuff> activeBuffs = new List<BaseBuff>();//集合，存buff
    public Dictionary<string,GameObject> buffEffectsDict = new Dictionary<string,GameObject>();

    //这个结构体方便在inspector里数组以达到初始化字典的目的，我也不知道这样子写蠢不蠢
    //最理想肯定是buff本身去instantiate这个特效，但是monobehavior类才能存预制体，存Gameobject，才能instantiate和destroy
    //隔天：最后发现可以调用UnityEngine.Object.Instantiate和UnityEngine.Object.Destroy,但是还是不能存预制体，那还是要这个字典
    //烦
    [System.Serializable]
    public struct BuffEffectPair 
    {
        public string buffName;
        public GameObject effectPrefab;
    }
    public BuffEffectPair[] buffEffectPairs;

    private BallScript selfBall;

    private void Awake()//挂载后自动执行
    {
        selfBall = GetComponent<BallScript>();//因为都是挂载在同一个球上的直接get
        foreach(var pair in buffEffectPairs)//初始化字典
        {
            buffEffectsDict[pair.buffName] = pair.effectPrefab;
        }
    }
    private void FixedUpdate()//处理buff效果
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)//倒序遍历
        {
            BaseBuff buff = activeBuffs[i];
            buff.UpdateEffect(selfBall);//执行效果
            if(buff.ifEnd())//检测是否结束
            {
                buff.End();
                activeBuffs.RemoveAt(i);//C#的类在没有被任何脚本引用的时候会自己释放内存
            }
        }
    }
    public void AddBuff(BaseBuff newBuff)//添加Buff的接口
    {
        foreach(var buff in activeBuffs)
        {
            if(buff.buffName == newBuff.buffName)
            {
                buff.Renew();
                return;
            }
        }
        newBuff.Init();

        GameObject effectPrefab = GetEffectPrefab(newBuff.buffName);//找到对应的粒子效果
        GameObject effectInstance = Instantiate(effectPrefab,selfBall.gameObject.transform.position,
        Quaternion.identity,selfBall.gameObject.transform);//生成，设置父物体为该球
        effectInstance.transform.localPosition = Vector3.zero;

        newBuff.effectInstance = effectInstance;

        activeBuffs.Add(newBuff);//进集合
    }
    public GameObject GetEffectPrefab(string buffName)//查字典找粒子效果
    {
        if (buffEffectsDict.TryGetValue(buffName, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}