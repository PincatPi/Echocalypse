using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class NPCDialogueController : MonoBehaviour
{
    private NodeTreeRunner nodeTreeRunner;
    //[SerializeField] private List<DialogueTree> dialogueTrees = new List<DialogueTree>();
    [SerializeField] private DialogueTree dialogueTree;
    private int currentDialogueIndex = 0;
    
    void Start()
    {
        nodeTreeRunner = GameObject.Find("NodeTreeRunner").GetComponent<NodeTreeRunner>();
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetDialogue();   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CancelDialogue();   
        }
    }

    private void SetDialogue()
    {
        //若当前对话树已经结束，则递增currentDialogueIndex索引
        // while (currentDialogueIndex < dialogueTrees.Count && dialogueTrees[currentDialogueIndex].isEnd == true)
        // {
        //     currentDialogueIndex++;
        // }
        // if (currentDialogueIndex < dialogueTrees.Count)
        // {
        //     nodeTreeRunner.SetNodeTree(dialogueTrees[currentDialogueIndex]); //设置对应的对话树   
        //     nodeTreeRunner.SetCanDialogue(true); //允许进行对话
        // }
        nodeTreeRunner.SetNodeTree(dialogueTree); //设置对应的对话树
        nodeTreeRunner.SetNPCDialogueController(this); //设置当前对话NPC
        nodeTreeRunner.SetCanDialogue(true); //允许进行对话
    }

    private void CancelDialogue()
    {
        nodeTreeRunner.OnNodeTreeExit(); //退出nodeTreeRunner当前正在运行的结点树
        nodeTreeRunner.SetCanDialogue(false); //不允许进行对话
    }

    #region 公共接口

    public void SetDialogueTree(DialogueTree dialogueTree)
    {
        this.dialogueTree = dialogueTree;
    }

    #endregion
}
