using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Transform UIIcon;
    private Transform UINew;
    private Transform UISelect;
    private Transform UIName;
    private Transform UIDeleteSelect;
    private Transform UIStars;
    
    private PackageLocalItem packageDynamicData;
    private PackageTableItem packageStaticData;
    private PackagePanel uiParent;
    
    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Top/Icon");
        UINew = transform.Find("Top/New");
        UISelect = transform.Find("Select");
        UIName = transform.Find("Bottom/Name");
        UIDeleteSelect = transform.Find("DeleteSelect");
        UIStars = transform.Find("Bottom/Stars");
        
        //UIDeleteSelect.gameObject.SetActive(false);
    }

    /// <summary>
    /// 刷新每个物品的状态
    /// </summary>
    /// <param name="packageDynamicData"></param>
    /// <param name="packagePanel"></param>
    public void Refresh(PackageLocalItem packageDynamicData, PackagePanel packagePanel)
    {
        //数据初始化
        this.packageDynamicData = packageDynamicData; //初始化动态数据
        //根据id到GameManager中找到静态数据并初始化静态数据
        this.packageStaticData = GameManager.Instance.GetPackageItemByID(packageDynamicData.id);
        this.uiParent = packagePanel;
        
        if(this.packageStaticData == null) Debug.Log("静态数据为空");
        //名字信息
        UIName.GetComponent<TMP_Text>().text = this.packageStaticData.name;
        //是否是新获得
        UINew.gameObject.SetActive(this.packageDynamicData.isNew);
        //物品图片
        //Sprite icon = Resources.Load<Sprite>(this.packageStaticData.imagePath);
        Texture2D texture = Resources.Load<Texture2D>(this.packageStaticData.imagePath);
        Sprite icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        UIIcon.GetComponent<Image>().sprite = icon;
        //刷新星级
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

    public void OnPointerClick(PointerEventData eventData)
    {
        //若当前点击的物品uid跟Panel当前物品uid相同，则说明是重复点击，无需执行任何逻辑
        if(this.uiParent.GetChooseItemUid() == this.packageDynamicData.uid)
            return;
        //根据点击设置新的uid，并刷新详情界面
        this.uiParent.SetChooseItemUid(this.packageDynamicData.uid);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter: " + eventData.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit: " + eventData.ToString());
    }
}
