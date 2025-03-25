using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PackagePanel : BasePanel
{
    private Transform UIMenu;
    private Transform UIMenuDriverDist;
    private Transform UIMenuEngine;
    private Transform UIMenuBangboo;
    private Transform UICloseBtn;
    private Transform UICenter;
    private Transform UIScrollView;
    private Transform UIDetailPanel;
    private Transform UILeftBtn;
    private Transform UIRightBtn;
    private Transform UITitle;

    public GameObject PackageUIItemPrefab;
    private string chooseItemUid;

    private void Awake()
    {
        InitUI();
        InitClick();
    }

    private void Start()
    {
        RefreshUI();
    }
    
    private void InitUI()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIMenu = transform.Find("LeftTop/Menu");
        UIMenuDriverDist = transform.Find("LeftTop/Menu/DriverDist");
        UIMenuEngine = transform.Find("LeftTop/Menu/Engine");
        UIMenuBangboo = transform.Find("LeftTop/Menu/Bangboo");
        UICloseBtn = transform.Find("RightTop/CloseButton");
        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/ScrollView");
        UIDetailPanel = transform.Find("Center/DetailPanel");
        UILeftBtn = transform.Find("Left/LeftButton");
        UIRightBtn = transform.Find("Right/RightButton");
        UITitle = transform.Find("TopCenter/Title");
    }

    private void InitClick()
    {
        UIMenuDriverDist.GetComponent<Button>().onClick.AddListener(OnClickDriverDist);
        UIMenuEngine.GetComponent<Button>().onClick.AddListener(OnClickEngine);
        UIMenuBangboo.GetComponent<Button>().onClick.AddListener(OnClickBangboo);
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickCloseBtn);
        UILeftBtn.GetComponent<Button>().onClick.AddListener(OnClickLeftBtn);
        UIRightBtn.GetComponent<Button>().onClick.AddListener(OnClickRightBtn);
    }

    private void RefreshUI()
    {
        RefreshScrollView();
    }

    private void RefreshScrollView()
    {
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            //清空滚动容器中的所有物品
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        //从排序好的背包物品列表中取出物品动态数据
        foreach (PackageLocalItem dynamicData in GameManager.Instance.GetSortPackageLocalData())
        {
            //重新实例化背包物品
            Transform PackageUIItem = Instantiate<Transform>(PackageUIItemPrefab.transform, scrollContent);
            PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();
            //根据动态数据初始化物品信息
            packageCell.Refresh(dynamicData, this);
        }
    }

    private void RefreshDetailPanel()
    {
        //根据Uid找到对应的物品动态数据
        PackageLocalItem dynamicData = GameManager.Instance.GetPackageLocalItemByUid(chooseItemUid);
        //刷新详情界面
        UIDetailPanel.GetComponent<PackageDetail>().Refresh(dynamicData, this);
    }

    #region 重载方法

    public override void OpenPanel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        base.OpenPanel();
    }

    public override void ClosePanel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        base.ClosePanel();
    }

    #endregion
    
    #region 按键响应事件

    private void OnClickDriverDist()
    {
        Debug.Log(">>>>> OnClickDriverDist <<<<<");
    }
    
    private void OnClickEngine()
    {
        Debug.Log(">>>>> OnClickEngine <<<<<");
    }
    
    private void OnClickBangboo()
    {
        Debug.Log(">>>>> OnClickBangboo <<<<<");
    }
    
    private void OnClickCloseBtn()
    {
        Debug.Log(">>>>> OnClickCloseBtn <<<<<");
        UIManager.Instance.ClosePanel(UIConst.PackagePanel);
    }
    
    private void OnClickLeftBtn()
    {
        Debug.Log(">>>>> OnClickLeftBtn <<<<<");
    }
    
    private void OnClickRightBtn()
    {
        Debug.Log(">>>>> OnClickRightBtn <<<<<");
    }

    #endregion

    #region 公共接口

    public string GetChooseItemUid() => chooseItemUid;

    public void SetChooseItemUid(string newChooseItemUid)
    {
        chooseItemUid = newChooseItemUid; 
        RefreshDetailPanel(); //刷新详情界面
    }

    #endregion
}
