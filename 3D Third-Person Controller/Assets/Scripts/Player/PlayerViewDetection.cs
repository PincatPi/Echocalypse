using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerViewDetection : MonoBehaviour
{
    private Animator animator;
    private ThirdPersonController thirdPersonController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    
    [Header("玩家锁敌")]
    private Collider[] enemies; //玩家面前一定距离内的敌人数组
    [SerializeField] private Transform targetTransform = null;
    [SerializeField] private bool isLockTarget = false; //锁定敌人目标
    [SerializeField] private bool isLocked = false;
    
    [Header("玩家视野检测")]
    [SerializeField] private float distance = 30f; //能够发现敌人的最远视线距离
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 cubeCenter;
    [SerializeField] private Vector3 rotateEuler;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask playerSubLayer;

    private int lockOnHash;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        lockOnHash = Animator.StringToHash("LockOn");
    }

    void LateUpdate()
    {
        FindEnemyInFront();
        SwitchAnimator();
        LockOnEnemy();
    }
    
    /// <summary>
    /// 查找玩家面前一定距离内的敌人
    /// </summary>
    private void FindEnemyInFront()
    {
        //若不处在锁定状态，则不进行查找
        if (!isLockTarget)
            return;
        
        //检测相机面前的盒形碰撞体内是否有Enemy
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        cubeCenter = new Vector3(offset.x * cameraForward.x, offset.y * cameraForward.y, offset.z * cameraForward.z)+ cameraPos;
        enemies = Physics.OverlapBox(cubeCenter, size / 2, Quaternion.Euler(rotateEuler), enemyLayer);
        
        float minDistance = float.MaxValue;
        if (enemies.Length > 0)
        {
            //找到所有enemies中距离玩家最近的
            for (int i = 0; i < enemies.Length; i++)
            {
                float distance = Vector3.Distance(this.transform.position, enemies[i].transform.position);
                //若该敌人与玩家间的距离小于最小距离，且能够在摄像机中被看到
                if (distance < minDistance && IsVisableInCamera(mainCamera, enemies[i].transform))
                {
                    minDistance = distance;
                    targetTransform = enemies[i].transform;
                }
            }
            //若找到了这样的对象
            if (!Mathf.Approximately(minDistance, float.MaxValue) && targetTransform)
            {
                //将该对象添加到虚拟相机的targetGroup中
                //cinemachineTargetGroup每时刻最多应该只有2个对象（m_Targets[0]固定为玩家对象，m_Targets[1]为敌人对象）
                if (cinemachineTargetGroup.m_Targets.Length == 1)
                {
                    cinemachineTargetGroup.AddMember(targetTransform, 1, 1);
                }
                else if(cinemachineTargetGroup.m_Targets.Length == 2)
                {
                    CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target
                    {
                        target = targetTransform, weight = 1f, radius = 1f
                    };
                    cinemachineTargetGroup.m_Targets[1] = newTarget;
                }
                cinemachineTargetGroup.DoUpdate();   
            }
        }
        //如果检测区内没有敌人 || 没有敌人是可以被相机看见的
        if(enemies.Length == 0 || !targetTransform || Mathf.Approximately(minDistance, float.MaxValue))
        {
            targetTransform = null; //目标对象置为空
            if (cinemachineTargetGroup.m_Targets.Length > 1)
            {
                cinemachineTargetGroup.m_Targets[1] = new CinemachineTargetGroup.Target();   
            }
            cinemachineTargetGroup.DoUpdate(); //更新
        }
    }
    
    /// <summary>
    /// 判断物体是否在相机中可见
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool IsVisableInCamera(Camera camera, Transform target)
    {
        if (!camera || !target)
            return false;
        //将目标物体坐标转为屏幕坐标
        Vector3 screenPoint = camera.WorldToScreenPoint(target.position);
        //该物体坐标在屏幕外
        if(screenPoint.x < 0 || screenPoint.y < 0 || screenPoint.x > Screen.width || screenPoint.y > Screen.height)
            return false;
        //从摄像机向目标物体发射射线
        Ray ray = camera.ScreenPointToRay(screenPoint);
        //忽略检测Player和Player下子物体层
        if (Physics.Raycast(ray, out RaycastHit hit, distance, ~(playerLayer | playerSubLayer)))
        {
            return hit.collider.gameObject == target.gameObject;
        }
        return false;
    }


    private Vector3 dir;
    private void SwitchAnimator()
    {
        dir = new Vector3(thirdPersonController.GetPlayerMovement().x, 0, thirdPersonController.GetPlayerMovement().z);
        //Vector3 dir = new Vector3(targetDirection.x, 0, targetDirection.z);
        if (thirdPersonController.playerPosture == ThirdPersonController.PlayerPosture.Stand)
        {
            animator.SetFloat("XInput", thirdPersonController.GetMoveInput().x, 0.1f, Time.deltaTime);
            animator.SetFloat("YInput", thirdPersonController.GetMoveInput().y, 0.1f, Time.deltaTime);
            switch (thirdPersonController.locomotionState)
            {
                case ThirdPersonController.LocomotionState.Idle:
                    //TEST: 测试代码
                    animator.SetFloat("XSpeed", 0, 0.1f, Time.deltaTime);
                    animator.SetFloat("YSpeed", 0, 0.1f, Time.deltaTime);
                    break;
                case ThirdPersonController.LocomotionState.Walk:
                    animator.SetFloat("XSpeed", dir.x * thirdPersonController.GetWalkSpeed(), 0.1f, Time.deltaTime);
                    animator.SetFloat("YSpeed", dir.z * thirdPersonController.GetWalkSpeed(), 0.1f, Time.deltaTime);
                    break;
                case ThirdPersonController.LocomotionState.Run:
                    animator.SetFloat("XSpeed", dir.x * thirdPersonController.GetRunSpeed(), 0.1f, Time.deltaTime);
                    animator.SetFloat("YSpeed", dir.z * thirdPersonController.GetRunSpeed(), 0.1f, Time.deltaTime);
                    break;
            }
        }
    }
    
    
    //TEST: 测试代码
    [SerializeField] private Transform target;
    [SerializeField] private float lockRotationSpeed;
    [SerializeField] private float offsetAngle;
    private Vector3 targetDirection;
    /// <summary>
    /// //TODO: 锁定状态下的攻击，令玩家对象始终面朝敌人对象
    /// </summary>
    private void LockOnEnemy()
    {
        //若不处在锁定状态 || 找不到可以锁定的目标
        //TEST: 下面这条语句将来要取消注释
        //if (!isLockTarget || !targetTransform)
        if(!isLockTarget)
        {
            //切换为NormalCamera
            animator.SetFloat(lockOnHash, 0f);
            targetTransform = null; //锁定目标置空（针对不处在锁定状态）
            isLockTarget = false; //退出锁定状态（针对找不到可以锁定的目标）
            return;
        }

        if (isLockTarget)
        {
            //设状态为LockOn，切换至LockOnCamera
            animator.SetFloat(lockOnHash, 1f);
            Debug.Log("处于锁定目标状态");
            
            //dir = new Vector3(thirdPersonController.GetPlayerMovement().x, 0f, thirdPersonController.GetPlayerMovement().z);
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0;
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("EquipMotion") || 
                animator.GetCurrentAnimatorStateInfo(0).IsTag("KatanaAttack") ||
                animator.GetCurrentAnimatorStateInfo(0).IsTag("GreatSwordAttack") ||
                animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll"))
            {
                Quaternion baseRotation = Quaternion.LookRotation(toTarget);
                //创建左侧偏移（绕Y轴旋转offsetAngle度）
                Quaternion leftOffset = Quaternion.AngleAxis(offsetAngle, Vector3.up);
                //组合两个旋转（注意乘法顺序）
                Quaternion targetRotation = baseRotation * leftOffset;
                //旋转玩家root
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockRotationSpeed * Time.deltaTime);
            }
        }
    }

    #region Gizmos
    
    private void OnDrawGizmos()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        cubeCenter = new Vector3(offset.x * cameraForward.x, offset.y * cameraForward.y, offset.z * cameraForward.z)+ cameraPos;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(cubeCenter, size);
    }
    private void DrawRay()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Transform target = enemies[i].transform;
            
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(target.position);
            Ray ray = mainCamera.ScreenPointToRay(screenPoint);
            Gizmos.DrawRay(ray.origin, ray.direction * distance);   
        }
    }

    #endregion
    
    #region 玩家输入相关
    
    //获取锁定敌人输入
    public void GetLockTargetInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isLockTarget = !isLockTarget;
        }
    }
    
    #endregion
}
