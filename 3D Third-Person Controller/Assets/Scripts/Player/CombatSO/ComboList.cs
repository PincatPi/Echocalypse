using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboList", menuName = "ScriptableObjects/Combat/ComboList")]
public class ComboList : ScriptableObject
{
    [SerializeField] private ComboConfig[] comboList;
    
    public int TryGetComboListCount() => comboList.Length;

    //攻击名字
    public string TryGetComboName(int comboIndex)
    {
        return comboIndex >= comboList.Length ? null : comboList[comboIndex].comboName;
    }

    //冷却时间
    public float TryGetCoolDownTime(int comboIndex)
    {
        return comboIndex >= comboList.Length ? 0 : comboList[comboIndex].coolDownTime;
    }

    //攻击交互
    public ComboInteractionConfig TryGetComboInteractionConfig(int comboIndex, int eventIndex)
    {
        if(comboIndex >= comboList.Length)
            return null;
        if(eventIndex >= comboList[comboIndex].comboInteractionConfigs.Length)
            return null;
        return comboList[comboIndex].comboInteractionConfigs[eventIndex];
    }
    
    //特效
    public FXConfig TryGetFXConfig(int comboIndex, int eventIndex)
    {
        if(comboIndex >= comboList.Length)
            return null;
        if(eventIndex >= comboList[comboIndex].fxConfigs.Length)
            return null;
        return comboList[comboIndex].fxConfigs[eventIndex];
    }
    
    //音效
    public ClipConfig TryGetClipConfig(int comboIndex, int eventIndex)
    {
        if(comboIndex >= comboList.Length)
            return null;
        if(eventIndex >= comboList[comboIndex].clipConfigs.Length)
            return null;
        return comboList[comboIndex].clipConfigs[eventIndex];
    }
    
    //自身位移补偿
    public SelfMoveOffsetConfig TryGetSelfMoveOffsetConfig(int comboIndex, int eventIndex)
    {
        if(comboIndex >= comboList.Length)
            return null;
        if(eventIndex >= comboList[comboIndex].selfMoveOffsetConfigsConfigs.Length)
            return null;
        return comboList[comboIndex].selfMoveOffsetConfigsConfigs[eventIndex];
    }
    
    //目标位移补偿
    public TargetMoveOffsetConfig TryGetTargetMoveOffsetConfig(int comboIndex, int eventIndex)
    {
        if(comboIndex >= comboList.Length)
            return null;
        if(eventIndex >= comboList[comboIndex].targetMoveOffsetConfigsConfigs.Length)
            return null;
        return comboList[comboIndex].targetMoveOffsetConfigsConfigs[eventIndex];
    }
}
