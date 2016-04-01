using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PreStartWindow : MonoBehaviour
{
    public Button StartButton;
    private Action callback;
    public void Init(Action callback)
    {
        this.callback = callback;
    }

    public void OnStart()
    {
        gameObject.SetActive(false);
        callback();
    }
}

