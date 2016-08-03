using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class IShopExecute : MonoBehaviour
{
//    public int MoneyCost;
    public int Parameter;
    public Sprite icon;
    public string name = "wrong";
    public bool CanBuy
    {
        get { return MainController.Instance.PlayerData.Level >= Parameter; }
    }

    public virtual int MoneyCost
    {
        get { return 0; }
    }
    public virtual int CrystalCost
    {
        get { return 0; }
    }


    public virtual void Execute(int parameter)
    {
        var playerData = MainController.Instance.PlayerData;
        if (MoneyCost > 0)
            playerData.Pay(ItemId.money, MoneyCost);
        if (CrystalCost > 0)
            playerData.Pay(ItemId.crystal, CrystalCost);
        playerData.Save();
    }

    public virtual void Init(int lvl)
    {
        Parameter = lvl;
        var cost = DataBaseController.Instance.GetCostItemsByLevel(lvl);
        CrystalCost = cost.val2;
    }
}

