using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承了MonoBehaviour的单例模式基类
/// </summary>
//约束：继承该基类的类必须继承MonoBehaviour
public class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
{
    //volatile关键字：保证多线程下的可见性
    private static volatile T _instance;
    private static object _lock = new object(); //锁对象
    
    public static T Instance
    {
        get
        {
            //双重检查锁DCL保证创建单例的线程安全
            if (!_instance)
            {
                lock (_lock)
                {
                    if (!_instance)
                    {
                        GameObject obj = new GameObject();
                        //动态挂载对应的单例模式脚本（懒汉式）
                        _instance = obj.AddComponent<T>();
                        //为依附的GameObject改名
                        obj.name = typeof(T).ToString();
                        //过场景时不移除对象，保证它在整个游戏生命周期中都存在
                        DontDestroyOnLoad(obj);
                    }
                }
            }
            return _instance;
        }
    }

    public static T GetInstance()
    {
        //双重检查锁确保线程安全
        if (!_instance)
        {
            lock (_lock)
            {
                if (!_instance)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();
                    DontDestroyOnLoad(obj);
                }
            }
        }
        return _instance;
    }

    //保护类型虚函数:子类中可以对Awake进行重写
    protected virtual void Awake()
    {
        //对_instance进行赋值
        _instance = this as T;
    }
}
