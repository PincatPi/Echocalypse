using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBundleTest : MonoBehaviour
{
    void Start()
    {
        ABManager.GetInstance().LoadResourceAsync<GameObject>("model", "Cube", (obj) =>
        {
            obj.transform.position = new Vector3(0, 1, 0);
            obj.name = "Cube";
        });
        
        ABManager.GetInstance().LoadResourceAsync("model", "Cube", typeof(GameObject), (obj) =>
        {
            (obj as GameObject).transform.position = new Vector3(0, -1, 0);
            obj.name = "Sphere";
        });
    }
}
