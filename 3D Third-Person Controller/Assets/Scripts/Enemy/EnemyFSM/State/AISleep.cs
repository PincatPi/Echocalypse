using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AISleep", menuName = "StateMachine/State/AISleep")]
public class AISleep : StateActionSO
{
    public override void OnUpdate()
    {
        Debug.Log("此时处于AISleep状态");
    }
}
