using UnityEngine;

/// <summary>
/// 有限状态机的状态配置文件基类
/// </summary>
public abstract class StateActionSO : ScriptableObject
{
    #region 组件
    
    [SerializeField] protected Animator animator;
    [SerializeField] protected EnemyCombatController enemyCombatController;
    [SerializeField] protected EnemyMovementController enemyMovementController;
    [SerializeField] protected EnemyBase enemyParameter;
    [SerializeField] protected Transform transform;
    
    #endregion
    
    // 该状态的状态优先级
    [SerializeField] protected int statePriority;

    protected virtual void Init(StateMachineSystem stateMachineSystem)
    {
        animator = stateMachineSystem.animator;
        enemyCombatController = stateMachineSystem.enemyCombatController;
        enemyMovementController = stateMachineSystem.enemyMovementController;
        enemyParameter = stateMachineSystem.enemyParameter;
        transform = stateMachineSystem.transform;
    }
    
    //进入该状态
    public virtual void OnEnter(StateMachineSystem stateMachineSystem)
    {
        //初始化状态，获取该状态所需要的参数
        Init(stateMachineSystem);
    }

    //处于该状态
    public abstract void OnUpdate();

    //退出该状态
    public virtual void OnExit() { }
    
    //提供给外部，获取状态优先级
    public int GetStatePriority() => statePriority;
}
