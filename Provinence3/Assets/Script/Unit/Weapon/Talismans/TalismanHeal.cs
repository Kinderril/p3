using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanHeal : Talisman
{
    public override void Use()
    {
        MainController.Instance.level.MainHero.GetHeal(sourseItem.power);
        base.Use();
    }
}

