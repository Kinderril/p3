using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SourceType
{
    weapon,
    talisman,
}

public class DeathInfo
{
    public float LastDamage;
    public WeaponType DamageType;
    public SourceType SourceType;
    public Unit Killer;

    public DeathInfo(float LastDamage, WeaponType DamageType, SourceType SourceType, Unit Killer = null)
    {
        this.DamageType = DamageType;
        this.LastDamage = LastDamage;
        this.SourceType = SourceType;
        this.Killer = Killer;
    }
}
