using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapDamage : Talisman
{
    private string BULLET_WAY = "";
    public TalismanTrapDamage(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {
    }
    public override void Use()
    {
        //TODO
        var p= hero.transform.position;
        var res = Resources.Load<Bullet>("BULLET_WAY");
        var item = DataBaseController.GetItem<Bullet>(res, p);
        base.Use();
    }
}

