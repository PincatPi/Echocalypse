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
        
        public override Node LogicUpdate()
        {
            //按下对应按键时，结束当前对话结点运行状态，若有子结点则返回子结点
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                state = E_NodeState.Waiting;
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
            Debug.Log(dialogueContent);
            // DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            // dialogueManager.UpdateDialogueInfo(dialogueContent, speakerName, speakerAvatar);
        }

        //退出该结点时，打印日志
        protected override void OnStop()
        {
            Debug.Log("退出该结点");
            // if (!childNode)
            // {
            //     DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            //     dialogueManager.EndDialogue();
            // }
        }
    }
}