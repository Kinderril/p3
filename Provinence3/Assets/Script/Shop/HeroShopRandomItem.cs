using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class HeroShopRandomItem : IShopExecute
{
    public void Execute(int level,Slot slot)
    {
        switch (slot)
        {
            case Slot.physical_weapon:
            case Slot.magic_weapon:
            case Slot.body:
            case Slot.helm:
                MainController.Instance.PlayerData.AddItem(CreatMainSlot(slot, level));
                break;
            case Slot.Talisman:
                MainController.Instance.PlayerData.AddItem(CreaTalic(level));
                break;
        }
        randomCreatAdditionalItem();
        base.Execute(level);
    }

    public override int MoneyCost
    {
        get { return Formuls.ShopPlayerItemCost(Parameter); }
    }
    protected void randomCreatAdditionalItem()
    {
        
    }

    public static TalismanItem CreaTalic(int levelResult)
    {
        int point = Formuls.GetTalismanPointsByLvl(levelResult);
        point = (int) Utils.RandomNormal(point*0.7f, point*1.3f);
        var type = ShopController.AllTalismanstypes.RandomElement();
        TalismanItem item = new TalismanItem(point, type);
        return item;
    }

    public static Rarity GetRarity()
    {
        var r = UnityEngine.Random.Range(0, 100);
        if (r < 60)
        {
            return Rarity.Normal;
        }
        if (r < 90)
        {
            return Rarity.Magic;
        }
        return Rarity.Rare;
    }

    public static PlayerItem CreatMainSlot(Slot slot, int level)
    {
        var totalPoints = Formuls.GetPlayerItemPointsByLvl(level) * Formuls.GetSlotCoef(slot);
        float diff = Utils.RandomNormal(0.5f, 1f);
        var primaryValue = totalPoints * diff;

        var rarity = GetRarity();
        var pparams = new Dictionary<ParamType, float>();
        bool addSpecial = false;
        var special = GetSpecial(slot);
        Action paramInit = () =>
        {
            if ((special == SpecialAbility.none || UnityEngine.Random.Range(0, 10) < 5) || addSpecial)
            {
                AddSecondaryParam(slot, pparams, totalPoints);
            }
            else
            {
                addSpecial = true;
            }
        };
        switch (rarity)
        {
            case Rarity.Magic:
                paramInit();
                break;
            case Rarity.Rare:
                paramInit();
                paramInit();
                break;
        }
        
        var primary = Connections.GetPrimaryParamType(slot);
        pparams.Add(primary,primaryValue);
        
        PlayerItem item = new PlayerItem(pparams,slot, rarity, totalPoints);
        if (addSpecial)
        {
            item.specialAbilities = special;
        }
        return item;
    }

    public static void AddSecondaryParam(Slot slot, Dictionary<ParamType, float> pparams,float totalPoints)
    {
    
        var secondaryParam = GetSecondaryParam(totalPoints, slot);
        if (pparams.ContainsKey(secondaryParam.Key))
        {
            pparams[secondaryParam.Key] += secondaryParam.Value;
        }
        else
        {
            pparams.Add(secondaryParam.Key, secondaryParam.Value);
        }

    }

    public static KeyValuePair<ParamType,float> GetSecondaryParam(float totalPoints,Slot slot)
    {
        var secondary = Connections.GetSecondaryParamType(slot);
        var secondaryValue = totalPoints * (0.3f);
        return new KeyValuePair<ParamType, float>(secondary, secondaryValue);
    }

    private static SpecialAbility GetSpecial(Slot slot)
    {
        if (slot == Slot.magic_weapon || slot == Slot.physical_weapon)
        {
            return ShopController.AllSpecialAbilities.RandomElement();
        }
        return SpecialAbility.none;
    }
//    public  static 

   

    public override void Init(int lvl)
    {
        base.Init(lvl);
        name = "Chest";
    }
}

