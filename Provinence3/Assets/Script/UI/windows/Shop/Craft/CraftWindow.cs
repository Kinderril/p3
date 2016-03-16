using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CraftWindow : MonoBehaviour
{
    public Transform CraftItemsLayout;
    public Transform CatalysItemsLayout;
    private RecipeItem recipeItem;
//    public Image ResultItemImage;
    public CatalysPlace CatalysPlace;
//    public Text ResultItemName;
    private ExecCatalysItem catalysItem;
    private List<CraftItemElement> elements = new List<CraftItemElement>();
    private bool canCraft = false;
    private CraftItemElement failElement = null;
    public CraftResultPlace CraftResultPlace;

    public CraftItemElement CraftItemPrefab;
    public CatalysItemElement CatalysItemPrefab;

    public void Init(RecipeItem recipeItem)
    {
        gameObject.SetActive(true);
        this.recipeItem = recipeItem;
        elements.Clear();
        BaseWindow.ClearTransform(CraftItemsLayout);
        BaseWindow.ClearTransform(CatalysItemsLayout);
        foreach (var craftElement in recipeItem.ItemsToCraft())
        {
            if (craftElement.count > 0)
            {
                CraftItemElement craftItemElement = DataBaseController.GetItem<CraftItemElement>(CraftItemPrefab);
                craftItemElement.transform.SetParent(CraftItemsLayout, false);
                craftItemElement.Init(craftElement);
                if (!craftItemElement.IsEnought)
                {
                    failElement = craftItemElement;
                    canCraft = false;
                }
            }
        }
        var allCatalys = MainController.Instance.PlayerData.GetAllItems().Where(x => x is ExecCatalysItem);
        foreach (var cat in allCatalys)
        {
            CatalysItemElement catalysItemElement = DataBaseController.GetItem<CatalysItemElement>(CatalysItemPrefab);
            catalysItemElement.Init(cat as ExecCatalysItem, OnCatalysClick);
        }
        CraftResultPlace.Init(recipeItem, null);
    }

    private void OnCatalysClick(ExecCatalysItem execCatalysItem)
    {
        CatalysChanges(execCatalysItem);
    }

    public void OnClearCatalys()
    {
        CatalysChanges(null);
    }

    public void OnCraftClick()
    {
        if (canCraft)
        {
            OnSimpleCraft();
        }
        else
        {
            if (failElement != null)
            {
                failElement.Uprise();
            }
        }
    }
    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    private void CatalysChanges(ExecCatalysItem catalysItem)
    {
        this.catalysItem = catalysItem;
        CatalysPlace.Set(catalysItem);
        CraftResultPlace.Init(recipeItem,catalysItem);
    }

    public void OnSimpleCraft()
    {
        WindowManager.Instance.ConfirmWindow.Init(
            () =>
            {
                MainController.Instance.PlayerData.DoCraft(recipeItem, catalysItem);
                OnClose();
            },
            OnClose,
            "You want to do craft?");
    }


}

