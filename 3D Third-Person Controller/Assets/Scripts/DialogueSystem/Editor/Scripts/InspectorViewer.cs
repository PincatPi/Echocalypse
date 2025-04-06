using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorViewer : VisualElement
{
    Editor editor;
    
    public new class UxmlFactory : UxmlFactory<InspectorViewer, UxmlTraits> { }

    /// <summary>
    /// 更新选择的对象的Inspector窗口信息
    /// </summary>
    internal void UpdateSelection(NodeView nodeView)
    {
        //清除旧信息
        Clear();
        Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (editor.target != null)
            {
                editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
}
