using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class InfoWindow : MonoBehaviour
{
    private Action onOK;
    public Text textField;
    public void Init(Action onOK,string msg)
    {
        this.onOK = onOK;
        textField.text = msg;
        gameObject.SetActive(true);
    }

    public void OnClickOk()
    {
        if (onOK != null)
            onOK();
        gameObject.SetActive(false);
    }
}

