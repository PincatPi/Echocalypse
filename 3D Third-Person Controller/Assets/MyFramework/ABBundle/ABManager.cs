using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

/// <summary>
/// AB包管理脚本
/// </summary>
public class ABManager : SingletonMonoBase<ABManager>
{
    //主包
    private AssetBundle mainAB = null;
    //获取依赖包所需要的主包配置文件
    private AssetBundleManifest manifest = null;

    /// <summary>
    /// AB包的存储路径成员属性，方便修改
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    
    /// <summary>
    /// 主包名，根据不同平台返回对应主包名
    /// IOS-"ios";ANDROID-"Android";PC-"PC"
    /// </summary>
    private string MainABName
    {
        get
        {
            #if UNITY_IOS
                return "iOS";
            #elif UNITY_ANDROID
                return "Android";
            #else
                return "PC";
            #endif
        }
    }
    
    //AB包不能够重复加载，否则会报错
    //使用字典来存储加载过的AB包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    
    /// <summary>
    /// 同步加载AB包及其依赖包
    /// </summary>
    /// <param name="abName"></param>
    public void LoadAB(string abName)
    {
        //加载主包
        if (!mainAB)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            //加载主包配置文件
            if (!manifest)
            {
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }
        //获取依赖包相关信息
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        //加载依赖包
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否已经被加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                //加载AB包
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                //存储到字典中
                abDic.Add(strs[i], ab);
            }
        }
        //加载目标包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            //存储到字典中
            abDic.Add(abName, ab);
        }
    }
    
    
    
    #region 同步加载AB包中资源
    
    /// <summary>
    /// 同步加载AB包中资源
    /// 若资源为GameObject类型，则实例化后再返回实例化对象
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <returns>需要加载的资源对象，为Object类型</returns>
    public Object LoadResource(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    
    /// <summary>
    /// 同步加载AB包中资源，通过type指定资源类型
    /// 若资源为GameObject类型，则实例化后再返回实例化对象
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <returns>需要加载的资源对象</returns>
    public Object LoadResource(string abName, string resName, System.Type type)
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }
    
    
    /// <summary>
    /// 同步加载AB包中资源，使用泛型指定资源类型
    /// 若资源为GameObject类型，则实例化后再返回实例化对象
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <typeparam name="T">资源类型</typeparam>
    /// <returns>需要加载的资源对象</returns>
    public T LoadResource<T>(string abName, string resName) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //加载资源
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }
    
    #endregion



    #region 异步加载AB包中资源

    //此处的异步加载只异步加载AB包中的资源，AB包本身还是同步加载的
    
    /// <summary>
    /// 根据资源名异步加载AB包中资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack">用于接收返回值的委托函数</param>
    public void LoadResourceAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //启动协程(要求该脚本需继承Mono)
        StartCoroutine(LoadAsyncCoroutine(abName, resName, callBack));
    }
    /// <summary>
    /// 用于异步加载AB包中资源的协程
    /// 若资源为GameObject类型则实例化后再返回实例化对象
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    private IEnumerator LoadAsyncCoroutine(string abName, string resName, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //加载AB包中的资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        //等待资源加载完毕后返回
        yield return abr;
        //异步加载结束后，通过委托传递给外部
        //若资源为GameObject类型则实例化后再返回
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else 
            callBack(abr.asset);
    }
     
    
    /// <summary>
    /// 根据资源名和类型异步加载AB包中资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callBack"></param>
    public void LoadResourceAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //启动协程
        StartCoroutine(LoadAsyncCoroutine(abName, resName, type, callBack));
    }
    /// <summary>
    /// 用于异步加载AB包中资源的协程
    /// 若资源为GameObject类型则实例化后再返回实例化对象
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    private IEnumerator LoadAsyncCoroutine(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //加载AB包中的资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        //等待资源加载完毕后返回
        yield return abr;
        //异步加载结束后，通过委托传递给外部
        //若资源为GameObject类型则实例化后再返回
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else 
            callBack(abr.asset);
    }
    
    
    /// <summary>
    /// 根据资源名和泛型异步加载AB包中资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadResourceAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //启动协程
        StartCoroutine(LoadAsyncCoroutine<T>(abName, resName, callBack));
    }
    /// <summary>
    /// 用于异步加载AB包中资源的协程
    /// 若资源为GameObject类型则实例化后再返回实例化对象
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private IEnumerator LoadAsyncCoroutine<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //加载AB包中的资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        //等待资源加载完毕后返回
        yield return abr;
        //异步加载结束后，通过委托传递给外部
        //若资源为GameObject类型则实例化后再返回
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset) as T);
        else 
            callBack(abr.asset as T);
    }
    
    #endregion
    
    
    /// <summary>
    /// 卸载单个AB包
    /// </summary>
    /// <param name="abName"></param>
    public void UnloadResource(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            //卸载该AB包
            abDic[abName].Unload(false);
            //将它从字典中移除
            abDic.Remove(abName);
        }
    }
    
    /// <summary>
    /// 卸载所有加载的AB包（包括主包）
    /// </summary>
    public void UnloadAllResources()
    {
        //卸载所有包
        AssetBundle.UnloadAllAssetBundles(false);
        //清空字典
        abDic.Clear();
        //主包和主包的配置文件设为null
        mainAB = null;
        manifest = null;
    }
}
