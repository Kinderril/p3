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
//    public CatalysPlace CatalysPlace;
//    public Text ResultItemName;

    public CraftResultPlace CraftResultPlace;

    public CraftItemElement CraftItemPrefab;
    public CatalysItemElement CatalysItemPrefab;



    private ExecCatalysItem selectedCatalysItem;
    private List<CraftItemElement> elements = new List<CraftItemElement>();
    private bool canCraft = false;
    private CraftItemElement failElement = null;
    private Sprite iconSprite;
    private Action<BaseItem> OnCraftComplete;
    private List<CatalysItemElement> catalysElements = new List<CatalysItemElement>(); 

    public void Init(RecipeItem recipeItem,Action<BaseItem> OnCraftComplete )
    {
        this.OnCraftComplete = OnCraftComplete;
        gameObject.SetActive(true);
        if (recipeItem.recipeSlot != Slot.Talisman)
        {
            string icon;
            RenderCam.Instance.DoRender(recipeItem.recipeSlot, out icon);
            Utils.LoadTexture(icon,out iconSprite);
        }
        else
        {
//            iconSprite = DataBaseController.Instance.TalismanIcon()
        }
        this.recipeItem = recipeItem;
        elements.Clear();
        BaseWindow.ClearTransform(CraftItemsLayout);
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
        BaseWindow.ClearTransform(CatalysItemsLayout);
        foreach (var cat in allCatalys)
        {
            CatalysItemElement catalysItemElement = DataBaseController.GetItem<CatalysItemElement>(CatalysItemPrefab);
            catalysItemElement.Init(cat as ExecCatalysItem, OnCatalysClick);
            catalysItemElement.transform.SetParent(CatalysItemsLayout,false);
            catalysElements.Add(catalysItemElement);
        }
        CatalysItemElement catalysItemElementFree = DataBaseController.GetItem<CatalysItemElement>(CatalysItemPrefab);
        catalysItemElementFree.Init(null, OnCatalysClick);
        catalysItemElementFree.transform.SetParent(CatalysItemsLayout, false);
        catalysElements.Add(catalysItemElementFree);

        CraftResultPlace.Init(iconSprite, null);
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
        catalysElements.Clear();
    }

    private void CatalysChanges(ExecCatalysItem catalysItem)
    {
        this.selectedCatalysItem = catalysItem;
        foreach (var catalysItemElement in catalysElements)
        {
            if (catalysItemElement.Item != catalysItem)
            {
                catalysItemElement.SelectAnother();
            }
        }
        CraftResultPlace.Init(iconSprite, selectedCatalysItem);
    }

    public void OnSimpleCraft()
    {
        WindowManager.Instance.ConfirmWindow.Init(
            () =>
            {
                var playerItem = MainController.Instance.PlayerData.DoCraft(recipeItem, selectedCatalysItem);
                OnClose();
                if (playerItem != null)
                {
                    OnCraftComplete(playerItem);
                }
            },
            OnClose,
            "You want to do craft?");
    }


}

