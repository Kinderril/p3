using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BaseSummon
{
    public float DelayShoot;
    public float ShootCount;

    public float CalcPower()
    {
        return ShootCount;
    }

    public BaseSummon CopyWithRnd()
    { 
        var obj = new BaseSummon();
        var delay = DelayShoot * UnityEngine.Random.Range(0.75f, 1.25f);
        obj.DelayShoot = delay;
        obj.ShootCount = ShootCount;
        return obj;
    }
}

