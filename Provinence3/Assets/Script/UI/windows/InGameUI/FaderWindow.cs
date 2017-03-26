using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class FaderWindow : MonoBehaviour
{
    private const string open = "open";
    private const string close = "close";

    public Animator anim;
    public GameObject info;
    public GameObject OpenGameObject;
    public GameObject CloseGameObject;

    public void Open()
    {
        WithGO(true);
        gameObject.SetActive(true);
        info.gameObject.SetActive(true);
        anim.SetTrigger(open);
    }

    private void WithGO(bool val)
    {
        if (OpenGameObject != null)
        {
            OpenGameObject.gameObject.SetActive(val);
        }
        if (CloseGameObject != null)
        {
            CloseGameObject.gameObject.SetActive(!val);
        }
    }

    public void OnOpenComplete()
    {
        info.gameObject.SetActive(false);
    }

    public void Close()
    {
        WithGO(false);
        gameObject.SetActive(true);
        info.gameObject.SetActive(true);

        anim.SetTrigger(close);
    }


}

