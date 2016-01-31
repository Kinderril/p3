using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class IShopExecute : MonoBehaviour
{
    public int CrystalCost;
    public int MoneyCost;
    public int Parameter;
    public Sprite icon;
    public bool CanBuy
    {
        get { return MainController.Instance.PlayerData.Level >= Parameter; }
    }

    public  virtual void Execute(int parameter)
    {
        if (MoneyCost > 0)
            MainController.Instance.PlayerData.Pay(ItemId.money, MoneyCost);
        if (CrystalCost > 0)
            MainController.Instance.PlayerData.Pay(ItemId.crystal, CrystalCost);

    }
}

