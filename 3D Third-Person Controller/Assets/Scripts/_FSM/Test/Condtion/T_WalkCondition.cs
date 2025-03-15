using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "T_WalkCondition", menuName = "StateMachine/Transition/New T_WalkCondition")]
public class T_WalkCondition : ConditionSO
{

    public float time;
    public float maxTime;
    
    
    public override bool ConditionSetUp()
    {
        if(time > 0) 
        {
            time -= Time.deltaTime;
            if (time <= 0)
                return true;
        }

        return false;
    }
}
