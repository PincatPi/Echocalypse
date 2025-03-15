using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenesTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScenesManager.GetInstance().LoadSceneAsync("Test", ChangeSceneFunc);
        }
    }

    private void ChangeSceneFunc()
    {
        Debug.Log("场景切换");
    }
}
