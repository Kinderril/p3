﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BaseSummon
{
    public int Id;
    public int IdVisual;
    public float DelayShoot;
    public int ShootCount;

    public BaseSummon(float delay, int cnt)
    {
        DelayShoot = delay;
        ShootCount = cnt;
    }

    public float CalcPower()
    {
        if (ShootCount <= 1f)
        {
            return 1f;
        }
        return ShootCount / 2f;
    }

    public BaseSummon CopyWithRnd()
    { 
        var delay = DelayShoot * SMUtils.Range(0.75f, 1.25f);
        var cntShoot =(int)((float) ShootCount * SMUtils.Range(0.25f, 1.75f));
        var obj = new BaseSummon(delay, cntShoot);
        return obj;
    }
    public string Save()
    {
        var result = Id.ToString() + SMUtils.DELEM + IdVisual + SMUtils.DELEM + DelayShoot.ToString() + SMUtils.DELEM + ShootCount;
        return result;
    }

    public static BaseSummon Load(string info)
    {
        var ss = info.Split(SMUtils.DELEM);
        int Id = Convert.ToInt32(ss[0]);
        int IdVisual = Convert.ToInt32(ss[1]);
        float DelayShoot = Convert.ToSingle(ss[2]);
        int ShootCount = Convert.ToInt32(ss[3]);
        var summon = new BaseSummon(DelayShoot, ShootCount);
        summon.Id = Id;
        summon.IdVisual = IdVisual;
        return summon;
    }
}

