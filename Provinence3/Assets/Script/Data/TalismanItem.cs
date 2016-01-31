using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum TalismanType
{
    firewave,
    massPush,
    massFreez,
    heal,
    doubleDamage,
    speed,
    megaArmor,
    chain,
    trapDamage,
    trapAOE,
    trapFreez,
    bloodDamage,
    cleave,
    energyVamp,
}

public class TalismanItem : BaseItem
{
    public float power;
    public TalismanType TalismanType;
    public float costShoot;
    public const char FIRSTCHAR = '=';


    public TalismanItem(int totalPoints, TalismanType type)
    {
        this.power = totalPoints;
        this.TalismanType = type;
        costShoot = power*1.3f;
        Slot = Slot.Talisman;
       // Debug.Log("cost " + costShoot);
    }

    public TalismanItem(float power1, float costShoot1, global::TalismanType type)
    {
        this.power = power1;
        this.costShoot = costShoot1;
        this.TalismanType = type;
        Slot = Slot.Talisman;
    }

    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero)
    {
        
    }

    public override string Save()
    {
        return power.ToString() + MDEL + costShoot.ToString() + MDEL + (int)TalismanType + MDEL + IsEquped;
    }

    public static TalismanItem Creat(string subStr)
    {
        Debug.Log("TalismanItem Creat from:   " + subStr);
        var pp = subStr.Split(MDEL);
        float power = Convert.ToSingle(pp[0]);
        float costShoot = Convert.ToSingle(pp[1]);
        global::TalismanType type = (TalismanType)Convert.ToInt32(pp[2]);
        bool isEquiped = Convert.ToBoolean(pp[3]);
        TalismanItem talic = new TalismanItem(power,costShoot,type);
        talic.IsEquped = isEquiped;
        return talic;
    }
}

