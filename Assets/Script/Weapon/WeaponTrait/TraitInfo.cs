using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Traits/TraitInfo")]
public class TraitInfo : ScriptableObject
{
    [Header("������Ϣ")]
    public string traitName;
    public string description;

    [Header("�߼�����")]
    public int level;            // �����ȼ�
    public float duration;       // ��������ʱ��
    public bool useUpdate;       // �Ƿ�����Update�߼�
    public bool useCoroutine;    // �Ƿ�����Э���߼�
    public bool useEvent;        // �Ƿ���Ӧ�¼�
}
