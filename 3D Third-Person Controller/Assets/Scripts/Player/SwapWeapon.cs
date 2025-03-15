using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class SwapWeapon : MonoBehaviour
{
    #region 组件
    
    private Animator animator;
    private ThirdPersonController thirdPersonController;
    public Transform effectTransform;
    public TwoBoneIKConstraint[] rightHandIKConstraints;
    private TwoBoneIKConstraint currentRightHandIKConstraint;
    public TwoBoneIKConstraint[] leftHandIKConstraints;
    private TwoBoneIKConstraint currentLeftHandIKConstraint;
    private AttackCheckGizmos attackCheck;
    private PlayerCombatController playerCombatController;
    
    #endregion
    
    private E_AttackType attackType = E_AttackType.Common;
    private E_WeaponType weaponType = E_WeaponType.Empty;
    public E_WeaponType WeaponType => weaponType;
    
    public GameObject[] weaponOnBack;
    public GameObject[] weaponInHand;

    private int equipHash;
    
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        currentRightHandIKConstraint = rightHandIKConstraints[0];
        currentLeftHandIKConstraint = leftHandIKConstraints[0];
        attackCheck = GetComponent<AttackCheckGizmos>();
        playerCombatController = GetComponent<PlayerCombatController>();
        animator = GetComponent<Animator>();
        
        equipHash = Animator.StringToHash("WeaponType");
    }
    
    void Update()
    {
        //设置动画状态
        SetAnimator();
    }

    private void SetAnimator()
    {
        //装备状态
        animator.SetInteger(equipHash, (int)weaponType);
        //控制掏出武器和收起武器时的右手IK权重
        currentRightHandIKConstraint.weight = animator.GetFloat("Right Hand Weight");
        currentLeftHandIKConstraint.weight = animator.GetFloat("Left Hand Weight");
    }
    
    #region 动画片段调用函数
    
    /// <summary>
    /// 切换背部武器和手部武器的显示
    /// </summary>
    /// <param name="weaponType">表示武器的位置是在背上0还是手上1\2\3</param>
    public void PutGrabWeapon(int weaponType)
    {
        //isOnBack为true时是装备武器，为false时是收回武器
        bool isOnBack = weaponOnBack[weaponType].activeSelf;
        weaponOnBack[weaponType].SetActive(!isOnBack);
        weaponInHand[weaponType].SetActive(isOnBack);
    }
    
    #endregion
    
    #region 玩家输入相关
    
    //判断是否接收玩家攻击输入
    private bool IsInputValid()
    {
        if (thirdPersonController.armState != ThirdPersonController.ArmState.Equip)
        {
            return false;
        }
        return true;
    }
    
    //获取玩家武器装备输入
    public void GetKatanaInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            //若当前手上没有武器
            if (weaponType == E_WeaponType.Empty)
            {
                weaponType = E_WeaponType.Katana;
                thirdPersonController.isEquip = true;
                //将当前有效的IK约束设置为Katana的IK约束
                currentRightHandIKConstraint = rightHandIKConstraints[(int)E_WeaponType.Katana];
                currentLeftHandIKConstraint = leftHandIKConstraints[(int)E_WeaponType.Katana];
            }
            //若手上有武器，则收回该武器
            else
            {
                weaponType = E_WeaponType.Empty;
                thirdPersonController.isEquip = false;
            }
            attackCheck.weaponType = weaponType;
            
            playerCombatController.weaponType = weaponType;
            //切换连招表
            playerCombatController.SwitchComboList(weaponType);
        }
    }
    public void GetGreatSwordInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (weaponType == E_WeaponType.Empty)
            {
                weaponType = E_WeaponType.GreatSword;
                thirdPersonController.isEquip = true;
                //将当前有效的IK约束设置为GreatSword的IK约束
                currentRightHandIKConstraint = rightHandIKConstraints[(int)E_WeaponType.GreatSword];
                currentLeftHandIKConstraint = leftHandIKConstraints[(int)E_WeaponType.GreatSword];
            }
            else
            {
                weaponType = E_WeaponType.Empty;
                thirdPersonController.isEquip = false;
            }
            attackCheck.weaponType = weaponType;
            
            playerCombatController.weaponType = weaponType;
            //切换连招表
            playerCombatController.SwitchComboList(weaponType);
        }
    }
    public void GetBowInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (weaponType == E_WeaponType.Empty)
            {
                weaponType = E_WeaponType.Bow;
                thirdPersonController.isEquip = true;
                //将当前有效的IK约束设置为Bow的IK约束
                currentRightHandIKConstraint = rightHandIKConstraints[(int)E_WeaponType.Bow];
                currentLeftHandIKConstraint = leftHandIKConstraints[(int)E_WeaponType.Bow];
            }
            else
            {
                weaponType = E_WeaponType.Empty;
                thirdPersonController.isEquip = false;
            }
            attackCheck.weaponType = weaponType;
            
            playerCombatController.weaponType = weaponType;
            //切换连招表
            playerCombatController.SwitchComboList(weaponType);
        }
    }
    
    //获取玩家闪避输入
    // public void GetSlideInput(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.interaction is TapInteraction && IsInputValid())
    //     {
    //         animator.SetTrigger("Roll");
    //     }
    // }
    
    //TEST: 暂停时间（调试用，发布时删除）
    public void StopTime(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if(Time.timeScale == 0f)
                Time.timeScale = 1f;
            else if(Time.timeScale == 1f)
                Time.timeScale = 0f;
        }
    }
    
    #endregion
    
}
