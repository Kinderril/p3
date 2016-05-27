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
        var totalPoints = GetPointsByLvl(levelResult)*GetSlotCoef(slot);
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
        switch (rarity)
        {
            case Rarity.Magic:
                if (UnityEngine.Random.Range(0, 10) < 5)
                {
                    var secondary = Connections.GetSecondaryParamType(slot);
                    var secondaryValue = totalPoints * (0.3f);
                    pparams.Add(secondary, secondaryValue);
                }
                else
                {
                    SpecialAbility spec;
                    spec = ShopController.AllSpecialAbilities.RandomElement();
                }


                break;
            case Rarity.Rare:

                break;
        }

//        float contest = UnityEngine.Random.Range(0.60f, 1f);
//        if (contest > 0.9f)
//            contest = 1f;

        var primary = Connections.GetPrimaryParamType(slot);
        pparams.Add(primary,primaryValue);
        

        PlayerItem item = new PlayerItem(pparams,slot, rarity, totalPoints);
        if ((catalysItem != null) && (slot == Slot.magic_weapon || slot == Slot.physical_weapon))
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

    public  static 

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

    public override void Init(int lvl)
    {
        base.Init(lvl);
        name = "Chest";
    }
}

