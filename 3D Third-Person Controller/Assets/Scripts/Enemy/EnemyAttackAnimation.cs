using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储敌人攻击时的动画事件函数
/// </summary>
public class EnemyAttackAnimation : MonoBehaviour
{
    public AICombat aiCombat;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyCombatController enemyCombatController;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private EnemySwapWeapon enemySwapWeapon;

    //每个技能的配置信息
    [SerializeField] private List<AbilityConfig> abilityConfigs;
    //当前正在执行的技能的配置信息
    [SerializeField] private AbilityConfig currentAbilityConfig;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyCombatController = GetComponent<EnemyCombatController>();
        enemyTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        enemySwapWeapon = GetComponent<EnemySwapWeapon>();
    }

    /// <summary>
    /// 在动画播放时调用的事件函数
    /// </summary>
    /// count表示是当前技能的第几个攻击
    public void OnAttackAnimationEnter(int count)
    {
        if(abilityConfigs == null)
            return;
        if (!(animator.GetCurrentAnimatorStateInfo(0).IsTag("Ability") ||
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")))
            return;
        
        int currentAbilityID = count / 10;
        int configIndex = count % 10;
        
        currentAbilityConfig = GetAbilityConfigByID(currentAbilityID);
        enemyCombatController.SetCurrentAbilityConfig(currentAbilityConfig); //同步当前攻击配置信息
        enemyCombatController.SetAttackConfigCount(configIndex); //同步当前攻击配置信息索引
        
        AttackDetectionEvent(configIndex);
        PlayClipEvent(configIndex);
        PlayFXEvent(configIndex);
    }

    private void AttackDetectionEvent(int count)
    {
        if(count >= currentAbilityConfig.detectionConfigs.Length)
            return;
        //攻击检测开启
        enemyCombatController.isAttacking = true;
        enemySwapWeapon.GetCurrentActiveWeapon().weaponDetection.enabled = true; //碰撞体启用
        //开始进行攻击检测关闭倒数,倒数结束后关闭攻击检测
        StartCoroutine(IE_AttackDetectionCount(currentAbilityConfig.detectionConfigs[count].detectionTime));
    }
    
    /// <summary>
    /// 倒计时结束后,结束攻击判定
    /// </summary>
    /// <param name="timer"></param>
    /// <returns></returns>
    IEnumerator IE_AttackDetectionCount(float timer)
    {
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }
        //结束攻击判定
        enemyCombatController.isAttacking = false;
        enemySwapWeapon.GetCurrentActiveWeapon().weaponDetection.enabled = false; //关闭碰撞体
    }

    private void PlayFXEvent(int count)
    {
        if(count >= currentAbilityConfig.fxConfigs.Length)
            return;
        StartCoroutine(IE_FXCount(currentAbilityConfig.fxConfigs[count]));
    }
    
    IEnumerator IE_FXCount(EnemyFXConfig fxConfig)
    {
        float timer = fxConfig.startTime;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        if (fxConfig.FXName != null)
        {
            //修改位置
            Vector3 fxPosition = enemyTransform.forward * fxConfig.position.z + 
                                 enemyTransform.up * fxConfig.position.y + 
                                 enemyTransform.right * fxConfig.position.x;
            //播放攻击特效
            FXManager.Instance.PlayOneFX(fxConfig, fxPosition + transform.position, fxConfig.rotation + transform.eulerAngles, fxConfig.scale);
        }
    }

    private void PlayClipEvent(int count)
    {
        if(count >= currentAbilityConfig.clipConfigs.Length)
            return;
        StartCoroutine(IE_ClipCount(currentAbilityConfig.clipConfigs[count]));
    }
    
    IEnumerator IE_ClipCount(EnemyClipConfig clipConfig)
    {
        float timer = clipConfig.startTime;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }
        //播放音效
        if (clipConfig.audioClip)
        {
            audioSource.PlayOneShot(clipConfig.audioClip, clipConfig.volume);
        }
    }
    
    private AbilityConfig GetAbilityConfigByID(int abilityID)
    {
        for (int i = 0; i < abilityConfigs.Count; i++)
        {
            if(abilityConfigs[i].abilityID == abilityID)
                return abilityConfigs[i];
        }
        return null;
    }
    
    #region 公共接口

    public void ChangeCurrentAbilityConfig(CombatAbilityBase currentAbility)
    {
        if(!currentAbility)
            return;
        if(abilityConfigs.Count == 0)
            return;
        //若当前配置信息已经是要当前技能的配置信息,则返回
        if(currentAbilityConfig != null && currentAbilityConfig.abilityID == currentAbility.GetAbilityID())
            return;
        Debug.Log("更新了技能配置信息");
        for (int i = 0; i < abilityConfigs.Count; i++)
        {
            if (abilityConfigs[i].abilityID == currentAbility.GetAbilityID())
            {
                currentAbilityConfig = abilityConfigs[i];
                return;
            }
        }
    }

    #endregion
}

/// <summary>
/// 存储一个技能中多段攻击的配置信息
/// </summary>
[Serializable]
public class AbilityConfig
{
    [Header("技能信息")]
    [SerializeField] public int abilityID;
    [SerializeField] public CombatAbilityBase ability;
    [Header("攻击检测")]
    [SerializeField] public EnemyAttackDetectionConfig[] detectionConfigs;
    [Header("音效数据")]
    [SerializeField] public EnemyClipConfig[] clipConfigs;
    [Header("特效数据")]
    [SerializeField] public EnemyFXConfig[] fxConfigs;
}

[Serializable]
public class EnemyAttackDetectionConfig
{
    //攻击检测持续时间
    public float detectionTime;
    //攻击伤害
    public int damage;
}

[Serializable]
public class EnemyFXConfig
{
    public float startTime;
    public GameObject FXPrefab;
    public string FXName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

[Serializable]
public class EnemyClipConfig
{
    public float startTime;
    public AudioClip audioClip;
    public float volume;
    public float duration;
}
