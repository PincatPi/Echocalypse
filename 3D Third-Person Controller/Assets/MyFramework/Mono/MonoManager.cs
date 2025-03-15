using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

public class MonoManager : SingletonPatternBase<MonoManager>
{
    private MonoController monoController;
    
    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        monoController = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// 外部调用:添加帧更新事件的函数
    /// </summary>
    /// <param name="function"></param>
    public void AddUpdateListener(UnityAction function)
    {
        monoController.AddUpdateListener(function);
    }
    
    /// <summary>
    /// 外部调用:移除帧更新事件的函数
    /// </summary>
    /// <param name="function"></param>
    public void RemoveUpdateListener(UnityAction function)
    {
        monoController.RemoveUpdateListener(function);
    }

    #region 外部调用:协程

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoController.StartCoroutine(routine);
    }

    /// <summary>
    /// 该方法不能够开启MonoController脚本中没有的函数的协程
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return monoController.StartCoroutine(methodName, value);
    }

    /// <summary>
    /// 该方法不能够开启MonoController脚本中没有的函数的协程
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return monoController.StartCoroutine(methodName);
    }

    /// <summary>
    /// 该方法不能够停止MonoController脚本中没有的函数的协程
    /// </summary>
    /// <param name="methodName"></param>
    public void StopCoroutine(string methodName)
    {
        monoController.StopCoroutine(methodName);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        monoController.StopCoroutine(routine);
    }
    
    public void StopCoroutine(Coroutine routine)
    {
        monoController.StopCoroutine(routine);
    }
    
    public void StopAllCoroutines()
    {
        monoController.StopAllCoroutines();
    }
    
    #endregion
}
