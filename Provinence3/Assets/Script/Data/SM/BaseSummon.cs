using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseSummon
{
    public const float LIFE_TIME_SEC = 15;

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
        var c = (DelayShoot - 2f);
        c = Mathf.Clamp(c, 1f, 3f);
        c = 1/c;
        if (ShootCount <= 1f)
        {
            return c * 1f;
        }
        return c *  ShootCount * 0.5f;
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

    public static BaseSummon Merge(BaseSummon s1, BaseSummon s2)
    {
        if (s1 == null && s2 == null)
        {
            return new BaseSummon(SMUtils.Range(0.8f, 3.5f), SMUtils.Range(2,5));
        }

        if (s1 == null)
        {
            return s2.CopyWithRnd();
        }
        if (s2 == null)
        {
            return s1.CopyWithRnd();
        }

        var shootsResult = Mathf.Clamp((int)SMUtils.Range(s1.ShootCount*SMUtils.Range(0.5f,1.5f), s2.ShootCount * SMUtils.Range(0.5f, 1.5f)),1,100);
        var delay = SMUtils.Range(s1.DelayShoot*SMUtils.Range(0.5f,1.5f), s2.DelayShoot * SMUtils.Range(0.5f, 1.5f));

        return new BaseSummon(delay, shootsResult);
    }
}

