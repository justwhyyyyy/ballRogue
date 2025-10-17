using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Traits/TraitInfo")]
public class TraitInfo : ScriptableObject
{
    [Header("基础信息")]
    public string traitName;
    public string description;

    [Header("逻辑参数")]
    public int level;            // 词条等级
    public float duration;       // 词条持续时间
    public bool useUpdate;       // 是否启用Update逻辑
    public bool useCoroutine;    // 是否启用协程逻辑
    public bool useEvent;        // 是否响应事件
}
