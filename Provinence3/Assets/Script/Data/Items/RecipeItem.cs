using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RecipeItem : BaseItem
{
    public int Level = 1;
    public Slot recipeSlot;
    public const char FIRSTCHAR = '§';
    private List<ExecCraftItem> list = null;

    public RecipeItem(int lvl, Slot slot)
    {
        Slot = Slot.recipe;
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
        get { return "Recipe"; }
    }
    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero)
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

