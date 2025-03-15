using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StateMachineSystem : MonoBehaviour
{
    //状态转换条件
    public NB_Transition transition;
    
    //当前状态
    public StateActionSO currentState;


    private void Awake()
    {
        transition?.Init(this);
        currentState?.OnEnter(this);
    }


    private void Update()
    {
        StateMachineTick();
    }

    private void StateMachineTick() 
    {
        //检查是否有条件成立的状态切换
        transition?.TryGetApplyCondition(); 
        //执行当前状态的运行逻辑
        currentState?.OnUpdate();
    }
}
