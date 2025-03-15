using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
    
    #endregion

    public float attack = 1f;
    private E_AttackType attackType = E_AttackType.Common;
    public E_WeaponType weaponType = E_WeaponType.Empty;
    [SerializeField] private ComboDictStruct[] comboDictStructs;
    private Dictionary<E_WeaponType, ComboList> comboListDict;
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
        rollHash = Animator.StringToHash("Roll");
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
