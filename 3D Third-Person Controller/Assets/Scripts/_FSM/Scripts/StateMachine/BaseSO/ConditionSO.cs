using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态转换条件配置文件基类
/// </summary>
public abstract class ConditionSO : ScriptableObject
{
    [SerializeField] protected int priority; //条件优先级

    //初始化
    public virtual void Init(StateMachineSystem stateSystem) { }
    
    /// <summary>
    /// 判断转换条件是否满足
    /// </summary>
    /// <returns></returns>
    public abstract bool ConditionSetUp();

    //获取条件优先级
    public int GetConditionPriority() => priority;
}
