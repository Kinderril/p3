using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CatalysItemType
{
    vampire,
    crit,

}

public class ExecCatalysItem : ExecutableItem
{
    public CatalysItemType ItemType;
    public ExecCatalysItem(CatalysItemType type, int count) 
        : base(ExecutableType.catalys, count)
    {

        IconSprite = UnityEngine.Resources.Load<Sprite>("sprites/Catalys/" + type.ToString());
        ItemType = type;
    }

    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }
}

