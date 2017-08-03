using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum SpellTargetType
{
    Self,
    ClosestsEnemy,
    LookDirection,
}

public enum SpellCoreType
{
    Shoot,
    Summon,
    Trigger,
}

public class BaseSpell
{
    public int Id;
    public string Name;
    public int IdIcon;
    public int IdCast;
    public Color ColorForMaterial;
    public int Cost;
    public int ValueGold;
    public int BulletCount;
    public int Charges;
    public SpellTargetType TargetType;
    public SpellTargetType StartType;
    public BaseBullet Bullet;
    public SpellCoreType SpellCoreType;
    public BaseSummon BaseSummon;
    public BaseTrigger BaseTrigger;
    public int Level;
//    public EffectPositiveType EffectPositiveType;

    public BaseSpell(SpellTargetType start, SpellTargetType end, SpellCoreType core, 
        int charges, int cost, int bulletCount, int level)
    {
        this.StartType = start;
        this.TargetType = end;
        this.SpellCoreType = core;
        Charges = charges;
        Level = level;
        BulletCount = bulletCount;
        Cost = cost;
        Name = SpellNameGenerator.GetName();
//        EffectPositiveType = IsPositive();
    }

    public EffectPositiveType IsPositive()
    {

        return TargetType == SpellTargetType.Self ? EffectPositiveType.Positive : EffectPositiveType.Negative;
    }
//    private EffectPositiveType IsPositive()
//    {
//
//        EffectPositiveType type = EffectPositiveType.Negative;
//
//        List< EffectPositiveType > effectse = new List<EffectPositiveType>();
//        foreach (var baseEffect in Bullet.Effect)
//        {
//            if (baseEffect.Value > 0)
//            {
//                effectse.Add(EffectPositiveType.Positive);
//            }
//            else
//            {
//                effectse.Add(EffectPositiveType.Negative);
//            }
//        }
//        return effectse[0];
//    }

    public string Save()
    {
        var spell = Id.ToString() + SMUtils.DELEM +   //0
                     IdIcon.ToString() + SMUtils.DELEM +//1
                     IdCast.ToString() + SMUtils.DELEM +//2
                     ColorForMaterial + SMUtils.DELEM +//3
                     Cost + SMUtils.DELEM +//4
                     BulletCount + SMUtils.DELEM +//5
                     Charges + SMUtils.DELEM +//6
                     TargetType.ToString() + SMUtils.DELEM +//7
                     StartType.ToString() + SMUtils.DELEM +//8
                     Bullet.Id + SMUtils.DELEM +//9
                     SpellCoreType.ToString() + SMUtils.DELEM +//10
                     ValueGold.ToString() + SMUtils.DELEM +//11
                     (BaseSummon != null?BaseSummon.Id:-1) + SMUtils.DELEM +//12
                     (BaseTrigger != null? BaseTrigger.Id:-1) + SMUtils.DELEM +//13
                     Level + SMUtils.DELEM + //14
                     Name + SMUtils.DELEM;//15
        return spell;
    }

    public static BaseSpell Load(string spell)
    {
        var strs = spell.Split(SMUtils.DELEM);
        int id = Convert.ToInt32(strs[0]);
        int IdIcon = Convert.ToInt32(strs[1]);
        int IdCast = Convert.ToInt32(strs[2]);
        string color = strs[3];
        int Cost = Convert.ToInt32(strs[4]);
        int BulletCount = Convert.ToInt32(strs[5]);
        int Charges = Convert.ToInt32(strs[6]);

        var TargetType = (SpellTargetType)Enum.Parse(typeof(SpellTargetType), strs[7]);
        var StartType = (SpellTargetType)Enum.Parse(typeof(SpellTargetType), strs[8]);
        int Bullet = Convert.ToInt32(strs[9]);
        var SpellCoreType = (SpellCoreType)Enum.Parse(typeof(SpellCoreType), strs[10]);
        int ValueGold = Convert.ToInt32(strs[11]);
        int BaseSummon = Convert.ToInt32(strs[12]);
        int BaseTrigger = Convert.ToInt32(strs[13]);
        int Level = Convert.ToInt32(strs[14]);
        string Name = strs[15];
        BaseSpell result = new BaseSpell(StartType,TargetType,SpellCoreType,Charges,Cost,BulletCount,Level);
        result.Id = id;
        result.IdIcon = IdIcon;
        result.IdCast = IdCast;
        result.Charges = Charges;
        result.ValueGold = ValueGold;
        result.Name = Name;
        result.Bullet = SpellsDataBase.Bullets[Bullet];
        if (BaseSummon > 0)
        {
            result.BaseSummon = SpellsDataBase.Summons[BaseSummon];
        }
        if (BaseTrigger > 0)
        {
            result.BaseTrigger = SpellsDataBase.Triggers[BaseTrigger];
        }

        return result;
    }

    public string DescFull(bool withCostCharges)
    {
        var name = " Name:" + Name + " ";
        var cost = " Cost:" + Cost + " ";
        var charges = "Charges:" + Charges + " ";
        var targetType = "";
        targetType = "Cast to ";
        switch (TargetType)
        {
            case SpellTargetType.Self:
                targetType += "Hero";
                break;
            case SpellTargetType.ClosestsEnemy:
                targetType += "Closest enemy";
                break;
            case SpellTargetType.LookDirection:
                targetType += "Look direction";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        switch (SpellCoreType)
        {
            case SpellCoreType.Trigger:
                targetType = "\n Trigger spell when " + BaseTrigger.GetDescByType(BaseTrigger.TriggerType) + ". " + targetType + "   shoots:" + BaseTrigger.ShootCount;
                break;
            case SpellCoreType.Shoot:

                break;
            case SpellCoreType.Summon:
                targetType = "\n Summon totem casting every " + BaseSummon.DelayShoot.ToString("0.0") + " Sec " + BaseSummon.ShootCount + " times. " + targetType;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return name + targetType + "\n <> " + Bullet.DescFull(this) + " \n" + (withCostCharges ? (cost + charges) : "");
    }

    public string Desc()
    {
//        var name = " Name:" + Name + " ";
//        var cost = " Cost:" + Cost + " ";
//        var charges = "Charges:" + Charges + " ";
        var targetType = "";
        switch (SpellCoreType)
        {
            case SpellCoreType.Shoot:
                switch (TargetType)
                {
                    case SpellTargetType.Self:
                        targetType = "Cast to hero";
                        break;
                    case SpellTargetType.ClosestsEnemy:
                        targetType = "Cast to closest enemy";
                        break;
                    case SpellTargetType.LookDirection:
                        targetType = "Look direction";
                        break;
                }

                break;
            case SpellCoreType.Summon:
                switch (TargetType)
                {
                    case SpellTargetType.Self:
                        targetType = "to hero";
                        break;
                    case SpellTargetType.ClosestsEnemy:
                        targetType = "to closest enemy";
                        break;
                }
                targetType = "Summon totem casting every " + BaseSummon.DelayShoot.ToString("0.0") + " Sec " + BaseSummon.ShootCount + " times " + targetType;
                break;
            case SpellCoreType.Trigger:
                targetType = "Trigger spell when" + BaseTrigger.GetDescByType(BaseTrigger.TriggerType)  + targetType;
                break;
        }
        return  targetType + ". " + Bullet.Desc(this,IsPositive());
    }
}

