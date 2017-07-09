using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AmmoMapItem : BaseMapItem
{
    private ItemId type;
    private int count;


    public void Init(ItemId type, int count,bool fromChest = false)
    {
        base.Init(fromChest);
        this.type = type;
        this.count = (int) Utils.RandomNormal(count*0.7f, count * 1.3f);
    }

    protected override void Take(Hero unit)
    {
        unit.GetItems(type, count);
    }
}

