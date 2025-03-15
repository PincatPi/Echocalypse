using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTest2
{
    public MonoTest2()
    {
        //开启协程
        MonoManager.GetInstance().StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Wait For 1 Sec And Print");
    }
    
    public void Update()
    {
        Debug.Log("MonoTest");
    }
}

public class MonoTest : MonoBehaviour
{
    void Start()
    {
        MonoTest2 test = new MonoTest2();
        MonoManager.GetInstance().AddUpdateListener(test.Update);
    }
}
