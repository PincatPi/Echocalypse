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
        public Node runningNode; //运行结点
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
        }
        
    #if UNITY_EDITOR
        /// <summary>
        /// 创建新结点
        /// </summary>
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name; //为结点命名
            node.guid = GUID.Generate().ToString(); //生成该结点的guid
            nodes.Add(node); //将该结点添加到这棵树的结点列表
            //若不处于游戏运行模式，才保存结点的创建，否则不保存
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// 删除结点
        /// </summary>
        public Node DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node); //将该结点从AssetDatabase中移除
            AssetDatabase.SaveAssets(); //保存更改
            return node;
        }
    #endif
    }
}
