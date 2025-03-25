using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBase<GameManager>
{
    private PackageTable packageTable;
    
    void Start()
    {
        Invoke(nameof(OpenPackagePanel), 0.1f);
    }

    private void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);
    }

    /// <summary>
    /// 获取背包物品静态数据
    /// </summary>
    /// <returns></returns>
    public PackageTable GetPackageTable()
    {
        if (packageTable == null)
        {
            packageTable = Resources.Load<PackageTable>("SO/Package/PackageTable");
        }
        return packageTable;
    }

    /// <summary>
    /// 获取背包物品动态数据
    /// </summary>
    /// <returns></returns>
    public List<PackageLocalItem> GetPackageLocalData()
    {
        return PackageLocalData.Instance.LoadPackageData();
    }

    /// <summary>
    /// 根据ID，获取静态数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PackageTableItem GetPackageItemByID(int id)
    {
        List<PackageTableItem> packageStaticDataList = GetPackageTable().dataList;
        foreach (PackageTableItem staticItem in packageStaticDataList)
        {
            if (staticItem.id == id)
            {
                return staticItem;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据UID，获取动态数据
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public PackageLocalItem GetPackageLocalItemByUid(string uid)
    {
        List<PackageLocalItem> packageDynamicDataList = GetPackageLocalData();
        foreach (PackageLocalItem dynamicItem in packageDynamicDataList)
        {
            if (dynamicItem.uid == uid)
            {
                return dynamicItem;
            }
        }
        return null;
    }

    /// <summary>
    /// 对背包中的物品（根据动态数据）进行排序
    /// </summary>
    /// <returns></returns>
    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = PackageLocalData.Instance.LoadPackageData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }
}

/// <summary>
/// 对背包中物品的自定义排序
/// </summary>
public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem lhs, PackageLocalItem rhs)
    {
        //静态数据
        PackageTableItem lhsStatic = GameManager.Instance.GetPackageItemByID(lhs.id);
        PackageTableItem rhsStatic = GameManager.Instance.GetPackageItemByID(rhs.id);
        //先按星级从大到小排序
        int starComparison = rhsStatic.star.CompareTo(lhsStatic.star);
        //若星级相同，则按id从大到小排序
        if (starComparison == 0)
        {
            int idComparison = rhs.id.CompareTo(lhs.id);
            //若id相同，则按等级从大到小排序
            if (idComparison == 0)
            {
                return rhs.level.CompareTo(lhs.level);
            }
            return idComparison;
        }
        return starComparison;
    }
}
