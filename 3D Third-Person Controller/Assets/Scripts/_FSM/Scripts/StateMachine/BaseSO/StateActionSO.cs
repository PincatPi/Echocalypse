using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有限状态机的状态配置文件基类
/// </summary>
public abstract class StateActionSO : ScriptableObject
{
    // 该状态的状态优先级
    [SerializeField] protected int statePriority;
    
    //进入该状态
    public virtual void OnEnter(StateMachineSystem stateMachineSystem) { }

    //处于该状态
    public abstract void OnUpdate();

    //退出该状态
    public virtual void OnExit() { }
    
    //提供给外部，获取状态优先级
    public int GetStatePriority() => statePriority;
}
