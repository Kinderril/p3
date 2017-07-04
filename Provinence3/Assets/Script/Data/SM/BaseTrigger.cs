using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SpellTriggerType
{
    shoot,
    shootMagic,
    getDamage,
    cast,
    deathNear,
    getGold,
//    questAction,
    getHeal,
    getEffect,

}

public class BaseTrigger
{
    public const float TRIGGER_COEF = 0.85f;
    public int Id;
    public SpellTriggerType TriggerType;
    public int ShootCount;
//    public float DelayShoot;

    public BaseTrigger( int shootCount, SpellTriggerType triggerType)
    {
//        DelayShoot = delayShoot;
        ShootCount = shootCount;
        if (ShootCount < 1)
        {
            ShootCount = 1;
        }
        TriggerType = triggerType;
    }

    public string Save()
    {
        var result = Id.ToString() + SMUtils.DELEM + TriggerType.ToString() + SMUtils.DELEM + SMUtils.DELEM + ShootCount;
        return result;
    }

    public static BaseTrigger Load(string info)
    {
        var ss = info.Split(SMUtils.DELEM);
        int Id = Convert.ToInt32(ss[0]);
        SpellTriggerType TriggerType = (SpellTriggerType)Enum.Parse(typeof(SpellTriggerType), ss[1]);
        int ShootCount = Convert.ToInt32(ss[2]);
        var trigger = new BaseTrigger( ShootCount, TriggerType);
        trigger.Id = Id;
        return trigger;
    }

    public float CalcPower()
    {
        var typeCOef = GetCoefByTiggerType(TriggerType);
        var r = typeCOef * TRIGGER_COEF * ShootCount;
        if (r == 0)
        {
            Console.WriteLine("???");
        }
        return r;
    }

    public static string GetDescByType(SpellTriggerType type)
    {
        switch (type)
        {
            case SpellTriggerType.shoot:
                return " shot any weapon";
            case SpellTriggerType.shootMagic:
                return " shot with crossbow";
            case SpellTriggerType.getDamage:
                return " get damage";
            case SpellTriggerType.cast:
                return " cast any spell";
            case SpellTriggerType.deathNear:
                return " someone become dead near";
            case SpellTriggerType.getGold:
                return " found some gold";
//            case SpellTriggerType.questAction:
//                return " do part of quest";
            case SpellTriggerType.getHeal:
                return " take bonus";
            case SpellTriggerType.getEffect:
                return " get some parameters effect";
        }
        return "";
    }

    private static SpellTriggerType RndClose(SpellTriggerType type)
    {
        int index = (int)type + SMUtils.Range(0, 3);
        var allTriggers = SpellMerger.Triggers2Rnd;
        if (index >= allTriggers.Count)
        {
            index = index - allTriggers.Count + 1;
        }
        var t = (SpellTriggerType)index;
        return t;
    }

    public BaseTrigger CopyWithRnd()
    {
        var cntShoot = (int)((float)ShootCount * SMUtils.Range(0.25f, 1.75f));
        var rnd = RndClose(TriggerType);
        var obj = new BaseTrigger(cntShoot, rnd);
        return obj;
    }

    public static float GetCoefByTiggerType(SpellTriggerType type)
    {
        switch (type)
        {
            case SpellTriggerType.shoot:
                return 1.2f;
            case SpellTriggerType.shootMagic:
                return 1f;
            case SpellTriggerType.getDamage:
                return 1f;
            case SpellTriggerType.cast:
                return 0.9f;
            case SpellTriggerType.deathNear:
                return 0.75f;
            case SpellTriggerType.getGold:
                return 0.7f;
//            case SpellTriggerType.questAction:
//                return 0.4f;
            case SpellTriggerType.getHeal:
                return 0.65f;
            case SpellTriggerType.getEffect:
                return 0.7f;
        }
        return 1f;
    }

    public static BaseTrigger Merge(BaseTrigger t1, BaseTrigger t2)
    {
        if (t1 == null && t2 == null)
        {
            var r = SpellMerger.Triggers2Rnd.RandomElement();
            return new BaseTrigger(SMUtils.Range(1,2), r);
        }
        if (t1 == null)
        {
            return t2.CopyWithRnd();
        }
        if (t2 == null)
        {
            return t1.CopyWithRnd();
        }
        SpellTriggerType resultType;
        var rnd1 = RndClose(t1.TriggerType);
        var rnd2 = RndClose(t2.TriggerType);
        if (SMUtils.Range(0, 100) < 50)
        {
            resultType = rnd2;
        }
        else
        {
            resultType = rnd1;
        }
        List<int> shoots = new List<int>()
        {
            t1.ShootCount,t2.ShootCount,
        };
        int shootsRes = shoots.RandomElement();
        return new BaseTrigger(shootsRes,resultType);
    }
}

