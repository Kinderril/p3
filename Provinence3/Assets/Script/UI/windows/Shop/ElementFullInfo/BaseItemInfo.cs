using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public abstract class BaseItemInfo : MonoBehaviour
{
    public Text NameLabel;
    public Transform moneyLayout;
    public Image mainIcon;
    public Image SlotLabel;
    protected BaseItem BaseItem;

    protected virtual void Init(BaseItem item,bool sell = true, bool WithButtons = true)
    {
        BaseItem = item;
        if (SlotLabel != null)
        {
            if (item != null)
            {
                SlotLabel.sprite = DataBaseController.Instance.SlotIcon(item.Slot);
            }
            else
            {
                SlotLabel.gameObject.SetActive(false);
            }
        }
        if (sell)
        {
            InitCost(0, item.cost);
        }
    }
    protected void InitCost(int crystals, int money)
    {
        //        Debug.Log("Init cost : " + crystals + "    " + money);
        if (crystals > 0)
        {
            var element = DataBaseController.GetItem<ParameterElement>(DataBaseController.Instance.DataStructs.PrefabsStruct.ParameterElement);
            element.Init(ItemId.crystal, crystals);
            element.transform.SetParent(moneyLayout);
        }
        if (money > 0)
        {
            var element = DataBaseController.GetItem<ParameterElement>(DataBaseController.Instance.DataStructs.PrefabsStruct.ParameterElement);
            element.Init(ItemId.money, money);
            element.transform.SetParent(moneyLayout);
        }
    }

    protected virtual void Refresh()
    {

    }
    protected bool EnoughtMoney(IShopExecute selectedShopElement)
    {
        bool haveMoney = MainController.Instance.PlayerData.CanPay(ItemId.money, selectedShopElement.MoneyCost);
        bool haveCrystal = MainController.Instance.PlayerData.CanPay(ItemId.crystal, selectedShopElement.CrystalCost);
        return haveMoney && haveCrystal;
    }

}

