using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : BasePanel
{
    public void OnOpen()
    {
        Debug.Log("MainMenuOpen");
        OpenPanel();
    }

    public void OnClose()
    {
        Debug.Log("MainMenuClose");
        ClosePanel();
    }
}
