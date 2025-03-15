using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPanel : BasePanel
{
    public void OnOpen()
    {
        Debug.Log("UserOpen");
        OpenPanel();
    }

    public void OnClose()
    {
        Debug.Log("UserClose");
        ClosePanel();
    }
}
