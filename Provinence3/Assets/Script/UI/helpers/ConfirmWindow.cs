using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ConfirmWindow : MonoBehaviour
{
    private Action onConfirm;
    private Action onReject ;
    public Text labelField;
    public void Init(Action onConfirm, Action onReject,string ss)
    {
        this.onConfirm = onConfirm;
        this.onReject = onReject;
        labelField.text = ss;
        gameObject.SetActive(true);
    }

    public void OnConfirmClick()
    {
        if (onConfirm != null)
            onConfirm();
        gameObject.SetActive(false);
    }
    public void OnRejectClick()
    {
        if (onReject != null)
            onReject();
        gameObject.SetActive(false);
    }
}

