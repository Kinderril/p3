using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ShopController : Singleton<ShopController>
{
    public static List<TalismanType> AllTalismanstypes = new List<TalismanType>();
    public static List<SpecialAbility> AllSpecialAbilities = new List<SpecialAbility>();
    public static List<ExecutableType> AllExecutables = new List<ExecutableType>();
    public void Init()
    {
        Connections.Init();
        foreach (TalismanType talic_type in Enum.GetValues(typeof(TalismanType)))
        {
            AllTalismanstypes.Add(talic_type);
        }
        foreach (SpecialAbility type in Enum.GetValues(typeof(SpecialAbility)))
        {
            AllSpecialAbilities.Add(type);
        }
        foreach (ExecutableType type in Enum.GetValues(typeof(ExecutableType)))
        {
            AllExecutables.Add(type);
        }
    }

    public void BuyItem(IShopExecute shopItem)
    {
        shopItem.Execute(shopItem.Parameter);
    }

    public static int RandomizeLvl(int baseLvl)
    {
        var lvls = new WDictionary<int>(new Dictionary<int, float>()
        {
            { baseLvl -1,3f },
            { baseLvl ,4f },
            { baseLvl +1,3f },
            { baseLvl +2,0.5f },
        });
        return lvls.Random();
    }

    public static Slot RandomSlot()
    {
        var slots = new WDictionary<Slot>(new Dictionary<Slot, float>()
        {
            { Slot.body,3f },
            { Slot.helm,4f },
            { Slot.magic_weapon,3f },
            { Slot.physical_weapon, 5f },
            { Slot.Talisman, 1f },
        });
        return slots.Random();
    }
    public static Bonustype RandomBonus()
    {
        var slots = new WDictionary<Bonustype>(new Dictionary<Bonustype, float>()
        {
            {Bonustype.money,3f },
            {Bonustype.damage,3f },
        });
        return slots.Random();
    }
    public static Slot RandomAfterLevelGift()
    {
        var slots = new WDictionary<Slot>(new Dictionary<Slot, float>()
        {
            { Slot.body,1f },
            { Slot.helm,1f },
            { Slot.magic_weapon,1f },
            { Slot.physical_weapon, 1f },
            { Slot.Talisman, 1f },
            { Slot.executable, 9f },
            { Slot.bonus, 9f },
        });
        return slots.Random();
    }



}

public static class Connections
{
    private static readonly Dictionary<Slot, WDictionary<ParamType>> primary = new Dictionary<Slot, WDictionary<ParamType>>();
    private static readonly Dictionary<Slot, WDictionary<ParamType>> secondary = new Dictionary<Slot, WDictionary<ParamType>>();
    public static void Init()
    {
        primary.Add(Slot.physical_weapon, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.PPower, 1 } }));
        primary.Add(Slot.magic_weapon, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.MPower, 3 } }));
        primary.Add(Slot.body, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.PDef, 3 }, { ParamType.MDef, 1 } }));
        primary.Add(Slot.helm, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.PDef, 1 }, { ParamType.MDef, 3 } }));

        secondary.Add(Slot.physical_weapon, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.PDef, 1 } }));
        secondary.Add(Slot.magic_weapon, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.MDef, 3 } }));
        secondary.Add(Slot.body, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.Hp, 1 }, { ParamType.Speed, 1 } }));
        secondary.Add(Slot.helm, new WDictionary<ParamType>(new Dictionary<ParamType, float>() { { ParamType.Hp, 1 }, { ParamType.PPower, 3 }, { ParamType.MPower, 3 } }));
    }

    public static ParamType GetPrimaryParamType(Slot slot)
    {
        return primary[slot].Random();
    }
    public static ParamType GetSecondaryParamType(Slot slot)
    {
        return secondary[slot].Random();
    }
}

public class WDictionary<T> : Dictionary<float, T>
{
    private float total = 0;
    public WDictionary(Dictionary<T,float> items )
    {
        float curW = 0;
        foreach (var item in items)
        {
            curW += item.Value;
            Add(curW,item.Key);
        }
        total = curW;
    }

    public T Random()
    {
        var res = UnityEngine.Random.Range(0, total);
        foreach (var key in Keys)
        {
            if (key >= res)
            {
                return this[key];
            }
        }
        return default(T);
    }
}

