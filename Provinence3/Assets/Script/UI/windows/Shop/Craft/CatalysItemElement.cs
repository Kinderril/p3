using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CatalysItemElement : MonoBehaviour
{
    public Image image;
    public Text text;
    private Action<ExecCatalysItem> callback;
    private ExecCatalysItem item;

    public void Init(ExecCatalysItem item,Action<ExecCatalysItem> callback)
    {
        image.sprite = item.IconSprite;
        this.item = item;
        this.callback = callback;
        text.text = item.count.ToString("0");
    }

    public void OnClick()
    {
        callback(item);
    }
}

