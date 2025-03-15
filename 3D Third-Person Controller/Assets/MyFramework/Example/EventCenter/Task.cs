using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("MonsterDead", TaskWaitMonsterDeadDo);
    }

    public void TaskWaitMonsterDeadDo(object info)
    {
        Debug.Log("任务完成");
    }

    void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("MonsterDead", TaskWaitMonsterDeadDo);
    }
}
