using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    
    [SerializeField] private Text dialogueContent; //对话内容
    [SerializeField] private Text speakerName; //对话者名字
    [SerializeField] private Image speakerAvatar; //对话者头像

    private void Awake()
    {
        // dialogueContent = GameObject.Find("DialoguePanel").transform.Find("DialogueContent/Content").GetComponent<Text>();
        // speakerName = GameObject.Find("DialoguePanel").transform.Find("DialogueContent/SpeakerName").GetComponent<Text>();
        // speakerAvatar = GameObject.Find("DialoguePanel").transform.Find("DialogueContent/SpeakerAvatar").GetComponent<Image>();
    }
    
    /// <summary>
    /// 开始对话，显示对话UI
    /// </summary>
    public void StartDialogue()
    {
        //TODO: 此处可以修改为使用UIManager统一管理
        dialoguePanel.SetActive(true);
    }

    /// <summary>
    /// 结束对话，关闭对话UI
    /// </summary>
    public void EndDialogue()
    {
        //TODO: 此处可以修改为使用UIManager统一管理
        dialoguePanel.SetActive(false);
    }
    
    public void UpdateDialogueInfo(string content, string name, Sprite avatar)
    {
        dialogueContent.text = content;
        if (name != string.Empty)
        {
            speakerName.text = name;
        }
        if (speakerAvatar)
        {
            speakerAvatar.sprite = avatar;   
        }
    }

    /// <summary>
    /// 对于多分支对话，生成分支选项
    /// </summary>
    public void GenerateOptions(List<string> optionsList, BranchDialogue branchDialogue)
    {
        for (int i = 1; i <= optionsList.Count; i++)
        {
            //根据选项的数量，进行偏移处理
            float offset = 400 / (optionsList.Count + 1);
            float rectPositionY = 200 - (i * offset);
            GenerateButton(i, rectPositionY, optionsList, branchDialogue);
        }
    }

    /// <summary>
    /// 生成多分支对话选项的按键
    /// </summary>
    public void GenerateButton(int index, float rectPositionY, List<string> optionsList, BranchDialogue branchDialogue)
    {
        //显示鼠标
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        //TODO: 可以修改逻辑，将按键Button做成预制体并在此处实例化
        
        DefaultControls.Resources uiResources = new DefaultControls.Resources(); //使用DefaultControls生成UIButton
        GameObject optionGameObject = DefaultControls.CreateButton(uiResources);
        optionGameObject.transform.SetParent(dialoguePanel.transform.Find("DialogueContent/OptionsList")); //设置父子关系
        optionGameObject.name = "Option" + index; //重命名
        
        //对Button进行位置偏移处理
        RectTransform rectTransform = optionGameObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = new Vector2(300, 100);
        rectTransform.anchoredPosition = new Vector2(0, rectPositionY);
        
        //在OptionButton上添加点击事件
        Button optionButton = optionGameObject.GetComponent<Button>();
        optionButton.onClick.AddListener(() =>
        {
            Debug.Log("添加事件");
            ReturnNextDialogue(index, branchDialogue);
        });
        
        //设置选项文本格式
        Text optionContent = optionGameObject.transform.Find("Text (Legacy)").GetComponent<Text>();
        optionContent.text = optionsList[index - 1];
        optionContent.fontStyle = FontStyle.Bold;
        optionContent.fontSize = 30;
    }

    /// <summary>
    /// 多分支对话选项的按键响应事件
    /// </summary>
    private void ReturnNextDialogue(int index, BranchDialogue branchDialogue)
    {
        branchDialogue.nextDialogueStarted = true;
        branchDialogue.nextDialogueIndex = index - 1;
        //关闭鼠标
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// 分支结点结束时调用的方法
    /// </summary>
    public void SelectedStop(BranchDialogue branchDialogue)
    {
        branchDialogue.nextDialogueStarted = false; //重置标记位
        //获取所有创建出来的Button并进行销毁
        Button[] optionButtons = dialoguePanel.transform.Find("DialogueContent/OptionsList").GetComponentsInChildren<Button>();
        foreach (Button optionButton in optionButtons)
        {
            Destroy(optionButton.gameObject);
        }
    }
}
