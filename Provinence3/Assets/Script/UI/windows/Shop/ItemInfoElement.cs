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
    public bool WithButtons = true;

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
                PlayerItemInfo.Init(item as PlayerItem, WithButtons);
                CurItemInfo = PlayerItemInfo;
                break;
            case BonusItem.FIRSTCHAR:
                BonusItemInfo BonusItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.BonusItemInfo);
                BonusItemInfo.Init(item as BonusItem, WithButtons);
                CurItemInfo = BonusItemInfo;
                break;
            case ExecutableItem.FIRSTCHAR:
                ExecutableItemInfo ExecutableItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.ExecutableItemInfo);
                ExecutableItemInfo.Init(item as ExecutableItem, WithButtons);
                CurItemInfo = ExecutableItemInfo;
                break;
            case TalismanItem.FIRSTCHAR:
                TalismanItemInfo TalismanItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.TalismanItemInfo);
                TalismanItemInfo.Init(item as TalismanItem, WithButtons);
                CurItemInfo = TalismanItemInfo;
                break;
            case SpellItem.FIRSTCHAR:
                SpellItemInfo SpellItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.SpellItemInfo);
                SpellItemInfo.Init(item as SpellItem, WithButtons);
                CurItemInfo = SpellItemInfo;
                break;
            case RecipeItem.FIRSTCHAR:
                RecepiItemInfo RecepiItemInfo = DataBaseController.GetItem(DataBaseController.Instance.DataStructs.PrefabsStruct.RecepiItemInfo);
                RecepiItemInfo.Init(item as RecipeItem, WithButtons);
                CurItemInfo = RecepiItemInfo;
                break;

        }
        Link();
//        if (OnInitCallback != null)
//            OnInitCallback(item,ItemOwner.Player);
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
//        if (OnInitCallback != null)
//            OnInitCallback(null, ItemOwner.Shop);
    }

    
}

