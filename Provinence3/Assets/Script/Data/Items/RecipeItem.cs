using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RecipeItem : BaseItem
{
    public int Level = 1;
    public Slot recipeSlot;
    public const char FIRSTCHAR = '§';
    private List<ExecCraftItem> list = null;

    public RecipeItem(int lvl, Slot slot)
    {
        string ss = "";
        switch (slot)
        {
            case Slot.physical_weapon:
            case Slot.magic_weapon:
                ss = "weapon";
                break;
            case Slot.body:
            case Slot.helm:
                ss = "armor";
                break;
            case Slot.Talisman:
                ss = "scroll";
                break;
        }
        cost = Formuls.RecepiCost(lvl)/3;
        Slot = Slot.recipe;

        IconSprite = UnityEngine.Resources.Load<Sprite>("sprites/RecipeItem/" + ss);
        this.Level = lvl;
        recipeSlot = slot;
    }

    public List<ExecCraftItem> ItemsToCraft()
    {
        if (list == null)
        {
            list = DataBaseController.Instance.CraftDB.GetRecipe(recipeSlot, Level);
        }
        return list;
    } 

    public override string Name
    {
        get
        {
            string sub = "";
            switch (recipeSlot)
            {
                case Slot.physical_weapon:
                    sub = "of weapon";
                    break;
                case Slot.magic_weapon:
                    sub = "of magic weapon";
                    break;
                case Slot.body:
                    sub = "of armor";
                    break;
                case Slot.helm:
                    sub = "of helmet";
                    break;
                case Slot.Talisman:
                    sub = "of talisman";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return "Recipe " + sub;
        }
    }
    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero, Level lvl)
    {

    }

    public void Open()
    {
        
    }

    public override string Save()
    {
        return Level.ToString() + DELEM + ((int)recipeSlot).ToString();
    }

    public static RecipeItem Create(string data)
    {
        var s = data.Split(DELEM);
        int lvl = Convert.ToInt32(s[0]);
        var slot = (Slot)Convert.ToInt32(s[1]);
        return new RecipeItem(lvl, slot);
    }
}

