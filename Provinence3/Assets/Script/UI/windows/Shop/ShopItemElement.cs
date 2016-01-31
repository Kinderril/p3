using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ShopItemElement : MonoBehaviour
{
    public Image icon;
    public IShopExecute shopExecute;
    public Text lvlField;
    public Image overlay;
    private bool isOpen = false;
    private Action<IShopExecute> callback;

    public void Init(IShopExecute shopExecute, Action<IShopExecute> callback)
    {
        this.shopExecute = shopExecute;
        this.callback = callback;
        icon.sprite = shopExecute.icon;
        lvlField.text = "Level:" + shopExecute.Parameter;
        isOpen = shopExecute.CanBuy;
        overlay.gameObject.SetActive(!isOpen);
    }

    public void OnClick()
    {
         callback(shopExecute);
    }

    public bool CanBuy()
    {
        return isOpen;
    }
}

