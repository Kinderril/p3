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
    public GameObject Selected;
    private Action<ExecCatalysItem> callback;
    private ExecCatalysItem item;

    public void Init(ExecCatalysItem item,Action<ExecCatalysItem> callback)
    {
        if (item != null)
        {
            image.sprite = item.IconSprite;
            text.text = item.count.ToString("0");
        }
        else
        {
            text.text = "";
        }
        this.item = item;
        this.callback = callback;
        Selected.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        callback(item);
        Selected.gameObject.SetActive(true);
    }

    public void SelectAnother()
    {
        Selected.gameObject.SetActive(false);
    }
}

