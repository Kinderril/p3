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

    public void Open()
    {
        gameObject.SetActive(true);
        info.gameObject.SetActive(true);
        anim.SetTrigger(open);
    }

    public void OnOpenComplete()
    {
        info.gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(true);
        info.gameObject.SetActive(true);
        anim.SetTrigger(close);
    }


}

