using System;
using DialogueSystem;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Node = DialogueSystem.Node;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Node node;
    public Port input;
    public Port output;
    public Action<NodeView> OnNodeSelected; //该结点被选中时的事件函数
    public NodeView(Node node) : base("Assets/Scripts/DialogueSystem/Editor/UI/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name; //GraphView的Title属性
        this.viewDataKey = node.guid; //GraphView的Key属性，设为node的guid
        //用node的position来设置
        style.left = node.position.x;
        style.top = node.position.y;
        CreateInputPorts();
        CreateOutputPorts();
        SetNodeClass();
    }

    /// <summary>
    /// 根据结点的不同类型，设置NodeView的不同UI样式
    /// </summary>
    private void SetNodeClass()
    {
        if (node is SingleNode)
        {
            AddToClassList("single");
        }
        else if (node is CompositeNode)
        {
            AddToClassList("branch");
        }
    }

    private void CreateInputPorts()
    {
        //生成端口，连接线方向为纵向，类型为复合输入端口，端口类型为bool
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
        if (node is SingleNode)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column; //端口和名字上下排布
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        //生成端口，连接线方向为纵向，类型为复合输出端口，端口类型为bool
        output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        if (node is SingleNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse; //端口和名字上下排布
            outputContainer.Add(output);
        }
    }

    /// <summary>
    /// NodeView被选中时的事件函数
    /// </summary>
    public override void OnSelected()
    {
        base.OnSelected();
        //将该NodeView传给结点视图容器的OnSelected方法
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    public override void SetPosition(Rect newPosition)
    {
        Undo.RecordObject(node, "Node(SetPosition)");
        base.SetPosition(newPosition);
        node.position.x = newPosition.x;
        node.position.y = newPosition.y;
        EditorUtility.SetDirty(node); //标记为脏
    }

    /// <summary>
    /// 设置NodeView运行时状态UI
    /// </summary>
    public void SetNodeState()
    {
        RemoveFromClassList("running");
        if (Application.isPlaying)
        {
            switch (node.state)
            {
                case E_NodeState.Running:
                    if (node.started)
                    {
                        AddToClassList("running");
                    }
                    break;
            }
        }
    }
}
