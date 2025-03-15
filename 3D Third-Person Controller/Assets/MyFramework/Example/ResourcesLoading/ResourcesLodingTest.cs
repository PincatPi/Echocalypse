using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLodingTest : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ResourcesLodingManager.GetInstance().Load<GameObject>("Test/Cube");
            ResourcesLodingManager.GetInstance().LoadAsync<GameObject>("Test/Cube", (obj) =>
            {
                Debug.Log(obj.name);
                obj.transform.localScale = Vector3.one * 2f;
            });
        }
    }
}
