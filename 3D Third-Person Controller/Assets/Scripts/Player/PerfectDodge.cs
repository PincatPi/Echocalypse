using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PerfectDodge : MonoBehaviour
{
    [SerializeField] private PlayerCombatController playerCombatController;
    private bool canTriggerPerfectDodge;

    private void Start()
    {
        canTriggerPerfectDodge = true;
    }

    public void PerfectDodgeInterface()
    {
        if (canTriggerPerfectDodge)
        {
            //执行完美闪避的逻辑
            canTriggerPerfectDodge = false;
            playerCombatController.PerfectDodge();
            StartCoroutine(IE_CanPerfectDodgeTimeCount(playerCombatController.GetCanPerfectDodgeTime()));
        }
    }

    IEnumerator IE_CanPerfectDodgeTimeCount(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        canTriggerPerfectDodge = true;
    }
}
