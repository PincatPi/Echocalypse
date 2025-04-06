using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class GMCmd
{
    [MenuItem("GMCmd/读取背包静态数据")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>("SO/Package/PackageTable");
        if(packageTable == null)
            Debug.Log("packageTable is null");
        foreach (var item in packageTable.dataList)
        {
            Debug.Log($"[id]:{item.id}, [name]:{item.name}");
        }
    }

    [MenuItem("GMCmd/创建背包测试数据")]
    public static void CreateLocalPackageData()
    {
        //保存数据
        PackageLocalData.Instance.items = new List<PackageLocalItem>();
        for (int i = 0; i <= 4; ++i)
        {
            PackageLocalItem packageLocalItem = new PackageLocalItem()
            {
                uid = Guid.NewGuid().ToString(), //生成uid
                id = i,
                num = Random.Range(1, 10),
                level = Random.Range(1, 30),
                isNew = Random.Range(0, 2) == 0,
            };
            PackageLocalData.Instance.items.Add(packageLocalItem);
        }
        Debug.Log("创建数据");
        PackageLocalData.Instance.SavePackageData();
    }

    [MenuItem("GMCmd/创建大量背包测试数据")]
    public static void CreatePlentyPackageData()
    {
        //保存数据
        PackageLocalData.Instance.items = new List<PackageLocalItem>();
        //创建一万条背包测试动态数据
        for (int i = 0; i < 1000; ++i)
        {
            PackageLocalItem packageLocalItem = new PackageLocalItem()
            {
                uid = Guid.NewGuid().ToString(), //生成uid
                id = Random.Range(0, 5),
                num = Random.Range(1, 10),
                level = Random.Range(1, 30),
                isNew = Random.Range(0, 2) == 0,
            };
            PackageLocalData.Instance.items.Add(packageLocalItem);
        }
        Debug.Log("创建数据");
        PackageLocalData.Instance.SavePackageData();
    }

    [MenuItem("GMCmd/清空背包测试数据")]
    public static void ClearLocalPackageData()
    {
        PackageLocalData.Instance.items.Clear(); //清空动态数据
        PackageLocalData.Instance.SavePackageData(); //保存清空操作
    }

    [MenuItem("GMCmd/读取背包测试数据")]
    public static void ReadLocalPackageData()
    {
        //读取数据
        List<PackageLocalItem> readItems = PackageLocalData.Instance.LoadPackageData();
        if(readItems == null || readItems.Count == 0)
            Debug.Log("readItems为空");
        foreach (var item in readItems)
        {
            Debug.Log(item);
        }
        Debug.Log("读取背包测试数据");
    }

    [MenuItem("GMCmd/打开背包界面")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);
    }
}
