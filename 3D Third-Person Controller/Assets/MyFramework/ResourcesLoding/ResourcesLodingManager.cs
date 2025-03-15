using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源动态加载模块
/// </summary>
public class ResourcesLodingManager : SingletonPatternBase<ResourcesLodingManager>
{
    private ResourcesLodingManager() { }
    
    /// <summary>
    /// 同步加载资源,同时实例化GameObject类型对象
    /// </summary>
    /// <param name="path">资源路径名</param>
    public T Load<T>(string path) where T : Object
    {
        T result = Resources.Load<T>(path);
        //若对象是GameObject
        if (result is GameObject)
        {
            //实例化对象
            return GameObject.Instantiate(result);
        }
        else
        {
            return result;
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="path">资源路径名</param>
    /// /// <param name="callback">委托函数</param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        //使用MonoManager开启协程
        MonoManager.GetInstance().StartCoroutine(LoadAsyncCoroutine(path, callback));
    }

    /// <summary>
    /// 开启异步加载的协程
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private IEnumerator LoadAsyncCoroutine<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;

        if (r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }
}
