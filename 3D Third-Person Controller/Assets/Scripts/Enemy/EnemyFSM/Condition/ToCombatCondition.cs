using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

[CreateAssetMenu(fileName = "ToCombatCondition", menuName = "StateMachine/Condition/ToCombatCondition")]
public class ToCombatCondition : ConditionSO
{
    public override bool ConditionSetUp()
    {
        return enemyCombatController.GetCurrentTarget() ? true : false;
    }
}
