using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "NormalDialogue", menuName = "DialogueSystem/NormalDialogue")]
    public class NormalDialogue : SingleNode
    {
        [TextArea] public string dialogueContent; //对话内容
        public Sprite speakerAvatar;
        public string speakerName;
        public DialogueTree nextDialogueTree;
        
        public override Node LogicUpdate()
        {
            //按下对应按键时，结束当前对话结点运行状态，若有子结点则返回子结点
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                state = E_NodeState.Waiting;
                //更新childNode结点
                childNode = children.Count > 0 ? children[0] : null;
                //TODO: 此处要更新逻辑
                if (childNode)
                {
                    childNode.state = E_NodeState.Running;
                    return childNode;
                }
            }
            return this;
        }

        //首次进入该结点时，打印对话内容
        protected override void OnStart()
        {
            DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            dialogueManager.UpdateDialogueInfo(dialogueContent, speakerName, speakerAvatar);
        }

        //退出该结点时，打印日志
        protected override void OnStop()
        {
            //若该对话分支上已经没有结点
            if (!childNode)
            {
                //若该结点有下一个对话树，说明当前结点是该对话树的结束结点，且需要切换到下一个对话树
                if (nextDialogueTree)
                {
                    Debug.Log("切换下一个对话树");
                    ((DialogueTree)nodeTree).isEnd = true; //将该结点树标记为已经结束
                    NodeTreeRunner nodeTreeRunner = GameObject.Find("NodeTreeRunner").GetComponent<NodeTreeRunner>();
                    NPCDialogueController npcDialogueController = nodeTreeRunner.GetNPCDialogueController();
                    npcDialogueController.SetDialogueTree(nextDialogueTree); //将该结点的下一个对话树设为当前NPC的对话树
                }
                DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
                dialogueManager.EndDialogue(); //关闭对话UI
            }
        }
    }
}