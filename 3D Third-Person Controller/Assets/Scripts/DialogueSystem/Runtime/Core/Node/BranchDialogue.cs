using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class BranchDialogue : CompositeNode
    {
        [TextArea] public string dialogueContent; //对话内容
        public Sprite speakerAvatar;
        public string speakerName;

        public List<string> optionsList;
        public int nextDialogueIndex = 0; //下一个结点的索引
        public bool nextDialogueStarted = false; //是否开始进入下一个结点

        //判断进入哪个对话结点
        public override Node LogicUpdate()
        {
            if (nextDialogueStarted)
            {
                state = E_NodeState.Waiting;
                if (children.Count > nextDialogueIndex)
                {
                    children[nextDialogueIndex].state = E_NodeState.Running;
                    return children[nextDialogueIndex];
                }
            }
            return this;
        }

        protected override void OnStart()
        {
            DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            dialogueManager.GenerateOptions(optionsList, this); //生成分支选项
            dialogueManager.UpdateDialogueInfo(dialogueContent, speakerName, speakerAvatar);
        }

        protected override void OnStop()
        {
            DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            dialogueManager.SelectedStop(this);
            //若该对话分支上已经没有结点
            if (children.Count <= 0)
            {
                dialogueManager.EndDialogue(); //关闭对话UI
            }
        }
    }
}