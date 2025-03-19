using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "大剑跳杀", menuName = "Abilities/大剑跳杀")]
public class Ability4 : CombatAbilityBase
{
    public override void InvokeAbility()
    {
        //若当前还没有使用技能或攻击
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && abilitiyIsAvailable)
        {
            //当技能被激活时，还没有进入允许释放的距离，则向玩家接近
            if (combatController.GetCurrentTargetDistance() > abilityUseDistance)
            {
                animator.SetFloat(verticalHash, 1f, 0.1f, Time.deltaTime);
                animator.SetFloat(horizontalHash, 0f, 0.1f, Time.deltaTime);
                //距离太远时跑步
                if (combatController.GetCurrentTargetDistance() > abilityUseDistance + 5f)
                {
                    animator.SetFloat(moveSpeedHash, enemyParameter.runSpeed, 0.1f, Time.deltaTime);
                }
                //距离较近时走路
                else
                {
                    animator.SetFloat(moveSpeedHash, enemyParameter.walkSpeed, 0.1f, Time.deltaTime);
                }
            }
            //若已经进入允许释放的距离，则释放技能
            else if (combatController.GetCurrentTargetDistance() < abilityUseDistance)
            {
                UseAbility();
            }
        }
    }
}
