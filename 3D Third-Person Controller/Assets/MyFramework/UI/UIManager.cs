using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PanelConfig
{
    public GameObject panelPrefab;
    public string path;
};

public class UIManager : SingletonMonoBase<UIManager>
{ 
    public PanelConfig[] _panelConfig = Array.Empty<PanelConfig>();
    [SerializeField] private string rootPath;
    private Transform _root; //UI的挂载根结点
    private Dictionary<string, string> pathDict;
    private Dictionary<string, GameObject> prefabDict; //存储UI预制件
    public Dictionary<string, BasePanel> activePanelDict; //存储当前已经打开的界面
    public Dictionary<string, BasePanel> negativePanelDict; //存储当前关闭的界面

    private void Start()
    {
        InitDicts();
    }
    
    private UIManager() { }

    public Transform UIRoot
    {
        get
        {
            if (!_root)
                _root = new GameObject("RootUICanvas").transform;
            return _root;
        }
    }
    
    /// <summary>
    /// 打开UI界面
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public BasePanel OpenPanel(string panelName)
    {
        Debug.Log(panelName);
        BasePanel panel = null;
        // 查看已打开UI字典，检查该UI是否已打开
        if (activePanelDict.TryGetValue(panelName, out panel))
        {
            Debug.LogError(panelName + " is already open");
            return null;
        }
        //若关闭界面字典中存在，说明已经实例化过，则取出来并设为active即可
        if (negativePanelDict.TryGetValue(panelName, out panel))
        {
            negativePanelDict.Remove(panelName);
            activePanelDict.Add(panelName, panel);
            panel.gameObject.SetActive(true);
            return panel;
        }
        //检查路径是否存在UIConfig配置类中
        string panelPath = "";
        if (!pathDict.TryGetValue(panelName, out panelPath))
        {
            Debug.LogError(panelName + " is doesn't exist");
            return null;
        }
        //若预制件缓存字典中存在，则直接使用，否则进行加载
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(panelName, out panelPrefab))
        {
            string realPath = rootPath + panelPath;
            panelPrefab = Resources.Load<GameObject>(realPath);
            prefabDict.Add(panelName, panelPrefab);
        }
        //打开界面
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        activePanelDict.Add(panelName, panel);
        panel.gameObject.SetActive(true);
        return panel;
    }
    
    /// <summary>
    /// 关闭UI界面
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public bool ClosePanel(string panelName)
    {
        //检查该界面是否已打开
        BasePanel panel = null;
        if (!activePanelDict.TryGetValue(panelName, out panel))
        {
            Debug.LogError(panelName + " is already closed");
            return false;
        }
        activePanelDict.Remove(panelName);
        negativePanelDict.Add(panelName, panel);
        panel.gameObject.SetActive(false);
        return true;
    }
    
    
    
    /// <summary>
    /// 初始化字典
    /// </summary>
    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        activePanelDict = new Dictionary<string, BasePanel>();
        negativePanelDict = new Dictionary<string, BasePanel>();
        pathDict = new Dictionary<string, string>();
        foreach (var config in _panelConfig)
        {
            pathDict.Add(config.panelPrefab.name, config.path);
        }
    }
}
