using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
[DisallowMultipleComponent]
public class InitLoopScrollView : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject item;
    public int totalCount = -1;
    
    //对象池
    Stack<Transform> pool = new Stack<Transform>();
    
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetObject(int index)
    {
        if (pool.Count == 0)
        {
            return Instantiate(item);
        }
        Transform candidate = pool.Pop();
        candidate.gameObject.SetActive(true);
        return candidate.gameObject;
    }

    /// <summary>
    /// 归还对象
    /// </summary>
    /// <param name="trans"></param>
    public void ReturnObject(Transform trans)
    {
        trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        pool.Push(trans);
    }

    public void ProvideData(Transform transform, int idx)
    {
        List<PackageLocalItem> items = GameManager.Instance.GetSortPackageLocalData();
        PackagePanel uiParent = (PackagePanel)UIManager.Instance.GetPanel(UIConst.PackagePanel);
        transform.GetComponent<PackageCell>().Refresh(items[idx], uiParent);
    }

    void Start()
    {
        var loopScrollRect = GetComponent<LoopScrollRect>();
        loopScrollRect.prefabSource = this;
        loopScrollRect.dataSource = this;
        loopScrollRect.totalCount = GameManager.Instance.GetSortPackageLocalData().Count;
        loopScrollRect.RefillCells();
    }
}
