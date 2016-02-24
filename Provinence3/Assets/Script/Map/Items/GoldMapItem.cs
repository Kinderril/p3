using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GoldMapItem : BaseMapItem
{
    private ItemId type;
    private int count;


    public void Init(ItemId type, int count)
    {
        base.Init();
        this.type = type;
        this.count = count;
    }

    protected override void Take(Hero unit)
    {
        unit.GetItems(type, count);
    }
}

