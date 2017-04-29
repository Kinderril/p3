using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeroShopArmor : HeroShopRandomItem
{
    public override void Execute(int level)
    {
        var slots = new WDictionary<Slot>(new Dictionary<Slot, float>()
        {
            { Slot.body,11f },
            { Slot.helm, 11f },
            { Slot.Talisman, 1f },
        });
        var slot = slots.Random();
        base.Execute(level,slot);
    }
}

