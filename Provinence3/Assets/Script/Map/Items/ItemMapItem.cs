using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ItemMapItem : BaseMapItem
{
    private CraftItemType type;
    private int count;

    public void Init(CraftItemType type,int count = 1)
    {
        base.Init(false);
        this.count = count;
        this.type = type;
        Utils.GroundTransform(transform);
    }

    protected override void Take(Hero unit)
    {
        unit.GetItem(type, count);
    }
}

