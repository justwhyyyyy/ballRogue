using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<BaseBuff> activeBuffs = new List<BaseBuff>();//���ϣ���buff
    public Dictionary<string,GameObject> buffEffectsDict = new Dictionary<string,GameObject>();

    //����ṹ�巽����inspector�������Դﵽ��ʼ���ֵ��Ŀ�ģ���Ҳ��֪��������д������
    //������϶���buff����ȥinstantiate�����Ч������monobehavior����ܴ�Ԥ���壬��Gameobject������instantiate��destroy
    //���죺����ֿ��Ե���UnityEngine.Object.Instantiate��UnityEngine.Object.Destroy,���ǻ��ǲ��ܴ�Ԥ���壬�ǻ���Ҫ����ֵ�
    //��
    [System.Serializable]
    public struct BuffEffectPair 
    {
        public string buffName;
        public GameObject effectPrefab;
    }
    public BuffEffectPair[] buffEffectPairs;

    private BallScript selfBall;

    private void Awake()//���غ��Զ�ִ��
    {
        selfBall = GetComponent<BallScript>();//��Ϊ���ǹ�����ͬһ�����ϵ�ֱ��get
        foreach(var pair in buffEffectPairs)//��ʼ���ֵ�
        {
            buffEffectsDict[pair.buffName] = pair.effectPrefab;
        }
    }
    private void FixedUpdate()//����buffЧ��
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)//�������
        {
            BaseBuff buff = activeBuffs[i];
            buff.UpdateEffect(selfBall);//ִ��Ч��
            if(buff.ifEnd())//����Ƿ����
            {
                buff.End();
                activeBuffs.RemoveAt(i);//C#������û�б��κνű����õ�ʱ����Լ��ͷ��ڴ�
            }
        }
    }
    public void AddBuff(BaseBuff newBuff)//���Buff�Ľӿ�
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

        GameObject effectPrefab = GetEffectPrefab(newBuff.buffName);//�ҵ���Ӧ������Ч��
        GameObject effectInstance = Instantiate(effectPrefab,selfBall.gameObject.transform.position,
        Quaternion.identity,selfBall.gameObject.transform);//���ɣ����ø�����Ϊ����
        effectInstance.transform.localPosition = Vector3.zero;

        newBuff.effectInstance = effectInstance;

        activeBuffs.Add(newBuff);//������
    }
    public GameObject GetEffectPrefab(string buffName)//���ֵ�������Ч��
    {
        if (buffEffectsDict.TryGetValue(buffName, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}