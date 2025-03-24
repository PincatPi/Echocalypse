using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    //组件
    //private EnemyView enemyView;
    
    //敌人公共字段
    public float walkSpeed; //走路移动速度
    public float runSpeed; //奔跑移动速度
    public float rotationSpeed; //旋转速度
    public int health; //血量
    public int endurance; //耐力
    public float defense; //防御力
    public float attack; //攻击力
    
    //战斗相关
    // [SerializeField] protected LayerMask playerLayer;
    // [SerializeField, Header("攻击目标")] protected Transform currentTarget = null;
    // [SerializeField] protected GameObject attacker;
    
    //巡逻
    //public Transform[] patrolPoints;

    protected virtual void Start()
    {
        //enemyView = GetComponent<EnemyView>();
    }
    
    
    //敌人公共方法
    //受到攻击
    // public override void OnHit(ComboInteractionConfig interactionConfig, Transform attacker)
    // {
    //     base.OnHit(interactionConfig, attacker);
    //     FindTarget();
    //     LookAtTarget();
    // }
    //
    // private void FindTarget()
    // {
    //     //若已经有目标，则返回
    //     if(currentTarget)
    //         return;
    //     Collider[] target = new Collider[1];
    //     var size = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(4, 4, 4), target, Quaternion.identity, playerLayer);
    //     if (size != 0)
    //     {
    //         currentTarget = target[0].transform;
    //     }
    // }
    //
    // private void LookAtTarget()
    // {
    //     if(!currentTarget)
    //         return;
    //     Vector3 dir = currentTarget.position - transform.position;
    //     transform.forward = dir.normalized;
    // }
}
