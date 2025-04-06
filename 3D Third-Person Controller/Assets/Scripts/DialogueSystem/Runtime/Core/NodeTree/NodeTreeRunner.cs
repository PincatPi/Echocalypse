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
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Debug.Log("按下了enter键，启动对话树");
            nodeTree.OnTreeStart();
        }
        //执行每一帧的对话树逻辑
        if (nodeTree)
        {
            nodeTree.Update();
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Debug.Log("按下了esc键，退出对话树");
            // nodeTree.OnTreeEnd();
        }
    }
}

}