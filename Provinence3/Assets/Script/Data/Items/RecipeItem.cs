using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RecipeItem : BaseItem
{
    public const float RED_COEF = 1.25f;
    public const float BLACK_MIN_COEF = 1f;
    public const float BLACK_MAX_COEF = 1.5f;

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

    public PlayerItem Craft(ExecCatalysItem catalysItem)
    {
        var resultItem = HeroShopRandomItem.CreatMainSlot(recipeSlot, Level);
        if (catalysItem != null)
        {
            var primaryType = Connections.GetPrimaryParamType(resultItem.Slot);
            switch (resultItem.Slot)
            {
                case Slot.physical_weapon:
                case Slot.magic_weapon:
                    var spedAbilities = PosibleAbilities(catalysItem.ItemType);
//                    var sa = ;
                    resultItem.specialAbilities = spedAbilities.RandomElement();
                    break;
                case Slot.body:
                case Slot.helm:
                    var paramMain = resultItem.parameters[primaryType];
                    switch (catalysItem.ItemType)
                    {
                        case CatalysItemType.red:
                            paramMain *= RED_COEF;
                            resultItem.parameters[primaryType] = paramMain;
                            break;
                        case CatalysItemType.blue:
                        case CatalysItemType.green:
                            HeroShopRandomItem.AddSecondaryParam(resultItem.Slot, resultItem.parameters, paramMain);
                            break;
                        case CatalysItemType.black:
                            paramMain *= UnityEngine.Random.Range(BLACK_MIN_COEF, BLACK_MAX_COEF);
                            resultItem.parameters[primaryType] = paramMain;
                            break;
                        case CatalysItemType.white:
                            resultItem.cost *= 2;
                            break;
                    }
                    break;
                case Slot.Talisman:
                    switch (catalysItem.ItemType)
                    {
                        case CatalysItemType.red:
                            break;
                        case CatalysItemType.blue:
                            break;
                        case CatalysItemType.green:
                            break;
                        case CatalysItemType.black:
                            break;
                        case CatalysItemType.white:
                            break;
                    }
                    break;
            }
        }
        return resultItem;
    }

    public static List<SpecialAbility> PosibleAbilities(CatalysItemType type)
    {
        List<SpecialAbility> spedAbilities = null;
        switch (type)
        {
            case CatalysItemType.red:
                spedAbilities = new List<SpecialAbility>()
                            {
                                SpecialAbility.clear,SpecialAbility.critical
                            };
                break;
            case CatalysItemType.blue:
                spedAbilities = new List<SpecialAbility>()
                            {
                                SpecialAbility.distance,SpecialAbility.hp
                            };
                break;
            case CatalysItemType.green:
                spedAbilities = new List<SpecialAbility>()
                            {
                                SpecialAbility.shield,SpecialAbility.vampire
                            };
                break;
            case CatalysItemType.black:
                spedAbilities = new List<SpecialAbility>()
                            {
                                SpecialAbility.slow,SpecialAbility.stun
                            };
                break;
            case CatalysItemType.white:
                spedAbilities = new List<SpecialAbility>()
                            {
                                SpecialAbility.removeDefence,SpecialAbility.critical
                            };
                break;
        }
        return spedAbilities;
    } 
}

