using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum ItemOwner
{
    Shop,
    Player
}
public class ItemInfoElement : MonoBehaviour
{
    private BaseItemInfo CurItemInfo;
    private Action<BaseItem, ItemOwner> OnInitCallback;

    public void SetCallBack(Action<BaseItem, ItemOwner> OnInitCallback)
    {
        this.OnInitCallback = OnInitCallback;
    }


    private void Clear()
    {
        if (CurItemInfo != null)
        {
            GameObject.Destroy(CurItemInfo.gameObject);
        }
    }
    public void Init(BaseItem item)
    {
        Clear();
        switch (item.FirstChar())
        {
            case PlayerItem.FIRSTCHAR:
                PlayerItemInfo PlayerItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.PlayerItemInfo);
                PlayerItemInfo.Init(item as PlayerItem);
                CurItemInfo = PlayerItemInfo;
                break;
            case BonusItem.FIRSTCHAR:
                BonusItemInfo BonusItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.BonusItemInfo);
                BonusItemInfo.Init(item as BonusItem);
                CurItemInfo = BonusItemInfo;
                break;
            case ExecutableItem.FIRSTCHAR:
                ExecutableItemInfo ExecutableItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.ExecutableItemInfo);
                ExecutableItemInfo.Init(item as ExecutableItem);
                CurItemInfo = ExecutableItemInfo;
                break;
            case TalismanItem.FIRSTCHAR:
                TalismanItemInfo TalismanItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.TalismanItemInfo);
                TalismanItemInfo.Init(item as TalismanItem);
                CurItemInfo = TalismanItemInfo;
                break;
        }
        Link();
        if (OnInitCallback != null)
            OnInitCallback(item,ItemOwner.Player);
    }

    private void Link()
    {
        CurItemInfo.transform.SetParent(transform, false);
    }
    

    public void Init(IShopExecute item)
    {
        Clear();
        ShopSellElement ShopSellElement = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.ShopSellElement);
        ShopSellElement.Init(item);
        CurItemInfo = ShopSellElement;
        Link();
        if (OnInitCallback != null)
            OnInitCallback(null, ItemOwner.Shop);
    }

    
}

