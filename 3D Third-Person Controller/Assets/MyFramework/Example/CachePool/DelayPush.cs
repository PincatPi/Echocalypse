using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPush : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke(nameof(Push), 1f);
    }

    private void Push()
    {
        CachePoolManager.GetInstance().PushObject(this.gameObject);
    }
}
