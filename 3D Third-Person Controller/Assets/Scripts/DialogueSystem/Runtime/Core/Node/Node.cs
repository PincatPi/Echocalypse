using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DialogueSystem
{
    public enum E_NodeState
    {
        Running,
        Waiting
    };

    [CreateAssetMenu(fileName = "Node", menuName = "DialogueSystem/Node")]
    public abstract class Node : ScriptableObject
    {
        [SerializeField] protected NodeTree nodeTree; //该节点属于的结点树
        public E_NodeState state = E_NodeState.Waiting; //对话结点当前状态
        public bool started = false; //用于判断是否已经开始当前对话结点
        //public bool isEndNode = false; //标记该结点是否是该结点树的结束结点
        public List<Node> children = new List<Node>();
        [TextArea] public string description; //对话结点描述
        [HideInInspector] public string guid; //结点uid
        [HideInInspector] public Vector2 position; //当前结点在Editor中的位置

        public Node OnUpdate()
        {
            //若是第一次进入该结点，则调用OnStart方法
            if (!started)
            {
                OnStart();
                started = true;
            }
            Node currentNode = LogicUpdate();
            //若该结点不处于运行状态，则调用OnStop方法
            if (state != E_NodeState.Running)
            {
                OnStop();
                started = false;
            }
            return currentNode;
        }

        public abstract Node LogicUpdate();
        protected abstract void OnStart();
        protected abstract void OnStop();

        #region 公共方法

        public void SetNodeTree(NodeTree nodeTree)
        {
            this.nodeTree = nodeTree;
        }

        #endregion
    }
}