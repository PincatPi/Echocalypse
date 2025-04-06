using UnityEngine;

namespace DialogueSystem
{
    public class BranchDialogue : CompositeNode
    {
        [TextArea] public string dialogueContent;
        public int nextDialogueIndex = 0; //下一个对话结点的索引

        //判断进入哪个对话结点
        public override Node LogicUpdate()
        {
            return null;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }
    }
}