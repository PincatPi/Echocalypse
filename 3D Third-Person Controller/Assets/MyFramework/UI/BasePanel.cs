using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isActive = false;
    protected new string name;

    public virtual void OpenPanel()
    {
        isActive = true;
        this.gameObject.SetActive(true);
        //UIManager.Instance.OpenPanel(gameObject.name);
    }
    
    public virtual void ClosePanel()
    {
        isActive = false;
        this.gameObject.SetActive(false);
        //UIManager.Instance.ClosePanel(gameObject.name);
    }
}
