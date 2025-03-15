using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel
{
    public void OnSettingPanelOpened()
    {
        isActive = true;
        OpenPanel();
    }

    public void OnSettingPanelClosed()
    {
        isActive = false;
        ClosePanel();
    }
}
