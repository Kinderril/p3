﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HeroShopRandomItem : IShopExecute
{
    public override void Execute(int level)
    {
        var slot = ShopController.RandomSlot();
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

    private void randomCreatAdditionalItem()
    {
        
    }

    public static TalismanItem CreaTalic(int levelResult)
    {
        int power = GetTalismanPointsByLvl(levelResult);
        var type = ShopController.AllTalismanstypes.RandomElement();
        TalismanItem item = new TalismanItem(power, type);
        return item;
    }

    private static int GetTalismanPointsByLvl(int lvl)
    {
        return lvl * 11 + 33;
    }

    public static PlayerItem CreatMainSlot(Slot slot, int levelResult, ExecCatalysItem catalysItem = null)
    {
        var totalPoints = GetPointsByLvl(levelResult)*GetSlotCoef(slot);
        float diff = 0;
        if (catalysItem == null)
        {
            diff = Utils.RandomNormal(0.5f, 1f);
        }
        else
        {
            diff = Utils.RandomNormal(0.75f, 1f);
        }
        bool isRare = diff > 0.95f;
        totalPoints *= diff;
        float contest = UnityEngine.Random.Range(0.60f, 1f);
        if (contest > 0.9f)
            contest = 1f;

        var pparams = new Dictionary<ParamType,float>();
        var primary = Connections.GetPrimaryParamType(slot);
        var primaryValue = totalPoints*contest;
        pparams.Add(primary,primaryValue);

        if (contest < 0.95f)
        {
            var secondary = Connections.GetSecondaryParamType(slot);
            var secondaryValue = totalPoints*(1f - contest);
            pparams.Add(secondary, secondaryValue);
        }

        PlayerItem item = new PlayerItem(pparams,slot,isRare, totalPoints);
        if ((contest < 0.85f || catalysItem != null) && (slot == Slot.magic_weapon || slot == Slot.physical_weapon))
        {
            SpecialAbility spec;
            if (catalysItem == null)
            {
                spec = ShopController.AllSpecialAbilities.RandomElement();
            }
            else
            {
                spec = catalysItem.GetSpec();
            }
            item.specialAbilities = spec;
        }
        return item;
    }

    private static int GetPointsByLvl(int lvl)
    {
        return lvl*7 + 20;
    }

    private static float GetSlotCoef(Slot slot)
    {
        float val = 1f;
        switch (slot)
        {
            case Slot.physical_weapon:
                val = 1.1f;
                break;
            case Slot.magic_weapon:
                val = 1.3f;
                break;
            case Slot.body:
                val = 1f;
                break;
            case Slot.helm:
                val = 0.8f;
                break;
        }
        return val;
    }

    public HeroShopRandomItem(int lvl) 
        : base(lvl)
    {
        name = "Chest";
    }
}

