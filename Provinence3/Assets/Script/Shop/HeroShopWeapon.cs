using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeroShopWeapon : HeroShopRandomItem
{
    public override void Execute(int level)
    {
        var slots = new WDictionary<Slot>(new Dictionary<Slot, float>()
        {
            { Slot.magic_weapon,10f },
            { Slot.physical_weapon, 11f },
            { Slot.Talisman, 1f },
        });
        var slot = slots.Random();
        
        base.Execute(level,slot);
    }
}

