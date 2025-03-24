using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Assembly = System.Reflection.Assembly;
using Random = UnityEngine.Random;


public class EnemyCombatController : CombatControllerBase
{
    //组件
    private EnemyView enemyView;
    private EnemyBase enemyParameter;
    private EnemyMovementController enemyMovementController;
    private CinemachineImpulseSource cinemachineImpulseSource;
    private EnemyAttackDetection enemyAttackDetection;

    //战斗相关
    [Header("战斗相关")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Transform currentTarget = null;
    [SerializeField] protected GameObject attacker;

    [Header("技能")] 
    [SerializeField] private List<CombatAbilityBase> abilityList = new List<CombatAbilityBase>();
    [SerializeField] public List<CombatAbilityBase> availableAbilityList = new List<CombatAbilityBase>();
    
    [Header("攻击检测")] 
    private int attackConfigCount; //当前攻击配置信息索引
    private AbilityConfig currentAbilityConfig; //当前攻击配置信息

    [Header("玩家射线检测")] 
    [SerializeField] private Transform lockOnTransform;
    
    
    private int lockOnHash;

    private void Start()
    {
        base.Start();
        enemyView = GetComponent<EnemyView>();
        enemyParameter = GetComponent<EnemyBase>();
        enemyMovementController = GetComponent<EnemyMovementController>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        enemyAttackDetection = GetComponent<EnemyAttackDetection>();

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
    public override void OnHit(ComboInteractionConfig interactionConfig, AttackFeedbackConfig attackFeedbackConfig, Transform attacker)
    {
        //若可以受击且血量大于0，才能执行受击逻辑（否则要么在硬直，要么敌人已经处于死亡状态）
        if(!canBeHit || enemyParameter.health <= 0)
            return;
        base.OnHit(interactionConfig, attackFeedbackConfig, attacker);

        //受击时停止攻击检测，防止因为受击而导致攻击检测不会停止
        enemyAttackDetection.EndAttacking();
        
        //计算敌人攻击方向
        Vector3 dir = (attacker.position - this.transform.position).normalized;
        // 计算与前方和右侧的夹角
        float angleForward = Vector3.Angle(dir, transform.forward);
        
        //处理受伤逻辑
        int healthDamage = interactionConfig.healthDamage + Random.Range(-10, 10);
        Debug.Log("受击了!受到了来自" + interactionConfig.weaponType + "的" +  healthDamage + "点伤害!");
        enemyParameter.health -= healthDamage; //扣血
        if (enemyParameter.health <= 0)
        {
            enemyParameter.health = 0; 
            Debug.Log("敌人似了!"); 
            //进敌人死亡状态，播放死亡动画，切换状态机
            if (angleForward < 90f)
            {
                animator.CrossFadeInFixedTime("Die_Front", 0.15f, 0, 0);
            }
            else
            {
                animator.CrossFadeInFixedTime("Die_Back", 0.15f, 0, 0);
            }
            return;
        }   
        
        //处理耐力逻辑
        if (enemyParameter.endurance > 0) //还有耐力时
        {
            int enduranceDamage = interactionConfig.enduranceDamage + Random.Range(-10, 10);
            Debug.Log(string.Format("减少了{0}点耐力，当前耐力为{1}点!",  interactionConfig.enduranceDamage, enemyParameter.endurance));
            enemyParameter.endurance -= enduranceDamage; //扣耐力
            //若耐力归零，则出大硬直
            if (enemyParameter.endurance <= 0)
            {
                enemyParameter.endurance = 0;
                Debug.Log("敌人耐力清空");
                
                //TODO: 播放耐力清零动画，出大硬直
                //TODO: 可以添加子弹时间效果
                if (angleForward < 90f)
                {
                    animator.CrossFadeInFixedTime("KnockDown_Front", 0.15f, 0, 0);
                }
                else
                {
                    animator.CrossFadeInFixedTime("KnockDown_Back", 0.15f, 0, 0);
                }
            }
            //若没有归零，则播放Hit层的上半身受击动画
            else
            {
                //播放Hit层的受击动画
                animator.CrossFadeInFixedTime(interactionConfig.hitName, 0.1555f, 1, 0);
            }
        }
        else //没有耐力时
        {
            //若是大剑攻击或大硬直动画，则播放Hit层的受击动画（大剑攻击无法被打断）
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("GSAbility") || animator.GetCurrentAnimatorStateInfo(0).IsTag("HitStun"))
            {
                animator.CrossFadeInFixedTime(interactionConfig.hitName, 0.1555f, 1, 0);
            }
            //若不是大剑攻击，则播放Base层的受击动画（非大剑攻击可以被打断）
            else
            {
                //播放Base层的受击动画
                animator.CrossFadeInFixedTime(interactionConfig.hitName, 0.1555f, 0, 0);
            }
        }
        
        //处理敌人旋转
        FindTarget();
        LookAtTarget();
        
        //处理攻击反馈
        if (attackFeedbackConfig != null)
        {
            cinemachineImpulseSource.GenerateImpulseWithVelocity(attackFeedbackConfig.velocity); //屏幕震动
            StartCoroutine(IE_HitAudioSound(attackFeedbackConfig.audioStartTime, attackFeedbackConfig.audioClip)); //播放受击音效
            SetAnimatorSpeed(attackFeedbackConfig.animatorSpeed); //顿帧效果
            Invoke(nameof(ResetAnimatorSpeed), attackFeedbackConfig.stopFrameTime);
        }
    }

    IEnumerator IE_HitAudioSound(float countTime, AudioClip audioClip)
    {
        while (countTime > 0)
        {
            yield return null;
            countTime -= Time.deltaTime;
        }
        audioSource.PlayOneShot(audioClip, 0.1f); //播放受击音 
    }

    private void FindTarget()
    {
        //若已经有目标，则返回
        if (currentTarget)
            return;
        Collider[] target = new Collider[1];
        var size = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(4, 4, 4), target, Quaternion.identity,
            playerLayer);
        if (size != 0)
        {
            currentTarget = target[0].transform;
        }
    }

