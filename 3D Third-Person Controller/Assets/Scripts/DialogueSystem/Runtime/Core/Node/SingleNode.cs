using UnityEngine;

namespace DialogueSystem
{
    public abstract class SingleNode : Node
    {
        //SingleNode只有一个子结点
        [HideInInspector] public Node childNode;        
    }
}