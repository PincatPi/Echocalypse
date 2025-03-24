using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttackCheckGizmos : MonoBehaviour
{
    private Animator animator;
    
    //敌人层级
    [SerializeField] protected LayerMask enemyLayer;
    //是否正在攻击
    [SerializeField] protected bool isAttacking = false;
    //当前的武器类型
    [SerializeField] public E_WeaponType weaponType = E_WeaponType.Empty;
    //对应武器和攻击检测点组的字典
    protected Dictionary<E_WeaponType, Transform[]> attackCheckPointsOfWeapon = new Dictionary<E_WeaponType, Transform[]>();
    //太刀攻击检测点
    public Transform[] katanaCheckPoints;
    //大剑攻击检测点
    public Transform[] greatSwordCheckPoints;
    
    //武器上的攻击检测点
    [SerializeField] protected Transform[] attackCheckPoints;
    //上一次检测时检测点的位置
    [SerializeField] protected Vector3[] lastCheckPointsPosition;
    //检测时间间隔
    public float timeBetweenCheck;
    //计时器
    protected float timeCounter;
    //是否是第一次检测
    protected bool isFirstCheck = true;
    protected RaycastHit[] enemiesRaycastHits;
    
    //本次攻击的交互数据
    protected ComboInteractionConfig comboInteractionConfig;
    //本次攻击的反馈数据
    protected AttackFeedbackConfig attackFeedbackConfig;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        //注册不同武器的攻击检测点
        attackCheckPointsOfWeapon.Add(E_WeaponType.Katana, katanaCheckPoints);
        attackCheckPointsOfWeapon.Add(E_WeaponType.GreatSword, greatSwordCheckPoints);
    }

    protected virtual void Update()
    {
        if (isAttacking)
        {
            timeCounter += Time.deltaTime;
        }
        SwitchAttackCheckPoints();
    }

    protected virtual void FixedUpdate()
    {
        AttackCheck();
    }

    protected virtual void SwitchAttackCheckPoints()
    {
        switch (weaponType)
        {
            case E_WeaponType.Empty:
                break;
            
            case E_WeaponType.Katana:
                attackCheckPoints = attackCheckPointsOfWeapon[E_WeaponType.Katana];
                enemiesRaycastHits = new RaycastHit[attackCheckPoints.Length];
                break;
            
            case E_WeaponType.GreatSword:
                attackCheckPoints = attackCheckPointsOfWeapon[E_WeaponType.GreatSword];
                enemiesRaycastHits = new RaycastHit[attackCheckPoints.Length];
                break;
            
            case E_WeaponType.Bow:
                //功能待添加
                break;
        }
    }
    
    public virtual void AttackCheck()
    {
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
                                    EnemyCombatController enemyHit = enemy.transform.gameObject.GetComponent<EnemyCombatController>();
                                    if (enemyHit)
                                    {
                                        enemyHit.OnHit(comboInteractionConfig, attackFeedbackConfig, this.transform); //调用受击函数   
                                        SetAnimatorSpeed(attackFeedbackConfig.animatorSpeed); //对玩家进行顿帧
                                        Invoke(nameof(ResetAnimatorSpeed), attackFeedbackConfig.stopFrameTime); //结束顿帧
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

    private void SetAnimatorSpeed(float speed) => animator.speed = speed;

    private void ResetAnimatorSpeed() => animator.speed = 1f;
    
    public void StartAttacking(ComboInteractionConfig comboConfig, AttackFeedbackConfig feedbackConfig)
    {
        isAttacking = true;
        comboInteractionConfig = comboConfig;
        attackFeedbackConfig = feedbackConfig;
    }

    public void EndAttacking()
    {
        isAttacking = false;
        comboInteractionConfig = null;
        attackFeedbackConfig = null;
    }
}
