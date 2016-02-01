using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapFreez : Talisman
{
    public const string WAY_CHAIN_BULLET = "";
    private AOETrap cacheGameObject;
    public TalismanTrapFreez(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {
        cacheGameObject = Resources.Load(WAY_CHAIN_BULLET, typeof(AOETrap)) as AOETrap;
    }
    public override void Use()
    {
        var p = hero.transform.position;
        var item = DataBaseController.GetItem<AOETrap>(cacheGameObject, p);
        base.Use();
    }
}

