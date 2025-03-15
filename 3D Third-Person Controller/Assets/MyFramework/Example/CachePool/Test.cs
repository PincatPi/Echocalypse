using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CachePoolManager.GetInstance().GetObject("Test/Cube");
        }

        if (Input.GetMouseButtonDown(1))
        {
            CachePoolManager.GetInstance().GetObject("Test/Sphere");
        }
    }
}
