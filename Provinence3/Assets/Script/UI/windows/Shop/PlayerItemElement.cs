using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UnderUi
{
    none,
    delete,
    equip
}

public class PlayerItemElement : BaseSimpleElement
{
    public Image equpedImage;
    private Action<PlayerItemElement> callback;
        
    public void Init(BaseItem item,Action<PlayerItemElement> OnClicked)
    {
        base.Init(item);
        this.callback = OnClicked;
        PlayerItem = item;
        Refresh();
    }
    public override void Refresh()
    {
        base.Refresh();
        equpedImage.gameObject.SetActive(PlayerItem.IsEquped);
    }

    public void OnPointerClick()
    {
        callback(this);
    }
    
    public void Equip(bool val)
    {
        equpedImage.gameObject.SetActive(val);
    }
}

