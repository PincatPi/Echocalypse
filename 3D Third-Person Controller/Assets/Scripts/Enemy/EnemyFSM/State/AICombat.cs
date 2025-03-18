using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AICombat", menuName = "StateMachine/State/AICombat")]
public class AICombat : StateActionSO
{
    [SerializeField] private float backwardDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float runDistance;

    private int verticalHash = Animator.StringToHash("Vertical");
    private int horizontalHash = Animator.StringToHash("Horizontal");
    private int moveSpeedHash = Animator.StringToHash("MoveSpeed");

    private int randomHorizontal;
    
    // public override void OnUpdate()
    // {
    //     Debug.Log("此时处于AICombat状态");
    //     LookAtTarget();
    //     NoCombat();
    // }

    // private void NoCombat()
    // {
    //     //若不能攻击，则远离玩家
    //     if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
    //     {
    //         if (enemyCombatController.GetCurrentTargetDistance() < backwardDistance && enemyCombatController.GetCurrentTargetDistance() > 0f)
    //         {
    //             Debug.Log("开始慢速后退");
    //             //往玩家反方向慢速移动
    //             //TODO: 补全代码
    //         
    //             //TODO: Animator修改动画
    //             if (enemyCombatController.GetCurrentTargetDistance() < fastBackwardDistance && enemyCombatController.GetCurrentTargetDistance() > 0f)
    //             {
    //                 //animator.CrossFadeInFixedTime("Roll_Back", 0.1f);
    //                 //先面朝向玩家，然后向后翻滚
    //                 animator.Play("Roll_Back");
    //             }
    //         }
    //     }
    // }
    //

    public override void OnUpdate()
    {
        NoCombatMove();
        LookAtTarget();
    }

    public override void OnExit()
    {

    }
    
    private void NoCombatMove()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && !Mathf.Approximately(enemyCombatController.GetCurrentTargetDistance(), -1f))
        {
            //玩家距离小于攻击距离，则进行攻击
            if (enemyCombatController.GetCurrentTargetDistance() < attackDistance)
            {
                Debug.Log("攻击");
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || !animator.GetCurrentAnimatorStateInfo(0).IsTag("Defense"))
                {
                    //TODO:待添加
                    animator.Play("Normal01");
                    //animator.CrossFade("Combo01_1", 0.25f);
                }
            }
            //玩家距离小于后退距离，则向后退
            else if (enemyCombatController.GetCurrentTargetDistance() < backwardDistance)
            {
                Debug.Log("后退");
                //TODO: 检查此处是否需要加负号
                //enemyMovementController.CharacterMoveInterface(-enemyCombatController.GetDirectionForTarget(), enemyParameter.walkSpeed, true);
                animator.SetFloat(verticalHash, -1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.25f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.25f, Time.deltaTime);

                randomHorizontal = GetRandomHorizontal();
            }
            //玩家距离大于后退距离且小于追击距离，则进行平移
            else if (enemyCombatController.GetCurrentTargetDistance() > backwardDistance && enemyCombatController.GetCurrentTargetDistance()< chaseDistance)
            {
                Debug.Log("平移");
                //enemyMovementController.CharacterMoveInterface(enemyMovementController.transform.right * ((randomHorizontal == 0) ? 1 : randomHorizontal), enemyParameter.walkSpeed, true);
                animator.SetFloat(verticalHash, 0f,0.25f, Time.deltaTime);
                Debug.Log(randomHorizontal);
                animator.SetFloat(horizontalHash, randomHorizontal, 0.25f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.25f, Time.deltaTime);
            }
            //玩家距离大于追击距离，则向玩家移动
            else if (enemyCombatController.GetCurrentTargetDistance() > chaseDistance && enemyCombatController.GetCurrentTargetDistance() < runDistance)
            {
                Debug.Log("向玩家走来");
                //enemyMovementController.CharacterMoveInterface(enemyMovementController.transform.forward, enemyParameter.walkSpeed, true);
                animator.SetFloat(verticalHash, 1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.25f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.25f, Time.deltaTime);
              
                randomHorizontal = GetRandomHorizontal();
            }
            //玩家距离大于奔跑追击距离，则奔跑着向玩家移动
            else if (enemyCombatController.GetCurrentTargetDistance() > runDistance)
            {
                Debug.Log("向玩家跑来");
                //enemyMovementController.CharacterMoveInterface(enemyMovementController.transform.forward, enemyParameter.runSpeed, true);
                animator.SetFloat(verticalHash, 1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.25f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, enemyParameter.runSpeed, 0.25f, Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat(verticalHash, 0f, 0.5f, Time.deltaTime);
            animator.SetFloat(horizontalHash, 0f, 0.5f, Time.deltaTime);
            animator.SetFloat(moveSpeedHash, 0f, 0.5f, Time.deltaTime);
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
}
