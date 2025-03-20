using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponAttack : MonoBehaviour
{
    [SerializeField] private EnemyCombatController enemyCombatController;

    private void OnTriggerEnter(Collider other)
    {
        //若击打到Player碰撞体
        if (other.CompareTag("Player"))
        {
            //执行攻击到玩家的逻辑
            enemyCombatController.HitPlayer(other);
        }
    }
}
