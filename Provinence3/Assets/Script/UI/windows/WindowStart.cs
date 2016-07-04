using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : BaseWindow
{
    public override void Init()
    {
        base.Init();
        WindowManager.Instance.MainBack.gameObject.SetActive(true);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnClearAllClick()
    {
        PlayerPrefs.DeleteAll();
    }
}

