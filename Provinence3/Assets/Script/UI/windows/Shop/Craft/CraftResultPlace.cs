using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CraftResultPlace : MonoBehaviour
{

    public Image MainImage;
//    public Text mainParameter;
    public Image IconRune;

    public void Init(Sprite mainIcon, ExecCatalysItem catalys = null)
    {
        bool haveCatalys = catalys != null;
        if (haveCatalys)
        {
            IconRune.sprite = catalys.IconSprite;
        }
        MainImage.sprite = mainIcon;
        IconRune.gameObject.SetActive(haveCatalys);

    }
}

