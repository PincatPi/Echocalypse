using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

[Serializable]
public struct ComboDictStruct
{
    public E_WeaponType weaponType;
    public ComboList comboList;
}

public class PlayerCombatController : CombatControllerBase
{
    #region 组件
    
    private CharacterController controller;
    private ThirdPersonController thirdPersonController;
    private AttackCheckGizmos attackCheck;
    
    private InputAction movementInputAction;
    private InputAction attackAction;
    
    #endregion

    public float attack = 1f;
    private E_AttackType attackType = E_AttackType.Common;
    public E_WeaponType weaponType = E_WeaponType.Empty;
    [SerializeField] private ComboDictStruct[] comboDictStructs;
    private Dictionary<E_WeaponType, ComboList> comboListDict;
    [SerializeField] private bool canPlayHitAnim;
    
    private int rollHash;

    void Awake()
    {
        comboListDict = new Dictionary<E_WeaponType, ComboList>();
        //将对应的连招表添加到字典中
        foreach (ComboDictStruct comboDict in comboDictStructs)
        {
            comboListDict.Add(comboDict.weaponType, comboDict.comboList);
        }
    }
    
    void Start()
    {
        base.Start();
        controller = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        attackCheck = GetComponent<AttackCheckGizmos>();
        
        movementInputAction = GetComponent<PlayerInput>().actions["PlayerMovement"];
        attackAction = GetComponent<PlayerInput>().actions["Attack"];
        
        rollHash = Animator.StringToHash("Roll");

        canPlayHitAnim = true;
    }
    
    void Update()
    {
        base.Update();
    }

    public void SwitchComboList(E_WeaponType _weaponType)
    {
        if (!comboListDict.ContainsKey(_weaponType))
            return;
        currentComboList = comboListDict[_weaponType];
    }

    /// <summary>
    /// 玩家受击逻辑
    /// </summary>
    public void PlayerOnHit(EnemyAttackDetectionConfig attackConfig, Transform attackerTransform)
    {
        if(!canBeHit)
            return;
        canBeHit = false;
        
        //禁用玩家移动和攻击输入
        movementInputAction.Disable();
        attackAction.Disable();
        
        int damage = attackConfig.damage + Random.Range(-10, 10);
        //TODO: 扣除生命值等逻辑
        Debug.Log("玩家受到了" + damage + "点伤害!");

        if (canPlayHitAnim)
        {
            Vector3 dir = (attackerTransform.position - this.transform.position).normalized;
            //float dot = Vector3.Dot(dir, this.transform.forward);
            // //在正前方90度内
            //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        
            // 计算与前方和右侧的夹角
            float angleForward = Vector3.Angle(dir, transform.forward);
            float angleRight = Vector3.Angle(dir, transform.right);

            // 判断方位
            if (angleForward <= 45f) // 前方90度范围内
            {
                animator.Play("Hit_Front_" + weaponType.ToString());
            }
            else if (angleForward >= 135f) // 后方90度范围内
            {
                animator.Play("Hit_Back_" + weaponType.ToString());
            }
            else if (angleRight <= 45f) // 右侧90度范围内
            {
                animator.Play("Hit_Right_" + weaponType.ToString());
            }
            else if (angleRight >= 135f) // 左侧90度范围内
            {
                animator.Play("Hit_Left_" + weaponType.ToString());
            }
        }
        
        //无敌时间计时
        StartCoroutine(IE_HitCoolDown(hitCoolDown));
    }
    
    private IEnumerator IE_HitCoolDown(float coolDownTime)
    {
        while (coolDownTime > 0)
        {
            yield return null;
            coolDownTime -= Time.deltaTime;
        }
        canBeHit = true;
        //启用玩家移动和攻击输入
        movementInputAction.Enable();
        attackAction.Enable();
    }

    #region 玩家输入相关

    public void GetAttackInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started && weaponType != E_WeaponType.Empty)
        {
            ExecuteCombo(); 
        }
    }
    
    //获取玩家闪避输入
    public void GetSlideInput(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is TapInteraction && canExecuteCombo)
        {
            animator.SetTrigger(rollHash);
        }
    }
    
    #endregion
    
}