    private void LookAtTarget()
    {
        if (!currentTarget)
            return;
        Vector3 dir = currentTarget.position - transform.position;
        //transform.forward = dir.normalized;
        // 目标旋转
        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized);
        // 当前旋转逐渐过渡到目标旋转
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, enemyParameter.rotationSpeed * Time.deltaTime);
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

    /// <summary>
    /// 攻击到玩家时执行的逻辑
    /// </summary>
    public void HitPlayer(Collider playerCollider)
    {
        playerCollider.GetComponent<PlayerCombatController>().PlayerOnHit(currentAbilityConfig.detectionConfigs[attackConfigCount], this.transform);
    }
    
    #region 公共接口
    
    public Transform GetCurrentTarget()
    {
        if (!currentTarget)
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
        if (!currentTarget)
            return Vector3.zero;
        return (currentTarget.position - transform.position).normalized;
    }
    
    public Transform GetLockOnTransform() => lockOnTransform;

    public void SetAttackConfigCount(int count) => attackConfigCount = count;
    
    public void SetCurrentAbilityConfig(AbilityConfig abilityConfig) => currentAbilityConfig = abilityConfig;
    
    #endregion

    #region 技能

    /// <summary>
    /// 初始化所有技能
    /// </summary>
    private void InitAllAbilities()
    {
        if (abilityList.Count == 0)
            return;
        for (int i = 0; i < abilityList.Count; i++)
        {
            //初始化每个技能
            abilityList[i].Init(animator, this, enemyMovementController, enemyParameter);
            //将技能设为可用
            abilityList[i].SetAbilityAvailable(true);
            //将技能加入可用列表中，等待选择
            availableAbilityList.Add(abilityList[i]);
        }
    }

    /// <summary>
    /// 获得一个可用的（不在冷却中）技能 
    /// </summary>
    public CombatAbilityBase GetAnAvailableAbility()
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i].GetAbilityAvailable())
                return abilityList[i];
        }

        return null;
    }

    /// <summary>
    /// 随机返回一个可用技能
    /// </summary>
    public CombatAbilityBase GetRandomAvailableAbility()
    {
        if (availableAbilityList.Count == 0)
            return null;
        return availableAbilityList[UnityEngine.Random.Range(0, availableAbilityList.Count)];
    }

    /// <summary>
    /// 根据技能名，获得指定的可用技能（若该技能在冷却中则返回null）
    /// </summary>
    public CombatAbilityBase GetAbilityByName(string abilityName)
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i].GetAbilityName() == abilityName)
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
            if (abilityList[i].GetAbilityID() == abilityID)
                return abilityList[i];
        }

        return null;
    }

    #endregion
}
