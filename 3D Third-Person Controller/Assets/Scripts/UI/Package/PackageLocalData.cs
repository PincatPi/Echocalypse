using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLocalData : SingletonPatternBase<PackageLocalData>
{
    public List<PackageLocalItem> items;

    public void SavePackageData()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }

    public List<PackageLocalItem> LoadPackageData()
    {
        if (items != null)
        {
            return items;
        }

        if (PlayerPrefs.HasKey("PackageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");
            PackageLocalData packageLocalData = JsonUtility.FromJson<PackageLocalData>(inventoryJson);
            items = packageLocalData.items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            Debug.Log("本地没有找到PackageLocalData的缓存文件");
        }
        return items;
    }
}

[Serializable]
//背包物品动态数据
public class PackageLocalItem
{
    public string uid;
    public int id;
    public int num;
    public int level;
    public bool isNew;
    public override string ToString()
    {
        return $"[id]:{id}  [num]:{num}";
    }
}