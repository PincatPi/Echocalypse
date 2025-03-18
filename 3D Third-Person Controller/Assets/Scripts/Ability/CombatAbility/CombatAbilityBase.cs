using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CombatAbilityBase : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected int abilityID;
    [SerializeField] protected float abilityCD;
    [SerializeField] protected float abilityUseDistance;
    [SerializeField] protected bool abilitiyIsAvailable;

    #region 组件

    protected Animator animator;
    protected EnemyCombatController combatController;
    protected EnemyMovementController enemyMovementController;
    protected EnemyBase enemyParameter;

    #endregion

    #region 动画状态机哈希值

    protected int verticalHash = Animator.StringToHash("Vertical");
    protected int horizontalHash = Animator.StringToHash("Horizontal");
    protected int moveSpeedHash = Animator.StringToHash("MoveSpeed");

    #endregion

    /// <summary>
    /// 调用技能
    /// </summary>
    public abstract void InvokeAbility();

    /// <summary>
    /// 使用技能
    /// </summary>
    protected void UseAbility()
    {
        //动画机中播放技能动画
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion"))
        {
            animator.CrossFade(abilityName, 0.1f);
        }
        abilitiyIsAvailable = false;
        //将自己从可用技能列表中移出，并移入冷却技能列表
        
        //技能CD
        AbilityCoolDown();
    }

    public void AbilityCoolDown()
    {
        Timer timer = CachePoolManager.Instance.GetObject("Tool/Timer").GetComponent<Timer>();
        timer.CreateTime(abilityCD, () =>
        {
            abilitiyIsAvailable = true;
        });
    }
    
    #region 公共调用接口

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(Animator animator, EnemyCombatController combatController, 
        EnemyMovementController enemyMovementController, EnemyBase enemyParameter)
    {
        this.animator = animator;
        this.combatController = combatController;
        this.enemyMovementController = enemyMovementController;
        this.enemyParameter = enemyParameter;
    }
    
    
    public string GetAbilityName() => abilityName;
    public int GetAbilityID() => abilityID;
    public float GetAbilityCD() => abilityCD;
    public float GetAbilityUseDistance() => abilityUseDistance;
    public bool GetAbilityAvailable() => abilitiyIsAvailable;
    
    public void SetAbilityAvailable(bool isDone) { abilitiyIsAvailable = isDone; }

    #endregion
}
