using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("进度条更新", PrintProcess);
        Debug.Log("AddEventListener");
    }
    
    public void PrintProcess(object info)
    {
        Debug.Log(info.ToString());
    }
    
    void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("进度条更新", PrintProcess);
    }
}
