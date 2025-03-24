using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerViewDetection : MonoBehaviour
{
    private Animator animator;
    private ThirdPersonController thirdPersonController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    
    [FormerlySerializedAs("enemies")]
    [Header("玩家锁敌")]
    [SerializeField] private Collider[] enemyColliders; //玩家面前一定距离内的敌人数组
    //[SerializeField] private Transform targetTransform = null;
    [SerializeField] private bool isLockTarget = false; //锁定敌人目标
    
    [FormerlySerializedAs("viewDistance")]
    [FormerlySerializedAs("distance")]
    [Header("玩家视野检测")]
    [SerializeField] private float maxLockOnDistance = 30f; //能够发现敌人的最远视线距离
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 cubeCenter;
    [SerializeField] private Vector3 rotateEuler;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask ignoreLayer;

    private int lockOnHash = Animator.StringToHash("LockOn");
    private int xInputHash = Animator.StringToHash("XInput");
    private int yInputHash = Animator.StringToHash("YInput");
    private int xSpeedHash = Animator.StringToHash("XSpeed");
    private int ySpeedHash = Animator.StringToHash("YSpeed");
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    void LateUpdate()
    {
        FindEnemyInFront();
        SwitchAnimator();
        LockOnEnemy();

        //TEST: 测试代码
        if (nearestLockOnTarget)
        {
            Vector3 dir = nearestLockOnTarget.position - mainCamera.transform.position;
            dir.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngles = targetRotation.eulerAngles;
            eulerAngles.y = 0;
            mainCamera.transform.localEulerAngles = eulerAngles;
        }
    }
    
    [SerializeField] private Transform viewTransform;
    [SerializeField][Range(0, 180)] private float viewAngle = 50f;
    //TODO: 此处将来要改成EnemyCombatController
    [SerializeField] private List<EnemyLockOn> availableTargets = new List<EnemyLockOn>();
    [SerializeField] private Transform nearestLockOnTarget;
    private void FindEnemyInFront()
    {
        //若不处在锁定状态，则不进行查找
        if (!isLockTarget)
            return;
        
        availableTargets.Clear();
        //检测相机面前的盒形碰撞体内是否有Enemy
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        cubeCenter = new Vector3(offset.x * cameraForward.x, offset.y * cameraForward.y, offset.z * cameraForward.z)+ cameraPos;
        enemyColliders = Physics.OverlapBox(cubeCenter, size / 2, Quaternion.Euler(rotateEuler), enemyLayer);
        
        if (enemyColliders.Length > 0)
        {
            //找到所有enemies中距离玩家最近的
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                //TODO: 此处将来要改成EnemyCombatController
                EnemyLockOn enemy = enemyColliders[i].GetComponent<EnemyLockOn>();
                if (enemy)
                {
                    Vector3 lockTargetDirection = new Vector3();
                    lockTargetDirection = enemy.transform.position - viewTransform.position;
                    float distanceFromTarget = Vector3.Distance(viewTransform.position, enemy.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, mainCamera.transform.forward);

                    if (viewableAngle > -viewAngle && viewableAngle < viewAngle && distanceFromTarget <= maxLockOnDistance)
                    {
                        availableTargets.Add(enemy);
                    }
                }
            }

            //寻找距离玩家最近的目标
            float shortestDistance = float.MaxValue;
            for (int i = 0; i < availableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(viewTransform.position, availableTargets[i].transform.position);
                if (distanceFromTarget <= maxLockOnDistance)
                {
                    shortestDistance = distanceFromTarget;
                    //TODO: 改为下面这句
                    //nearestLockOnTarget = availableTargets[i].GetLockOnTransform();
                    nearestLockOnTarget = availableTargets[i].lockOnTransform;
                }
            }

            if (nearestLockOnTarget) //若存在可以被锁定的目标对象
            {
                //设置摄像机对象
                SetCameraTarget(nearestLockOnTarget);
            }
        }
    }

    [SerializeField] private float targetWeight;
    private void SetCameraTarget(Transform targetTransform)
    {
        animator.SetFloat(lockOnHash, 1f);
        //cinemachineTargetGroup每时刻最多应该只有2个对象（m_Targets[0]固定为玩家对象，m_Targets[1]为敌人对象）
        //若当前没有
        if (cinemachineTargetGroup.m_Targets.Length == 1)
        {
            cinemachineTargetGroup.AddMember(targetTransform, targetWeight, 1);
        }
        //若此时已经有了一个目标对象，则使用传入的targetTransform更换它
        else if(cinemachineTargetGroup.m_Targets.Length == 2)
        {
            CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target
            {
                target = targetTransform, weight = targetWeight, radius = 1f
            };
            cinemachineTargetGroup.m_Targets[1] = newTarget;
        }
        else
        {
            Debug.LogError(string.Format("CinemachineTargetGroup的对象数量不正确, 此时其中有{0}个对象", cinemachineTargetGroup.m_Targets.Length));
        }
        
        Vector3 dir = targetTransform.position - mainCamera.transform.position;
        dir.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        Vector3 eulerAngles = targetRotation.eulerAngles;
        eulerAngles.y = 0;
        mainCamera.transform.localEulerAngles = eulerAngles;
        
        cinemachineTargetGroup.DoUpdate(); 
    }
    
    /// <summary>
    /// 查找玩家面前一定距离内的敌人
    /// </summary>
    // private void FindEnemyInFront()
    // {
    //     //若不处在锁定状态，则不进行查找
    //     if (!isLockTarget)
    //         return;
    //     
    //     //检测相机面前的盒形碰撞体内是否有Enemy
    //     Vector3 cameraPos = mainCamera.transform.position;
    //     Vector3 cameraForward = mainCamera.transform.forward;
    //     cubeCenter = new Vector3(offset.x * cameraForward.x, offset.y * cameraForward.y, offset.z * cameraForward.z)+ cameraPos;
    //     enemies = Physics.OverlapBox(cubeCenter, size / 2, Quaternion.Euler(rotateEuler), enemyLayer);
    //     
    //     float minDistance = float.MaxValue;
    //     if (enemies.Length > 0)
    //     {
    //         //找到所有enemies中距离玩家最近的
    //         for (int i = 0; i < enemies.Length; i++)
    //         {
    //             float distance = Vector3.Distance(this.transform.position, enemies[i].transform.position);
    //             //若该敌人与玩家间的距离小于最小距离，且能够在摄像机中被看到
    //             if (distance < minDistance && IsVisableInCamera(mainCamera, enemies[i].transform))
    //             {
    //                 minDistance = distance;
    //                 targetTransform = enemies[i].transform;
    //             }
    //         }
    //         //若找到了这样的对象
    //         if (!Mathf.Approximately(minDistance, float.MaxValue) && targetTransform)
    //         {
    //             //将该对象添加到虚拟相机的targetGroup中
    //             //cinemachineTargetGroup每时刻最多应该只有2个对象（m_Targets[0]固定为玩家对象，m_Targets[1]为敌人对象）
    //             if (cinemachineTargetGroup.m_Targets.Length == 1)
    //             {
    //                 cinemachineTargetGroup.AddMember(targetTransform, 1, 1);
    //             }
    //             else if(cinemachineTargetGroup.m_Targets.Length == 2)
    //             {
    //                 CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target
    //                 {
    //                     target = targetTransform, weight = 1f, radius = 1f
    //                 };
    //                 cinemachineTargetGroup.m_Targets[1] = newTarget;
    //             }
    //             cinemachineTargetGroup.DoUpdate();   
    //         }
    //     }
    //     //如果检测区内没有敌人 || 没有敌人是可以被相机看见的
    //     if(enemies.Length == 0 || !targetTransform || Mathf.Approximately(minDistance, float.MaxValue))
    //     {
    //         targetTransform = null; //目标对象置为空
    //         if (cinemachineTargetGroup.m_Targets.Length > 1)
    //         {
    //             cinemachineTargetGroup.m_Targets[1] = new CinemachineTargetGroup.Target();   
    //         }
    //         cinemachineTargetGroup.DoUpdate(); //更新
    //     }
    // }
    
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
        if (Physics.Raycast(ray, out RaycastHit hit, maxLockOnDistance, ~(ignoreLayer)))
        {
            if (hit.collider.gameObject != target.gameObject)
            {
                Debug.Log("视线被" + hit.collider.gameObject.name + "阻挡");
            }
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
            animator.SetFloat(xInputHash, thirdPersonController.GetMoveInput().x);
            animator.SetFloat(yInputHash, thirdPersonController.GetMoveInput().y);
            switch (thirdPersonController.locomotionState)
            {
                case ThirdPersonController.LocomotionState.Idle:
                    //TEST: 测试代码
                    animator.SetFloat(xSpeedHash, 0, 0.1f, Time.deltaTime);
                    animator.SetFloat(ySpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case ThirdPersonController.LocomotionState.Walk:
                    animator.SetFloat(xSpeedHash, dir.x * thirdPersonController.GetWalkSpeed(), 0.1f, Time.deltaTime);
                    animator.SetFloat(ySpeedHash, dir.z * thirdPersonController.GetWalkSpeed(), 0.1f, Time.deltaTime);
                    break;
                case ThirdPersonController.LocomotionState.Run:
                    animator.SetFloat(xSpeedHash, dir.x * thirdPersonController.GetRunSpeed(), 0.1f, Time.deltaTime);
                    animator.SetFloat(ySpeedHash, dir.z * thirdPersonController.GetRunSpeed(), 0.1f, Time.deltaTime);
                    break;
            }
        }
    }
    
    
    //TEST: 测试代码
    [SerializeField] private Transform target;
    [SerializeField] private float lockRotationSpeed;
    [SerializeField] private float offsetAngle;
    [SerializeField] private float stopFaceDis;
    private Vector3 targetDirection;
    /// <summary>
    /// //TODO: 锁定状态下的攻击，令玩家对象始终面朝敌人对象
    /// </summary>
    private void LockOnEnemy()
    {
        //若不处在锁定状态 || 找不到可以锁定的目标
        //if(!isLockTarget)
        if (!isLockTarget || !nearestLockOnTarget)
        {
            //TEST: 测试注释掉
            //
            // //切换为NormalCamera
            animator.SetFloat(lockOnHash, 0f);
            ClearViewTarget(); //清空之前查找到的目标
            ClearCameraTarget(); //清空CameraGroup中除了玩家之外的对象
            isLockTarget = false; //退出锁定状态（针对找不到可以锁定的目标）
            return;
        }
        //设状态为LockOn，切换至LockOnCamera
        //TEST: 测试注释掉
        //animator.SetFloat(lockOnHash, 1f);
            
        //dir = new Vector3(thirdPersonController.GetPlayerMovement().x, 0f, thirdPersonController.GetPlayerMovement().z);
        Vector3 toTarget = nearestLockOnTarget.position - transform.position;
        toTarget.y = 0;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("EquipMotion") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Equip") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("KatanaAttack") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("GSAttack") ||
            ((animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll")) && Vector3.Distance(transform.position, nearestLockOnTarget.position) > stopFaceDis) ||
            animator.IsInTransition(0))
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

    private void ClearViewTarget()
    {
        //targetTransform = null;
        nearestLockOnTarget = null;
        availableTargets.Clear();
    }

    private void ClearCameraTarget()
    {
        CinemachineTargetGroup.Target[] newTargets = new CinemachineTargetGroup.Target[]{cinemachineTargetGroup.m_Targets[0]};
        cinemachineTargetGroup.m_Targets = newTargets;
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
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            Transform target = enemyColliders[i].transform;
            
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(target.position);
            Ray ray = mainCamera.ScreenPointToRay(screenPoint);
            Gizmos.DrawRay(ray.origin, ray.direction * maxLockOnDistance);   
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
