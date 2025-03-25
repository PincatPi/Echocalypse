using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PackageTable", menuName = "UI/PackageTable")]
public class PackageTable : ScriptableObject
{
    public List<PackageTableItem> dataList = new List<PackageTableItem>();
}

[System.Serializable]
//背包物品静态数据
public class PackageTableItem
{
    public int id; //id
    public E_ItemType type; //类型
    public int star; //星级
    public string name; //名字
    public string description; //描述
    public string skillDescription; //技能描述
    public string imagePath; //图标资源路径
}

public enum E_ItemType
{
    DriverDisk = 0,
    Engine = 1,
    Bangboo = 2
}