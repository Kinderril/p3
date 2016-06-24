using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class RecepiItemInfo : BaseItemInfo
{
    public Text descField;
    public void Init(RecipeItem recipeItem)
    {
        base.Init(recipeItem);
        mainIcon.sprite = recipeItem.IconSprite;
        NameLabel.text = recipeItem.Name;
        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(recipeItem.Slot);
    }
}
