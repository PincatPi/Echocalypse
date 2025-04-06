using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "NodeTree", menuName = "DialogueSystem/NodeTree")]
    public class NodeTree : ScriptableObject
    {
        public Node rootNode; //对话树根结点
        private Node runningNode; //运行结点
        public E_NodeState treeState = E_NodeState.Waiting; //对话树当前状态
        public List<Node> nodes = new List<Node>(); //所有对话内容的存储列表

        public virtual void Update()
        {
            if (treeState == E_NodeState.Running && runningNode.state == E_NodeState.Running)
            {
                //执行运行节点的运行逻辑
                runningNode = runningNode.OnUpdate();
            }
        }
        //对话树开始的触发方法
        public virtual void OnTreeStart()
        {
            runningNode = rootNode;
            treeState = E_NodeState.Running;
            runningNode.state = E_NodeState.Running;
        }
        //对话树结束的触发方法
        public virtual void OnTreeEnd()
        {
            treeState = E_NodeState.Waiting;
            runningNode.state = E_NodeState.Waiting;
            runningNode.started = false;
        }
        
    #if UNITY_EDITOR
        /// <summary>
        /// 创建新结点
        /// </summary>
        public Node CreateNode(System.Type type)
        {
            Undo.RecordObject(this, "NodeTree(CreateNode)"); //记录新增结点前的结点树状态
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name; //为结点命名
            node.guid = GUID.Generate().ToString(); //生成该结点的guid
            nodes.Add(node); //将该结点添加到这棵树的结点列表
            //若不处于游戏运行模式，才保存结点的创建，否则不保存
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "NodeTree(CreateNode)");
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// 删除结点
        /// </summary>
        public Node DeleteNode(Node node)
        {
            Undo.RecordObject(this, "NodeTree(DeleteNode)"); //记录新增结点前的结点树状态
            nodes.Remove(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets(); //保存更改
            return node;
        }

        /// <summary>
        /// 删除两个结点间的父子关系
        /// </summary>
        public void RemoveChild(Node parentNode, Node childNode)
        {
            Undo.RecordObject(parentNode, "NodeTree(RemoveChild)"); //记录新增结点前的结点树状态
            parentNode.children.Remove(childNode); //将childNode从parentNode的children列表中移除
            EditorUtility.SetDirty(parentNode); //标记为脏
        }
        
        /// <summary>
        /// 添加两个结点间的父子关系
        /// </summary>
        public void AddChild(Node parentNode, Node childNode)
        {
            Undo.RecordObject(parentNode, "NodeTree(AddChild)"); //记录新增结点前的结点树状态
            parentNode.children.Add(childNode);
            EditorUtility.SetDirty(parentNode); //标记为脏
        }

        /// <summary>
        /// 获取父结点的所有子结点
        /// </summary>
        public List<Node> GetChildren(Node parentNode)
        {
            return parentNode.children;
        }
    #endif
    }
}
