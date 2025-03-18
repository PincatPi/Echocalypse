using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Assembly = System.Reflection.Assembly;


public class EnemyCombatController : CombatControllerBase
{
    //组件
    private EnemyView enemyView;
    private EnemyBase enemyParameter;
    private EnemyMovementController enemyMovementController;
    
    //战斗相关
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField, Header("攻击目标")] protected Transform currentTarget = null;
    [SerializeField] protected GameObject attacker;
    
    [Header("技能")]
    [SerializeField] private List<CombatAbilityBase> abilityList = new List<CombatAbilityBase>();

    private int lockOnHash;
    
    private void Start()
    {
        base.Start();
        enemyView = GetComponent<EnemyView>();
        enemyParameter = GetComponent<EnemyBase>();
        enemyMovementController = GetComponent<EnemyMovementController>();
        
        lockOnHash = Animator.StringToHash("LockOn");
        
        //初始化所有技能
        InitAllAbilities();
    }

    private void Update()
    {
        UpdateCurrentTarget();
    }
    
    //敌人公共方法
    //受到攻击
    public override void OnHit(ComboInteractionConfig interactionConfig, Transform attacker)
    {
        base.OnHit(interactionConfig, attacker);
        FindTarget();
        LookAtTarget();
    }

    private void FindTarget()
    {
        //若已经有目标，则返回
        if(currentTarget)
            return;
        Collider[] target = new Collider[1];
        var size = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(4, 4, 4), target, Quaternion.identity, playerLayer);
        if (size != 0)
        {
            currentTarget = target[0].transform;
        }
    }

    private void LookAtTarget()
    {
        if(!currentTarget)
            return;
        Vector3 dir = currentTarget.position - transform.position;
        transform.forward = dir.normalized;
    }

    private void UpdateCurrentTarget()
    {
        currentTarget = enemyView.CurrentTarget;
        if (currentTarget)
        {
            animator.SetFloat(lockOnHash, 1f);   
        }
        else
        {
            animator.SetFloat(lockOnHash, 0f);
        }
    }
    
    public Transform GetCurrentTarget()
    {
        if(!currentTarget)
            return null;
        return currentTarget;
    }

    public float GetCurrentTargetDistance()
    {
        if (!currentTarget)
            return -1f;
        return Vector3.Distance(transform.position, currentTarget.position);
    }
    
    public Vector3 GetDirectionForTarget()
    {
        if(!currentTarget)
            return Vector3.zero;
        return (currentTarget.position - transform.position).normalized;
    }

    #region 技能

    /// <summary>
    /// 初始化所有技能
    /// </summary>
    private void InitAllAbilities()
    {
        if(abilityList.Count == 0)
            return;
        for (int i = 0; i < abilityList.Count; i++)
        {
            //初始化每个技能
            abilityList[i].Init(animator, this, enemyMovementController, enemyParameter);
            //将技能设为可用
            abilityList[i].SetAbilityAvailable(true);
        }
    }

    /// <summary>
    /// 获得一个可用的（不在冷却中）技能 
    /// </summary>
    public CombatAbilityBase GetAnAvailableAbility()
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if(abilityList[i].GetAbilityAvailable())
                return abilityList[i];
        }
        
        return null;
    }

    /// <summary>
    /// 根据技能名，获得指定的可用技能（若该技能在冷却中则返回null）
    /// </summary>
    public CombatAbilityBase GetAbilityByName(string abilityName)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if(abilityList[i].GetAbilityName() == abilityName)
                return abilityList[i];
        }
        return null;
    }
    
    /// <summary>
    /// 根据技能ID，获得指定的可用技能（若该技能在冷却中则返回null）
    /// </summary>
    public CombatAbilityBase GetAbilityByID(int abilityID)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if(abilityList[i].GetAbilityID() == abilityID)
                return abilityList[i];
        }
        return null;
    }

    #endregion
}
