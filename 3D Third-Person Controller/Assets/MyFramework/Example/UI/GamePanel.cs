using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel
{
    public void OnOpen()
    {
        Debug.Log("GameOpen");
        OpenPanel();
    }

    public void OnClose()
    {
        Debug.Log("GameClose");
        ClosePanel();
    }
}
