﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BaseItemInfo : MonoBehaviour
{
    public Text NameLabel;
    public Transform moneyLayout;
    public Image mainIcon;
    public Image SlotLabel;

    protected void Init(BaseItem item,bool sell = true)
    {
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
            InitCost(0, item.cost / 3);
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
}

