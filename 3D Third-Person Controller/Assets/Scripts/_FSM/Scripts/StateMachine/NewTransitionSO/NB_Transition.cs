using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NB_Transition", menuName = "StateMachine/Transition/New NB_Transition")]
public class NB_Transition : ScriptableObject
{
    [Serializable]
    private class StateActionConfig 
    {
        public StateActionSO fromState;
        public StateActionSO toState;
        public List<ConditionSO> conditions;
    }
    
    //存储所有状态转换信息和条件
    private Dictionary<StateActionSO, List<StateActionConfig>> states = new Dictionary<StateActionSO, List<StateActionConfig>>();
    //在Inspector窗口中显式List<StateActionConfig>的信息(因为字典不能显示)
    [SerializeField] private List<StateActionConfig> configStateData = new List<StateActionConfig>();
    private StateMachineSystem stateMachineSystem;


    public void Init(StateMachineSystem stateMachineSystem) 
    {
        this.stateMachineSystem = stateMachineSystem;
        SaveAllStateTransitionInfo();
    }
    
    //保存所有状态配置信息
    private void SaveAllStateTransitionInfo() 
    {
        foreach (var item in configStateData)
        {
            //将在inspector窗口中配置的configStateData添加到字典states中
            if (!states.ContainsKey(item.fromState)) 
            {
                states.Add(item.fromState, new List<StateActionConfig>());
                states[item.fromState].Add(item);
            }
            else 
            {
                states[item.fromState].Add(item);
            }
        }
    }

    // 尝试获取条件成立的新状态
    public void TryGetApplyCondition() 
    {
        int conditionPriority = 0; //条件优先级（0最低）
        int statePriority = 0; //状态优先级（0最低）
        List<StateActionSO> toStates = new List<StateActionSO>();
        StateActionSO toState = null;
        //若当前Transition中存在以该currentState为起点的状态转换，则继续；否则直接返回即可
        if (states.ContainsKey(stateMachineSystem.currentState)) 
        {
            //遍历当前状态能转换的状态是否有条件成立
            //取出该条件的所有StateActionConfig
            foreach (var stateItem in states[stateMachineSystem.currentState])
            {
                //遍历每个StateActionConfig
                foreach (var conditionItem in stateItem.conditions)
                {
                    if (conditionItem.ConditionSetUp())
                    {
                        if (conditionItem.GetConditionPriority() >= conditionPriority)
                        {
                            //将转换关系中的下一个状态保存起来
                            //所有StateActionConfig遍历完后，会存在一个唯一满足条件优先级的状态
                            conditionPriority = conditionItem.GetConditionPriority();
                            toStates.Add(stateItem.toState);
                        }
                    }
                }
            }
        }
        // 若没有则返回
        else 
        {
            return;
        }

        if(toStates.Count!=0 || toStates != null) 
        {
            //遍历成立的状态的状态优先级，返回状态优先级最高的条件
            foreach (var item in toStates)
            {
                if (item.GetStatePriority() >= statePriority)
                {
                    statePriority = item.GetStatePriority();
                    toState = item;
                }
            }
        }

        if (toState != null) 
        {
            //进行状态切换
            stateMachineSystem.currentState.OnExit(); //当前状态执行退出逻辑
            stateMachineSystem.currentState = toState; //更新状态
            stateMachineSystem.currentState.OnEnter(this.stateMachineSystem); //新状态执行进入逻辑  
            //重置
            toStates.Clear();
            conditionPriority = 0;
            statePriority = 0;
            toState = null;
        }
    }
}
