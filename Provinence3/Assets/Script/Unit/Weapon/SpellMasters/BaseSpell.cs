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
}

public class BaseSpell
{
    public int Id;
    public int Cost;
    public int BulletCount;
    public int Charges;
    public SpellTargetType TargetType;
    public SpellTargetType StartType;
    public BaseBullet Bullet;
    public SpellCoreType SpellCoreType;
    public BaseSummon BaseSummon;
    public int Level;
    public Color ColorForMaterial;

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
    }


    public string Desc(bool withCostCharges)
    {
        var cost = " Cost: " + Cost;
        var charges = "Charges: " + Charges;
        var targetType ="";
        switch (SpellCoreType)
        {
            case SpellCoreType.Shoot:
                targetType = "Cast to ";
                switch (TargetType)
                {
                    case SpellTargetType.Self:
                        targetType += "Self";
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
                break;
            case SpellCoreType.Summon:
                targetType = "Summon totem casting every " + BaseSummon.DelayShoot + " Sec " + BaseSummon.ShootCount + " times.";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return targetType + " <> " + Bullet.Desc(BulletCount) + (withCostCharges?(cost + charges):"");
        
    }
}

