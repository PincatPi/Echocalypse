using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心,单例模式对象
/// </summary>
public class EventCenter : SingletonPatternBase<EventCenter>
{
    /// <summary>
    /// key-事件的名字; value-监听该事件的对应委托函数
    /// </summary>
    private Dictionary<string, UnityAction<object>> eventsDictionary = new Dictionary<string, UnityAction<object>>();
    
    private EventCenter() { }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">用于处理对应事件的委托函数</param>
    public void AddEventListener(string eventName, UnityAction<object> action)
    {
        //是否存在对用的事件监听
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName] += action;
        }
        else
        {
            eventsDictionary.Add(eventName, action);
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">需要被移除的委托函数</param>
    public void RemoveEventListener(string eventName, UnityAction<object> action)
    {
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName] -= action;
        }
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="eventName">触发的事件名字</param>
    /// <param name="info">触发事件时传入的参数信息</param>
    public void EventTrigger(string eventName, object info)
    {
        if (eventsDictionary.ContainsKey(eventName))
        {
            eventsDictionary[eventName].Invoke(info);
        }
    }

    /// <summary>
    /// 清空事件中心,主要用在场景切换时
    /// </summary>
    public void Clear()
    {
        eventsDictionary.Clear();
    }
}
