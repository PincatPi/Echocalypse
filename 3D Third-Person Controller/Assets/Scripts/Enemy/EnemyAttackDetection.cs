using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyAttackDetection : AttackCheckGizmos
{
    private EnemyCombatController enemyCombatController;
    
    private void Start()
    {
        base.Start();
        weaponType = E_WeaponType.Katana;
        enemyCombatController = GetComponent<EnemyCombatController>();
    }

    private void Update()
    {
        // if (isAttacking)
        // {
        //     timeCounter += Time.deltaTime;
        // }
        SwitchAttackCheckPoints();
        //AttackCheck();
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            timeCounter += Time.fixedDeltaTime;
        }
        AttackCheck();
    }

    
    public override void AttackCheck()
    {
        //TODO: 检查这句是否可以删除
        if(weaponType == E_WeaponType.Empty)
            return;
        
        //若当时处于攻击状态
        if (isAttacking)
        {
            if (timeCounter >= timeBetweenCheck)
            {
                //如果是第一次检查，则不进行检测
                if (isFirstCheck)
                {
                    lastCheckPointsPosition = new Vector3[attackCheckPoints.Length];
                    //将isFirstCheck置false
                    isFirstCheck = false;
                }
                //不是第一次检查，则进行检测
                else
                {
                    for (int i = 0; i < attackCheckPoints.Length; i++)
                    {
                        //进行射线检测
                        Ray ray = new Ray(lastCheckPointsPosition[i], (attackCheckPoints[i].position - lastCheckPointsPosition[i]).normalized);
                        int length = Physics.RaycastNonAlloc(ray, enemiesRaycastHits, Vector3.Distance(attackCheckPoints[i].position, lastCheckPointsPosition[i]), enemyLayer);
                        //若检测到了敌人
                        if (length > 0)
                        {
                            foreach (RaycastHit enemy in enemiesRaycastHits)
                            {
                                if (enemy.transform)
                                {
                                    PlayerCombatController playerHit = enemy.transform.gameObject.GetComponent<PlayerCombatController>();
                                    if (playerHit)
                                    {
                                        Debug.Log("打到了玩家!");
                                        enemyCombatController.HitPlayer(enemy.collider);   
                                    }
                                }
                            }
                        }
                        //绘制从上一次记录的该点的位置到当前该点的位置的线段
                        Debug.DrawRay(lastCheckPointsPosition[i], (attackCheckPoints[i].position - lastCheckPointsPosition[i]).normalized * Vector3.Distance(attackCheckPoints[i].position, lastCheckPointsPosition[i]), Color.red, 2f);
                    }
                }
                //记录上一次Check时攻击判定点的位置
                for (int i = 0; i < attackCheckPoints.Length; i++)
                {
                    lastCheckPointsPosition[i] = attackCheckPoints[i].position;
                }
                timeCounter = 0f; //计时器归零，重新开始计时
            }
        }
        else
        {
            isFirstCheck = true;
            lastCheckPointsPosition = null;
        }
    }

    public void StartAttacking()
    {
        isAttacking = true;
    }

    public void EndAttacking()
    {
        isAttacking = false;
    }
}
