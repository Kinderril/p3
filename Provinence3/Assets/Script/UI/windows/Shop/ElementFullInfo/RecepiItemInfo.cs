using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class RecepiItemInfo : InventoryItemInfo
{
    public Text descField;
    public Button RecipeButton;
    public void Init(RecipeItem recipeItem,bool WithButtons)
    {
        base.Init(recipeItem,true, WithButtons);
        mainIcon.sprite = recipeItem.IconSprite;
        NameLabel.text = recipeItem.Name;
        RecipeButton.gameObject.SetActive(WithButtons);
        //        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(recipeItem.Slot);
    }
    public void OnRecipeOpen()
    {
        var recipe = BaseItem as RecipeItem;
        var shop = WindowManager.Instance.CurrentWindow as WindowShop;
        if (shop != null)
        {
            shop.OpenCraft(recipe);
        }
    }
}
