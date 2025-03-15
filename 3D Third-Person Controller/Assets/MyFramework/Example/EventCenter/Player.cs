using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("MonsterDead", MonsterDeadDo);
    }

    public void MonsterDeadDo(object info)
    {
        Debug.Log("玩家得到奖励" + (info as Monster).name);
    }

    void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("MonsterDead", MonsterDeadDo);
    }
}
