using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanSpeed : Talisman
{

    public override void Use()
    {
        var e = new ParameterEffect(hero, 10, ParamType.Speed, 1.5f);
        TimeEffect.Creat(hero, e);
        base.Use();
    }
}

