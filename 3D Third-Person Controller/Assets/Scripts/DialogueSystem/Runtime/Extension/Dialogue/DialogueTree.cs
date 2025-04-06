using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTree", menuName = "DialogueSystem/DialogueTree")]
public class DialogueTree : NodeTree
{
    //对话树开始的触发方法
    public override void OnTreeStart()
    {
        base.OnTreeStart();
        GameObject.Find("DialogueManager").GetComponent<DialogueManager>().StartDialogue();
    }
    //对话树结束的触发方法
    public override void OnTreeEnd()
    {
        base.OnTreeEnd();
        GameObject.Find("DialogueManager").GetComponent<DialogueManager>().EndDialogue();
    }
}
