using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    private float timer;
    private Action action;
    private bool timeIsDone;

    private void Update()
    {
        OnUpdate();
        RecycleObject();
    }


   

    private void OnUpdate()
    {
        if (!this.gameObject.activeSelf) return;

        if (timer > 0 && !timeIsDone)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                action?.Invoke();
                timeIsDone = true; 
            }
        }
    }

    /// <summary>
    /// 创建计时器
    /// </summary>
    /// <param name="timer">计时时间</param>
    /// <param name="cllBackAction">回调函数</param>
    public void CreateTime(float timer, Action cllBackAction, bool timeIsDone = false) 
    {
        this.timer = timer;
        this.action = cllBackAction;
        this.timeIsDone = timeIsDone;
    }

    public void RecycleObject()
    {
        if (timeIsDone) 
        {
            action = null;
            //回收到对象池中
            CachePoolManager.Instance.PushObject(this.gameObject);
        }
    }
}

