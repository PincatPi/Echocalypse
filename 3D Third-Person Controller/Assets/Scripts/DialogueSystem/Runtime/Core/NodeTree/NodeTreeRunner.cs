using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    /// <summary>
    /// NodeTreeRunner需要挂载到游戏对象上
    /// </summary>
    public class NodeTreeRunner : MonoBehaviour
    {
        public NodeTree nodeTree;
        [SerializeField] private NPCDialogueController npcDialogueController;
        [SerializeField] private bool canDialogue = false;
        
        void Update()
        {
            if (canDialogue)
            {
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    nodeTree?.OnTreeStart();
                }
                //执行每一帧的对话树逻辑
                if (nodeTree)
                {
                    nodeTree.Update();
                }
            }
        }

        /// <summary>
        /// 执行nodeTree的退出逻辑
        /// </summary>
        public void OnNodeTreeExit()
        {
            nodeTree?.OnTreeEnd();
        }
        
        #region 公共接口

        public void SetCanDialogue(bool value)
        {
            canDialogue = value;
        }

        public void SetNodeTree(NodeTree value)
        {
            nodeTree = value;
        }

        public void SetNPCDialogueController(NPCDialogueController value)
        {
            npcDialogueController = value;
        }

        public NPCDialogueController GetNPCDialogueController()
        {
            return npcDialogueController;
        }

        #endregion
    }
}