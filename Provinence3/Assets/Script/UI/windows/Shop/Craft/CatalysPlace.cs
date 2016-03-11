using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CatalysPlace : MonoBehaviour
{
    public Image img;
    

    public void Set(ExecCatalysItem catalysItem)
    {
        if (catalysItem == null)
        {
            img.gameObject.SetActive(false);
            return;
        }
        else
        {
            img.sprite = catalysItem.IconSprite;
            img.gameObject.SetActive(true);
        }
    }
}

