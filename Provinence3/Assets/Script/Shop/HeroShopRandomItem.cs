using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HeroShopRandomItem : IShopExecute
{
    public override void Execute(int level)
    {
        var slot = ShopController.RandomSlot();
//        Debug.Log("SLOT:" + slot);
        var levelResult = ShopController.RandomizeLvl(level);
        switch (slot)
        {
            case Slot.physical_weapon:
            case Slot.magic_weapon:
            case Slot.body:
            case Slot.helm:
                MainController.Instance.PlayerData.AddItem(CreatMainSlot(slot, levelResult));
                break;
            case Slot.Talisman:
                MainController.Instance.PlayerData.AddItem(CreaTalic(levelResult));
                break;
        }
        randomCreatAdditionalItem();
        base.Execute(level);
    }

    public override int MoneyCost
    {
        get { return Formuls.ChestItemCost(Parameter); }
    }
    private void randomCreatAdditionalItem()
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

    public static PlayerItem CreatMainSlot(Slot slot, int levelResult, ExecCatalysItem catalysItem = null)
    {
        var totalPoints = Formuls.GetPlayerItemPointsByLvl(levelResult)* Formuls.GetSlotCoef(slot);
        var rarity = GetRarity();
        float diff = 0;
        if (catalysItem == null)
        {
            diff = Utils.RandomNormal(0.5f, 1f);
        }
        else
        {
            diff = Utils.RandomNormal(0.75f, 1f);
        }
        var primaryValue = totalPoints * diff;

        var pparams = new Dictionary<ParamType, float>();
        bool addSpecial = false;
        var special = GetSpecial(slot, catalysItem);
        Action paramInit = () =>
        {
            if ((special == SpecialAbility.none || UnityEngine.Random.Range(0, 10) < 5) || addSpecial)
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

    private static KeyValuePair<ParamType,float> GetSecondaryParam(float totalPoints,Slot slot)
    {
        var secondary = Connections.GetSecondaryParamType(slot);
        var secondaryValue = totalPoints * (0.3f);
        return new KeyValuePair<ParamType, float>(secondary, secondaryValue);
    }

    private static SpecialAbility GetSpecial(Slot slot, ExecCatalysItem catalysItem = null)
    {
        if (slot == Slot.magic_weapon || slot == Slot.physical_weapon)
        {
            if (catalysItem == null)
            {
                return ShopController.AllSpecialAbilities.RandomElement();
            }
            else
            {
                return catalysItem.GetSpec();
            }
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

