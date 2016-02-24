using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum CatalysItemType
{
    
}

public class ExecCatalysItem : ExecutableItem
{
    public CatalysItemType ItemType;
    public ExecCatalysItem(CatalysItemType type, int count) 
        : base(ExecutableType.catalys, count)
    {

        ItemType = type;
    }

    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }
}

