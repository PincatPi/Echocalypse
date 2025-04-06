using DialogueSystem;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeEditor : EditorWindow
{
    public NodeTreeViewer nodeTreeViewer;
    public InspectorViewer inspectorViewer;

    [MenuItem("MyTools/DialogueEditor")]
    public static void ShowExample()
    {
        NodeEditor wnd = GetWindow<NodeEditor>();
        wnd.titleContent = new GUIContent("NodeEditor");
    }

    [OnOpenAsset]
    //打开NodeTree资产的方法
    public static bool OnOpenAsset(int instanceID, int line)
    {
        //若打开的资产是NodeTree
        if (Selection.activeObject is NodeTree)
        {
            ShowExample(); //则渲染该NodeEditor界面
            return true;
        }
        return false;
    }
    
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        
        //导入UXML
        var visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/DialogueSystem/Editor/UI/NodeEditor.uxml");
        visualTree.CloneTree(root);
        
        //导入USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/DialogueSystem/Editor/UI/NodeEditor.uss");
        root.styleSheets.Add(styleSheet);
        //nodeTreeViewer添加到NodeEditor视图中
        nodeTreeViewer = root.Q<NodeTreeViewer>();
        //inspectorViewer添加到NodeEditor视图中
        inspectorViewer = root.Q<InspectorViewer>();
        nodeTreeViewer.OnNodeSelected = OnNodeSelectionChanged;
    }

    /// <summary>
    /// 在选中时更新结点inspector面板
    /// </summary>
    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        inspectorViewer.UpdateSelection(nodeView);
    }
    
    /// <summary>
    /// 打开NodeEditor时，若当前选中对象是NodeTree，则赋值给NodeTreeViewer，在NodeTreeViewer中刷新视图
    /// </summary>
    private void OnSelectionChange()
    {
        NodeTree nodeTree = Selection.activeObject as NodeTree;
        //当选中的对象是NodeTree，才会进行NodeTreeViewer的视图渲染
        if (nodeTree)
        {
            nodeTreeViewer.PopulateView(nodeTree);   
        }
    }

    /// <summary>
    /// 更新NodeView状态UI
    /// </summary>
    private void OnInspectorUpdate()
    {
        nodeTreeViewer?.UpdateNodeStates();
    }
}
