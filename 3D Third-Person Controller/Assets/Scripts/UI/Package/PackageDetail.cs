using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackageDetail : MonoBehaviour
{
    private Transform UIStars;
    private Transform UIDescription;
    private Transform UISkillDescription;
    private Transform UIIcon;
    private Transform UITitle;
    private Transform UILevelText;

    private PackageLocalItem packageDynamicData;
    private PackageTableItem packageStaticData;
    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
        Test();
    }

    //TEST: 测试方法
    private void Test()
    {
        Refresh(GameManager.Instance.GetSortPackageLocalData()[4], null);
    }

    private void InitUIName()
    {
        UIStars = transform.Find("Center/Stars");
        UIDescription = transform.Find("Center/Description");
        UISkillDescription = transform.Find("Bottom/Description");
        UIIcon = transform.Find("Center/Icon");
        UITitle = transform.Find("Top/Name");
        UILevelText = transform.Find("Bottom/LevelBackground/LevelText");
    }

    public void Refresh(PackageLocalItem packageDynamicItem, PackagePanel uiParent)
    {
        //初始化动态数据、静态数据、父物体
        this.packageDynamicData = packageDynamicItem;
        this.packageStaticData = GameManager.Instance.GetPackageItemByID(packageDynamicItem.id);
        this.uiParent = uiParent;

        if(UILevelText == null) Debug.Log("UILevelText is null");
        //等级
        UILevelText.GetComponent<TMP_Text>().text = $"Lv.{this.packageDynamicData.level.ToString()}/50";
        //简短描述
        UIDescription.GetComponent<Text>().text = this.packageStaticData.description;
        //详细描述
        UISkillDescription.GetComponent<Text>().text = this.packageStaticData.skillDescription;
        //物品名称
        UITitle.GetComponent<TMP_Text>().text = this.packageStaticData.name;
        //物品图片
        Texture2D texture = Resources.Load<Texture2D>(this.packageStaticData.imagePath);
        Sprite icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        UIIcon.GetComponent<Image>().sprite = icon;
        //物品星级
        RefreshStars();
    }
    
    /// <summary>
    /// 刷新星级
    /// </summary>
    public void RefreshStars()
    {
        for (int i = 0; i < UIStars.childCount; i++)
        {
            Transform star = UIStars.GetChild(i);
            if (this.packageStaticData.star > i)
            {
                star.gameObject.SetActive(true);
            }
            else
            {
                star.gameObject.SetActive(false);
            }
        }
    }
}
