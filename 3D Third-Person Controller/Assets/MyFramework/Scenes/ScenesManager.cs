using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// </summary>
public class ScenesManager : SingletonPatternBase<ScenesManager>
{
    private ScenesManager() { }
    
    /// <summary>
    /// 切换场景,同步加载
    /// </summary>
    /// <param name="sceneName">场景名</param>
    public void LoadScene(string sceneName, UnityAction function)
    {
        //场景同步加载
        SceneManager.LoadScene(sceneName);
        function?.Invoke();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 切换场景,异步加载
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="function">加载后调用的函数</param>
    public void LoadSceneAsync(string sceneName, UnityAction function)
    {
        //通过MonoManager管理类开启协程
        MonoManager.GetInstance().StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        //加载后,调用函数
        function?.Invoke();
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        //加载场景
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        while (ao != null && !ao.isDone)
        {
            //事件中心向外分发进度情况
            EventCenter.GetInstance().EventTrigger("进度条更新", ao.progress);
            yield return ao.progress;
        }
    }
    
    public void LoadSceneAsync(string sceneName)
    {
        MonoManager.GetInstance().StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }
}
