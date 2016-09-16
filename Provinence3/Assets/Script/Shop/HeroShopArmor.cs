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
            { Slot.body,4f },
            { Slot.helm, 4f },
            { Slot.Talisman, 2f },
        });
        var slot = slots.Random();
        base.Execute(level,slot);
    }
}

