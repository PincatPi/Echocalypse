using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class CombatControllerBase : MonoBehaviour
{
    #region 组件
    
    protected AttackCheckGizmos attackCheckSystem;
    protected AudioSource audioSource;
    protected Animator animator;
    
    #endregion
    
    [SerializeField] protected ComboList currentComboList;
    protected int currentComboIndex;
    protected int nextComboIndex;
    protected bool canExecuteCombo;
    public bool CanExecuteCombo => canExecuteCombo;
    [SerializeField] protected float multiplier = 1.2f;
    [SerializeField] protected bool canBeHit;
    [SerializeField] protected float hitCoolDown = 0.25f;
    [SerializeField] protected HitFXConfig[] hitFXList;
    public Transform hitTransform; //播放受击特效的位置
    [SerializeField] protected Vector3 hitFXScale;
    
    protected virtual void Start()
    {
        runningEventIndex = new RunningEventIndex();
        animator = GetComponent<Animator>();
        attackCheckSystem = GetComponent<AttackCheckGizmos>();
        audioSource = GetComponent<AudioSource>();
        canExecuteCombo = true;
        canBeHit = true;
    }

    protected virtual void Update()
    {
        RunEvent();
    }
    
    //播放攻击动画
    protected void ExecuteCombo()
    {
        if(!canExecuteCombo)
            return;
        runningEventIndex.Reset();//重置攻击的事件计数
        currentComboIndex = nextComboIndex;
        //播放攻击动画
        animator.CrossFadeInFixedTime(currentComboList.TryGetComboName(currentComboIndex), 0.1555f, 0, 0);
        //更新攻击计数
        UpdateComboIndex();
        canExecuteCombo = false; //后摇
        StartCoroutine(IE_ExecuteComboCoolDown(currentComboList.TryGetCoolDownTime(currentComboIndex)));
        if (stopComboCoroutine != null)
        {
            StopCoroutine(stopComboCoroutine);
        }
        stopComboCoroutine = StartCoroutine(IE_StopCombo(currentComboList.TryGetCoolDownTime(currentComboIndex)));
    }
    
    private Coroutine stopComboCoroutine;
    
    //协程计算后摇时间
    IEnumerator IE_StopCombo(float coolDownTime)
    {
        float time = coolDownTime * multiplier;
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        //重置连招
        nextComboIndex = 0;
    }

    IEnumerator IE_ExecuteComboCoolDown(float coolDownTime)
    {
        while (coolDownTime > 0)
        {
            yield return null;
            coolDownTime -= Time.deltaTime;
        }
        canExecuteCombo = true;
    }

    //更新攻击计数
    private void UpdateComboIndex()
    {
        nextComboIndex++;
        //重置攻击计数
        if (nextComboIndex >= currentComboList.TryGetComboListCount())
        {
            nextComboIndex = 0;
        }
    }

    private RunningEventIndex runningEventIndex;
    //事件检测
    private void RunEvent()
    {
        if(!currentComboList)
            return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(currentComboList.TryGetComboName(currentComboIndex)) || 
            animator.IsInTransition(0))
            return;

        //攻击检测
        ComboInteractionConfig comboInteractionConfig = currentComboList.TryGetComboInteractionConfig(currentComboIndex, runningEventIndex.attackDetectionIndex);
        if (comboInteractionConfig != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > comboInteractionConfig.startTime)
            {
                //获得攻击反馈配置信息
                AttackFeedbackConfig attackFeedbackConfig = currentComboList.TryGetAttackFeedbackConfig(currentComboIndex, runningEventIndex.attackFeedbackIndex);
                //执行攻击检测
                attackCheckSystem.StartAttacking(comboInteractionConfig, attackFeedbackConfig); //开始进行攻击检测
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > comboInteractionConfig.endTime)
            {
                attackCheckSystem.EndAttacking();
                //执行一次事件后
                runningEventIndex.attackDetectionIndex++;
                runningEventIndex.attackFeedbackIndex++;
            }
        }
        //生成特效
        FXConfig fxConfig = currentComboList.TryGetFXConfig(currentComboIndex, runningEventIndex.FXIndex);
        if (fxConfig != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > fxConfig.startTime)
            {
                //修改位置
                Vector3 fxPosition = transform.forward * fxConfig.position.z + transform.up * fxConfig.position.y + transform.right * fxConfig.position.x;
                FXManager.Instance.PlayOneFX(fxConfig, fxPosition + transform.position, 
                    fxConfig.rotation + transform.eulerAngles, fxConfig.scale);
                runningEventIndex.FXIndex++;
            }
        }
        //播放音效
        ClipConfig clipConfig = currentComboList.TryGetClipConfig(currentComboIndex, runningEventIndex.clipIndex);
        if (clipConfig != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > clipConfig.startTime)
            {
                //播放音效
                if (clipConfig.audioClip)
                {
                    audioSource.PlayOneShot(clipConfig.audioClip, clipConfig.volume);
                    runningEventIndex.clipIndex++;
                }
            }
        }
    }

    [SerializeField] private float rotationSpeed;
    //受击函数
    public virtual void OnHit(ComboInteractionConfig interactionConfig, AttackFeedbackConfig attackFeedbackConfig, Transform attacker)
    {
        //看向攻击者
        // 获取当前的旋转和目标旋转
        Quaternion fromRotation = transform.rotation;
        Quaternion toRotation = Quaternion.LookRotation(-attacker.position, Vector3.up);
        // 平滑过渡到目标旋转
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * rotationSpeed);
        
        //播放受击动画
        if(!canBeHit)
            return;
        Debug.Log("受击了!受到了来自" + interactionConfig.weaponType + "的" + interactionConfig.damage + Random.Range(-10, 10) + "点伤害!");
        canBeHit = false;
        
        StartCoroutine(IE_HitCoolDown(attackFeedbackConfig, hitCoolDown));
        //生成受击特效
        string hitFXName = hitFXList[(int)interactionConfig.attackForce].TryGetHitFXName();
        FXManager.Instance.PlayOneHitFX(hitFXName, hitTransform.position, hitFXScale);
        //生成音效
    }

    IEnumerator IE_HitCoolDown( AttackFeedbackConfig attackFeedbackConfig, float coolDownTime)
    {
        coolDownTime = coolDownTime + attackFeedbackConfig.stopFrameTime;
        Debug.Log(coolDownTime);
        while (coolDownTime > 0)
        {
            yield return null;
            coolDownTime -= Time.deltaTime;   
        }
        canBeHit = true;
    }
    
    protected void SetAnimatorSpeed(float speed) => animator.speed = speed;

    protected void ResetAnimatorSpeed() => animator.speed = 1f;
}

public class RunningEventIndex
{
    public int attackDetectionIndex = 0;
    public int FXIndex = 0;
    public int clipIndex = 0;
    public int attackFeedbackIndex = 0;

    public void Reset()
    {
        attackDetectionIndex = 0;
        FXIndex = 0;
        clipIndex = 0;
        attackFeedbackIndex = 0;
    }
}
