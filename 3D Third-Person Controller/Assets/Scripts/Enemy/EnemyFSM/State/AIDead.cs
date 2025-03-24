using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "AIDead", menuName = "StateMachine/State/AIDead")]
public class AIDead : StateActionSO
{
    //TODO: 将来应该重构UI逻辑
    private BossHealthAndEndurance bossHealthAndEndurance;
    
    private CharacterController characterController;
    private EnemyView enemyView;
    private MonoBehaviour[] components;

    [SerializeField] private float destoryTime = 5f;
    [SerializeField] private float downVelocity = 5f;
    
    
    protected override void Init(StateMachineSystem stateMachineSystem)
    {
        base.Init(stateMachineSystem);
        components = stateMachineSystem.GetComponents<MonoBehaviour>();
        characterController = stateMachineSystem.GetComponentInChildren<CharacterController>();
        
        //TODO: 将来应该重构UI逻辑
        bossHealthAndEndurance = stateMachineSystem.GetComponent<BossHealthAndEndurance>();
    }

    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        base.OnEnter(stateMachineSystem);
        DisableAllScripts();
        //延迟destoryTime时间后销毁该对象
        DelayDestoryThisGameObject();
        
        //TODO: 将来应该重构UI逻辑
        bossHealthAndEndurance.AppearText();
        bossHealthAndEndurance.DisappearBar();
    }

    public override void OnUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            Vector3 currentPosition = transform.position;
            // 计算新的 y 坐标
            float newY = currentPosition.y - (Time.deltaTime * downVelocity);
            // 更新角色的位置
            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);   
        }
        
        Debug.Log("此时处于死亡状态");
    }

    private void DisableAllScripts()
    {
        foreach (var component in components)
        {
            component.enabled = false;
            if (component.GetType().Name == nameof(StateMachineSystem) ||
                component.GetType().Name == nameof(BossHealthAndEndurance))
            {
                component.enabled = true;
            }
        }
        characterController.enabled = false;
    }

    private void DelayDestoryThisGameObject()
    {
        Timer timer = CachePoolManager.Instance.GetObject("Tool/Timer").GetComponent<Timer>();
        timer.CreateTime(destoryTime, () =>
        {
            //倒计时结束，销毁该对象
            Destroy(transform.gameObject);
        });
    }
}
