using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assembly = System.Reflection.Assembly;


public class EnemyCombatController : CombatControllerBase
{
    //组件
    private EnemyView enemyView;
    private EnemyBase enemyParameter;
    
    //战斗相关
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField, Header("攻击目标")] protected Transform currentTarget = null;
    [SerializeField] protected GameObject attacker;
    
    //巡逻
    public Transform[] patrolPoints;

    private int lockOnHash;
    
    private void Start()
    {
        base.Start();
        enemyView = GetComponent<EnemyView>();
        enemyParameter = GetComponent<EnemyBase>();
        
        lockOnHash = Animator.StringToHash("LockOn");
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
}
