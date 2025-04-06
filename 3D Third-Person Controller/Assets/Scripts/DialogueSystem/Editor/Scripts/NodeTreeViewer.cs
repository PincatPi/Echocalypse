using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class NodeTreeViewer : GraphView
{
    public NodeTree nodeTree;
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<NodeTreeViewer, UxmlTraits> { }

    public NodeTreeViewer()
    {
        Insert(0, new GridBackground()); //插入背景
        this.AddManipulator(new ContentDragger()); //缩放功能
        this.AddManipulator(new ContentDragger()); //拖拽功能
        this.AddManipulator(new SelectionDragger()); //选中物体拖拽功能
        this.AddManipulator(new RectangleSelector()); //框选功能
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/DialogueSystem/Editor/UI/NodeTreeViewer.uss");
        styleSheets.Add(styleSheet);
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    /// <summary>
    /// 在视图层中撤销重做功能
    /// </summary>
    private void OnUndoRedo()
    {
        //重新渲染，即可将已经删除/新增的视图元素撤销
        PopulateView(nodeTree);
        AssetDatabase.SaveAssets();
    }
    
    //创建新结点
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var types = TypeCache.GetTypesDerivedFrom<DialogueSystem.Node>();
        foreach (var type in types)
        {
            if (!type.IsAbstract)
            {
                evt.menu.AppendAction($"{type.Name}", (a) => CreateNode(type));   
            }
        }
    }

    /// <summary>
    /// 创建新结点
    /// </summary>
    private void CreateNode(Type type)
    {
        //在项目资源中创建node结点对象
        DialogueSystem.Node node = nodeTree.CreateNode(type);
        CreateNodeView(node);
    }
    
    /// <summary>
    /// 创建新的结点视图
    /// </summary>
    private void CreateNodeView(DialogueSystem.Node node)
    {
        //在视图中创建一个结点对应的UI
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        //在NodeTreeViewer视图中新增结点UI
        AddElement(nodeView);
    }

    /// <summary>
    /// 视图改变时调用的事件函数
    /// </summary>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //若视图中有元素被删除，则循环遍历，找出被删除的元素是否是NodeView
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                NodeView nodeView = element as NodeView;
                //若该结点是nodeView结点
                if (nodeView != null)
                {
                    //则将它从nodeTree中删除
                    nodeTree.DeleteNode(nodeView.node);
                }
                //删除结点间的连接线
                Edge edge = element as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView; //输出端的是父结点
                    NodeView childView = edge.input.node as NodeView; //输入端的是子结点
                    //删除父子结点的父子关系
                    nodeTree.RemoveChild(parentView.node, childView.node);
                }
            });
        }
        //若当前视图中有新的连接线被创建
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView; //输出端的是父结点
                NodeView childView = edge.input.node as NodeView; //输入端的是子结点
                //添加父子结点的父子关系
                nodeTree.AddChild(parentView.node, childView.node);
            });
        }
        return graphViewChange;
    }
    
    internal void PopulateView(NodeTree nodeTree)
    {
        this.nodeTree = nodeTree;
        //刷新视图前取消视图改变事件（否则会报错）
        graphViewChanged -= OnGraphViewChanged;
        //清除之前渲染的所有内容
        DeleteElements(graphElements);
        //注册视图改变事件
        graphViewChanged += OnGraphViewChanged;
        
        //找到NodeTree下所有结点，并对遍历的这些结点创建NodeView视图
        nodeTree.nodes.ForEach(node => CreateNodeView(node));
        //为每个结点创建连接线
        nodeTree.nodes.ForEach(node =>
        {
            //获取每个结点的子结点列表
            var children = nodeTree.GetChildren(node);
            //循环列表，并创建连接线
            children.ForEach(child =>
            {
                NodeView parentView = FindNodeView(node); //找到父结点的NodeView
                NodeView childView = FindNodeView(child); //找到子结点的NodeView
                Edge edge = parentView.output.ConnectTo(childView.input); //从父结点的输出端口连接到子结点的输入端口
                AddElement(edge); //添加连接线
            });
        });
    }

    /// <summary>
    /// 通过结点Node找到结点视图NodeView
    /// </summary>
    NodeView FindNodeView(DialogueSystem.Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }
    
    /// <summary>
    /// 定义输入输出端口的连接逻辑
    /// </summary>
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(
            endPort => endPort.direction != startPort.direction
            && endPort.node != startPort.node).ToList();
    }

    /// <summary>
    /// 更新NodeView状态UI
    /// </summary>
    public void UpdateNodeStates()
    {
        nodes.ForEach(node =>
        {
            NodeView nodeView = node as NodeView;
            nodeView.SetNodeState();
        });
    }
}
