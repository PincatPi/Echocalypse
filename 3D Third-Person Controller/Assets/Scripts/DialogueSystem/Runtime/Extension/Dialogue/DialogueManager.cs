using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Text dialogueContent; //对话内容
    private Text speakerName; //对话者名字
    private Image speakerAvatar; //对话者头像

    private void Awake()
    {
        //TODO: 路径有待修改
        dialogueContent = GameObject.Find("UI").transform.Find("DialogueUI/DialogueContent/Content").GetComponent<Text>();
        speakerName = GameObject.Find("UI").transform.Find("DialogueUI/DialogueContent/speakerName").GetComponent<Text>();
        speakerAvatar = GameObject.Find("UI").transform.Find("DialogueUI/DialogueContent/speakerAvatar").GetComponent<Image>();
    }

    public void StartDialogue()
    {
        GameObject.Find("UI").transform.Find("DialogueUI").gameObject.SetActive(true);
    }

    public void EndDialogue()
    {
        GameObject.Find("UI").transform.Find("DialogueUI").gameObject.SetActive(false);
    }
    
    public void UpdateDialogueInfo(string Content, string name, Sprite avatar)
    {
        dialogueContent.text = Content;
        if (name != null)
        {
            speakerName.text = name;
        }
        if (speakerAvatar)
        {
            speakerAvatar.sprite = avatar;   
        }
    }
}
