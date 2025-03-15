using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 没有继承MonoBehaviour的单例模式基类
/// </summary>
public abstract class SingletonPatternBase<T> where T : class
{
    private static volatile T _instance;
    private static readonly object _lock = new object(); //锁对象

    /// <summary>
    /// 通过属性获取单例
    /// </summary>
    public static T Instance
    {
        get
        {
            //双重检查锁保证线程安全
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        //instance = new T();
                        //_instance = (T)System.Activator.CreateInstance(typeof(T));
                        Type type = typeof(T);
                        //获取T类型的私有无参构造函数
                        ConstructorInfo constructor = type.GetConstructor(
                            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                            null, Type.EmptyTypes, null);
                        //调用该私有无参构造函数
                        if (constructor != null)
                        {
                            _instance = constructor.Invoke(null) as T;
                        }
                        else
                        {
                            Debug.LogError("Constructor Not Found");
                        }
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 通过方法获取单例
    /// </summary>
    /// <returns>单例模式对象</returns>
    public static T GetInstance()
    {
        //双重检查锁保证线程安全
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    //instance = new T();
                    //_instance = (T)System.Activator.CreateInstance(typeof(T));
                    Type type = typeof(T);
                    ConstructorInfo constructor = type.GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null, Type.EmptyTypes, null);
                    if (constructor != null)
                    {
                        _instance = constructor.Invoke(null) as T;
                    }
                    else
                    {
                        //TODO: 此处可以优化错误提示方式
                        Debug.LogError("Constructor Not Found");
                    }
                }
            }
        }
        return _instance;
    }
}
