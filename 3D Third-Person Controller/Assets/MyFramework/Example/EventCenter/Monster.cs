using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string name = "123123";

    void Start()
    {
        Invoke(nameof(Dead), 1f);
    }

    void Dead()
    {
        Debug.Log("怪物死亡");
        //触发事件
        EventCenter.GetInstance().EventTrigger("MonsterDead", this);
    }
}
