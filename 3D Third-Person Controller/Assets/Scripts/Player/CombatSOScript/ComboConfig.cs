using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ComboConfig", menuName = "ScriptableObjects/Combat/ComboConfig")]
public class ComboConfig : ScriptableObject
{
    [Header("基础数据")] 
    public string comboName;
    public float coolDownTime;
    [Header("交互数据")]
    public ComboInteractionConfig[] comboInteractionConfigs;
    [Header("特效数据")]
    public FXConfig[] fxConfigs;
    [Header("音效数据")]
    public ClipConfig[] clipConfigs;
    [Header("自身位移补偿数据")]
    public SelfMoveOffsetConfig[] selfMoveOffsetConfigsConfigs;
    [Header("目标位移补偿数据")]
    public TargetMoveOffsetConfig[] targetMoveOffsetConfigsConfigs;
}

[System.Serializable]
public class ComboInteractionConfig
{
    public float startTime;
    public float endTime;
    public string hitName;
    public string hitAirName;
    //武器类型
    public E_WeaponType weaponType;
    //攻击力度
    public E_AttackForce attackForce;
    public float damage;
}

[System.Serializable]
public class AttackDetectionConfig
{
    public float startTime;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

[System.Serializable]
public class FXConfig
{
    public float startTime;
    public GameObject FXPrefab;
    public string FXName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

[System.Serializable]
public class ClipConfig
{
    public float startTime;
    public AudioClip audioClip;
    public float volume;
    public float duration;
}

[System.Serializable]
public class AttackFeedbackConfig
{
    public float strength; //屏幕震动
    public float frequency;
    public float duration;
    //TODO: 顿帧
}

[System.Serializable]
public class SelfMoveOffsetConfig
{
    public float startTime;
    public AnimationCurve animationCurve;
    public E_MoveOffsetDirection moveOffsetDirection;//方向
    public float scale;
    public float duration;
}

[System.Serializable]
public class TargetMoveOffsetConfig
{
    public float startTime;
    public AnimationCurve animationCurve;
    public E_MoveOffsetDirection moveOffsetDirection;//方向
    public float scale;
    public float duration;
}
