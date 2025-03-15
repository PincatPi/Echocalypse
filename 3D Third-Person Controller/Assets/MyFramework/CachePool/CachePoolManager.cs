using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 缓存池中的每一列
/// </summary>
public class CachePoolData
{
    public GameObject dataRoot;
    public List<GameObject> cachePoolList;

    public CachePoolData(GameObject obj, GameObject cachePoolRoot)
    {
        dataRoot = new GameObject(obj.name);
        dataRoot.transform.parent = cachePoolRoot.transform;
        //压入列表中
        cachePoolList = new List<GameObject>() { };
        PushObject(obj);
    }

    /// <summary>
    /// 向缓存池中添加对象
    /// </summary>
    /// <param name="obj"></param>
    public void PushObject(GameObject obj)
    {
        //存入缓存池中
        cachePoolList.Add(obj);
        //失活
        obj.SetActive(false);
        //设置inspector窗口中的父对象
        obj.transform.parent = dataRoot.transform;
    }

    /// <summary>
    /// 从缓存池中获取对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        GameObject result = null;
        //取出第一个缓存对象
        result = cachePoolList[0];
        //将其从缓存池中移除
        cachePoolList.RemoveAt(0);
        //激活，令其显示
        result.SetActive(true);
        //断开父子关系
        result.transform.parent = null;
        
        return result;
    }
}


/// <summary>
/// 缓存池管理模块
/// </summary>
public class CachePoolManager : SingletonPatternBase<CachePoolManager>
{
    public Dictionary<string, CachePoolData> cachePoolDic = new Dictionary<string, CachePoolData>();

    private GameObject cachePoolRoot;

    private CachePoolManager() { }

    /// <summary>
    /// 从缓存池中获取对象
    /// </summary>
    /// <param name="key">缓存池名,为资源的文件路径名</param>
    /// <returns>被获取的对象</returns>
    public GameObject GetObject(string key)
    {
        GameObject result = null;
        //缓存池中存在该类型对象的缓存
        if (cachePoolDic.ContainsKey(key) && cachePoolDic[key].cachePoolList.Count > 0)
        {
            result = cachePoolDic[key].GetObject();
        }
        else
        {
            result = GameObject.Instantiate(Resources.Load<GameObject>(key));
            //把对象名改成缓存池名
            result.name = key;
        }
        
        return result;
    }

    /// <summary>
    /// 向缓存池中添加对象,显示指定Key
    /// </summary>
    /// <param name="key">缓存池名</param>
    /// <param name="obj">被添加的对象</param>
    public void PushObject(string key, GameObject obj)
    {
        if (cachePoolRoot == null)
        {
            cachePoolRoot = new GameObject("CachePool");
        }

        //缓存池中存在该类型对象的缓存
        if (cachePoolDic.ContainsKey(key))
        {
            cachePoolDic[key].PushObject(obj);
        }
        else
        {
            cachePoolDic.Add(key, new CachePoolData(obj, cachePoolRoot));
        }
    }

    /// <summary>
    /// 向缓存池中添加对象,忽略Key(此时Key为obj.name)
    /// </summary>
    /// <param name="obj">被添加的对象</param>
    public void PushObject(GameObject obj)
    {
        PushObject(obj.name, obj);
    }

    /// <summary>
    /// 清空缓存池,主要用于场景切换时
    /// </summary>
    public void ClearCachePool()
    {
        cachePoolDic.Clear();
        cachePoolRoot = null;
    }
}
