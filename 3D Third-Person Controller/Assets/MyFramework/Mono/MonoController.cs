using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono管理类
/// </summary>
public class MonoController : MonoBehaviour
{
    public event UnityAction updateEvent;
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Update()
    {
        //执行帧更新委托
        if (updateEvent != null)
        {
            updateEvent.Invoke();
        }
    }

    /// <summary>
    /// 添加帧更新事件的函数
    /// </summary>
    /// <param name="function"></param>
    public void AddUpdateListener(UnityAction function)
    {
        updateEvent += function;
    }

    /// <summary>
    /// 移除帧更新事件的函数
    /// </summary>
    /// <param name="function"></param>
    public void RemoveUpdateListener(UnityAction function)
    {
        updateEvent -= function;
    }
}
