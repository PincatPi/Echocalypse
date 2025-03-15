using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachePoolObject : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1f;
    private float timer;
    void OnEnable()
    {
        Invoke(nameof(PushIntoCachePool), timeToDestroy);
        timer = timeToDestroy;
        StartCoroutine(IE_PushIntoCachePool());
    }

    IEnumerator IE_PushIntoCachePool()
    {
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }
        //将对象放入缓存池中
        PushIntoCachePool();
    }

    private void PushIntoCachePool()
    {
        CachePoolManager.Instance.PushObject(this.gameObject);
    }
}
