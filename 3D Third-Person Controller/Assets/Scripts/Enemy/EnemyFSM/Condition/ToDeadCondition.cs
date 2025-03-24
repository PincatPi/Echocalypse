using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToDeadCondition", menuName = "StateMachine/Condition/ToDeadCondition")]
public class ToDeadCondition : ConditionSO
{
    public override bool ConditionSetUp()
    {
        return enemyParameter.health <= 0;
    }
}
