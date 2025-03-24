using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AICombat", menuName = "StateMachine/State/AICombat")]
public class AICombat : StateActionSO
{
    //TODO: 将来应该重构UI逻辑
    private BossHealthAndEndurance bossHealthAndEndurance;
    
    private EnemyAttackAnimation enemyAttackAnimation;
    
    [SerializeField] private float backwardDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float runDistance;
    //[SerializeField] private float abilityCooldownTime; //使用技能的间隔时间
    
    //private bool canUseAbility = true;
    
    //当前技能
    [SerializeField] private CombatAbilityBase currentAbility;
    
    private int verticalHash = Animator.StringToHash("Vertical");
    private int horizontalHash = Animator.StringToHash("Horizontal");
    private int moveSpeedHash = Animator.StringToHash("MoveSpeed");

    private int randomHorizontal;

    protected override void Init(StateMachineSystem stateMachineSystem)
    {
        base.Init(stateMachineSystem);
        enemyAttackAnimation = stateMachineSystem.GetComponent<EnemyAttackAnimation>();
        
        //TODO: 将来应该重构UI逻辑
        bossHealthAndEndurance = stateMachineSystem.GetComponent<BossHealthAndEndurance>();
    }

    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        base.OnEnter(stateMachineSystem);
        animator.Play("WakeUp"); //播放苏醒动画
        
        //TODO: UI逻辑需要重构
        bossHealthAndEndurance.AppearBar(); //显示UI
    }

    public override void OnUpdate()
    {
        CombatAction();
        LookAtTarget();
    }

    public override void OnExit()
    {

    }
    
    private void NoCombatMove()
    {
        //若正在处于无法被打断的动画片段，或正处于动画过渡状态，则不执行
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Ability") ||
            animator.IsInTransition(0) || animator.GetCurrentAnimatorStateInfo(0).IsTag("GSAbililty"))
            return;
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && !Mathf.Approximately(enemyCombatController.GetCurrentTargetDistance(), -1f))
        {
            //玩家距离小于攻击距离，则进行攻击
            if (enemyCombatController.GetCurrentTargetDistance() < attackDistance)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || !animator.GetCurrentAnimatorStateInfo(0).IsTag("Ability") ||
                    !animator.GetCurrentAnimatorStateInfo(0).IsTag("GSAbililty"))
                {
                    //TODO:待添加
                    animator.Play("Normal01");
                }
            }
            //玩家距离小于后退距离，则向后退
            else if (enemyCombatController.GetCurrentTargetDistance() < backwardDistance)
            {
                animator.SetFloat(verticalHash, -1f, 0.1f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.1f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.1f, Time.deltaTime);

                randomHorizontal = GetRandomHorizontal();
            }
            //玩家距离大于后退距离且小于追击距离，则进行平移
            else if (enemyCombatController.GetCurrentTargetDistance() > backwardDistance && enemyCombatController.GetCurrentTargetDistance()< chaseDistance)
            {
                animator.SetFloat(verticalHash, 0f,0.1f, Time.deltaTime);
                animator.SetFloat(horizontalHash, randomHorizontal, 0.1f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.1f, Time.deltaTime);
            }
            //玩家距离大于追击距离，则向玩家移动
            else if (enemyCombatController.GetCurrentTargetDistance() > chaseDistance && enemyCombatController.GetCurrentTargetDistance() < runDistance)
            {
                animator.SetFloat(verticalHash, 1f, 0.1f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.1f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.1f, Time.deltaTime);
              
                randomHorizontal = GetRandomHorizontal();
            }
            //玩家距离大于奔跑追击距离，则奔跑着向玩家移动
            else if (enemyCombatController.GetCurrentTargetDistance() > runDistance)
            {
                animator.SetFloat(verticalHash, 1f, 0.1f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.1f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.runSpeed, 0.1f, Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat(verticalHash, 0f, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontalHash, 0f, 0.1f, Time.deltaTime);
            animator.SetFloat(moveSpeedHash, 0f, 0.1f, Time.deltaTime);
        }
    }
    
    private void LookAtTarget()
    {
        Transform target = enemyCombatController.GetCurrentTarget();
        if(!target)
            return;
        // 平滑过渡到目标旋转
        transform.forward = Vector3.Lerp(transform.forward, target.transform.position - transform.position, Time.deltaTime * enemyParameter.rotationSpeed);
    }

    /// <summary>
    /// 敌人战斗行为
    /// </summary>
    private void CombatAction()
    {
        //若当前没有技能，则执行NoCombatMove，进行移动
        if (!currentAbility)
        {
            NoCombatMove();
            if (Mathf.Approximately(enemyCombatController.GetCurrentTargetDistance(), -1f))
            {
                //TODO: 补充玩家不在敌人视野中的情况
            }
            GetAbility();
        }
        //有技能则使用技能
        else if(currentAbility)
        {
            currentAbility.InvokeAbility();
            //当前技能切换为不可用状态（即已经释放结束）
            if (currentAbility.GetAbilityAvailable() == false)
            {
                //丢弃当前技能
                currentAbility = null;
            }
        }
    }

    /// <summary>
    /// 获取技能
    /// </summary>
    private void GetAbility()
    {
        //TODO: 考虑添加一个释放技能的CD时间，以控制敌人AI的攻击欲望
        if (!currentAbility && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Ability") &&
            !animator.IsInTransition(0))
        {
            //获取一个可用的技能
            currentAbility = enemyCombatController.GetRandomAvailableAbility();
        }
    }
    
    
    #region 工具方法

    private int GetRandomHorizontal()
    {
        int randomNum = Random.Range(0, 100);
        return randomNum > 50 ? 1 : -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawRay(transform.position + Vector3.up, -enemyCombatController.GetDirectionForTarget());
    }

    #endregion
}
