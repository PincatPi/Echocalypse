using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FXManager : SingletonPatternBase<FXManager>
{
    public void PlayOneFX(FXConfig fxConfig, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject FX = CachePoolManager.Instance.GetObject(fxConfig.FXName);
        if (!FX)
        {
            Debug.LogError("FX Manager is NULL");
            return;
        }
        FX.transform.position = position;
        FX.transform.eulerAngles = rotation;
        FX.transform.localScale = scale;
        ParticleSystem particleSystem = FX.GetComponent<ParticleSystem>();
        particleSystem.Play(); //播放特效
    }

    public void PlayOneHitFX(string FXName, Vector3 position, Vector3 scale)
    {
        GameObject FX = CachePoolManager.Instance.GetObject(FXName);
        if (!FX)
        {
            Debug.LogError("FX Manager is NULL");
            return;
        }
        //设置特效位置
        FX.transform.position = position;
        FX.transform.localScale = scale;
        ParticleSystem particleSystem = FX.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }
    
    public void PlayOneFX(EnemyFXConfig fxConfig, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject FX = CachePoolManager.Instance.GetObject(fxConfig.FXName);
        if (!FX)
        {
            Debug.LogError("FX Manager is NULL");
            return;
        }
        FX.transform.position = position;
        FX.transform.eulerAngles = rotation;
        FX.transform.localScale = scale;
        ParticleSystem particleSystem = FX.GetComponent<ParticleSystem>();
        particleSystem.Play(); //播放特效
    }
}
