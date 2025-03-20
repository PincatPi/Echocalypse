using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    //引用
        protected Animator animator;
        protected CharacterController characterController;
        
        //MoveDirection(移动向量)
        protected Vector3 movementDirection;
        protected Vector3 verticalDirection;


        [SerializeField,Header("移动速度")] protected float characterGravity;
        [SerializeField] protected float characterCurrentMoveSpeed;
        protected float characterFallTime = 0.15f;
        protected float characterFallOutDeltaTime;
        protected float verticalSpeed; 
        protected float maxVerticalSpeed = 53f;

        
        [SerializeField,Header("地面检测")] protected float groundDetectionRang;
        [SerializeField] protected float groundDetectionOffset;
        [SerializeField] protected float slopRayExtent;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField,Tooltip("角色动画移动时检测障碍物的层级")] protected LayerMask whatIsObs;
        [SerializeField] protected bool isOnGround;
        
        
        //AnimationID
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        protected int horizontalHash = Animator.StringToHash("Horizontal");
        protected int verticalHash = Animator.StringToHash("Vertical");
        protected int moveSpeedHash = Animator.StringToHash("MoveSpeed");

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            characterController = GetComponent<CharacterController>();
        }
        
        protected virtual void Start()
        {
            characterFallOutDeltaTime = characterFallTime;
        }

        protected virtual void Update()
        {
            CharacterGravity();
            CheckOnGround();
        }

        #region 内部函数
        
        /// <summary>
        /// 角色重力
        /// </summary>
        private void CharacterGravity()
        {
            if (isOnGround)
            {
                
                characterFallOutDeltaTime = characterFallTime;

                if (verticalSpeed < 0.0f)
                {
                    verticalSpeed = -2f;
                }
            }
            else
            {
                if (characterFallOutDeltaTime >= 0.0f)
                {
                    characterFallOutDeltaTime -= Time.deltaTime;
                    characterFallOutDeltaTime = Mathf.Clamp(characterFallOutDeltaTime, 0, characterFallTime);
                }
            }

            if (verticalSpeed < maxVerticalSpeed)
            {
                verticalSpeed += characterGravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// 地面检测
        /// </summary>
        private void CheckOnGround()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundDetectionOffset, transform.position.z);
            isOnGround = Physics.CheckSphere(spherePosition, groundDetectionRang, whatIsGround, QueryTriggerInteraction.Ignore);
            
        }

        private void OnDrawGizmosSelected()
        {
            
            if (isOnGround) 
                Gizmos.color = Color.green;
            else 
                Gizmos.color = Color.red;

            Vector3 position = Vector3.zero;
            
            position.Set(transform.position.x, transform.position.y - groundDetectionOffset,
                transform.position.z);

            Gizmos.DrawWireSphere(position, groundDetectionRang);
            
        }

        /// <summary>
        /// 坡度检测
        /// </summary>
        /// <param name="dir">当前移动方向</param>
        /// <returns></returns>
        protected Vector3 ResetMoveDirectionOnSlop(Vector3 dir)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out var hit, slopRayExtent))
            {
                float newAnle = Vector3.Dot(Vector3.up, hit.normal);
                if (newAnle != 0 && verticalSpeed <= 0)
                {
                    return Vector3.ProjectOnPlane(dir, hit.normal);
                }
            }
            return dir;
        }
        
        protected bool CanAnimationMotion(Vector3 dir)
        {
            //TODO: 检查此处的animationMoveID有什么作用
            return Physics.Raycast(transform.position + transform.up * .5f, dir.normalized * animator.GetFloat(animationMoveID), out var hit, 1f,whatIsObs);
        }
        
        #endregion

        #region 公共函数
        
        /// <summary>
        /// 移动接口
        /// </summary>
        /// <param name="moveDirection">移动方向</param>
        /// <param name="moveSpeed">移动速度</param>
        public virtual void CharacterMoveInterface(Vector3 moveDirection, float moveSpeed,bool useGravity)
        {
            if (!CanAnimationMotion(moveDirection))
            {
                movementDirection = moveDirection.normalized;
        
                movementDirection = ResetMoveDirectionOnSlop(movementDirection);

                if (useGravity)
                {
                    verticalDirection.Set(0.0f, verticalSpeed, 0.0f);
                }
                else
                {
                    verticalDirection = Vector3.zero;
                }
                //角色移动
                characterController.Move((moveSpeed * Time.deltaTime)
                    * movementDirection.normalized + Time.deltaTime
                    * verticalDirection);
                //设置动画状态机
                animator.SetFloat(moveSpeedHash, moveSpeed);
            }
        }

        #endregion
}