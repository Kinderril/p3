using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    splitter,
}

public class TalismanItem : BaseItem
{
    public float power;
    public TalismanType TalismanType;
    public float costShoot;
    public const char FIRSTCHAR = '=';
    public int MaxCharges = 1;


    public TalismanItem(int power, TalismanType type)
    {
        Slot = Slot.Talisman;
        this.TalismanType = type;
        costShoot = 1;//power*1.3f;
        subInit(power);
        MaxCharges = GetMaxCharges(type);
        // Debug.Log("cost " + costShoot);
    }

    private int GetMaxCharges(TalismanType t)
    {
        switch (TalismanType)
        {
            case TalismanType.firewave:
                return 2;
            case TalismanType.massPush:
                return 2;
            case TalismanType.massFreez:
                return 2;
            case TalismanType.heal:
                return 1;
            case TalismanType.doubleDamage:
                return 1;
            case TalismanType.speed:
                return 1;
            case TalismanType.megaArmor:
                return  1;
            case TalismanType.chain:
                return  2;
            case TalismanType.trapDamage:
                return  3;
            case TalismanType.trapAOE:
                return  3;
            case TalismanType.trapFreez:
                return  3;
            case TalismanType.bloodDamage:
                return  2;
            case TalismanType.cleave:
                return  1;
            case TalismanType.energyVamp:
                return  1;
            case TalismanType.splitter:
                return  2;
        }
        return 1;
    }

    private void subInit(int totalPoints)
    {
        this.power = totalPoints;
        switch (TalismanType)
        {
            case TalismanType.firewave:
                break;
            case TalismanType.massPush:
                break;
            case TalismanType.massFreez:
                break;
            case TalismanType.heal:
                power *= 1.5f;
                break;
            case TalismanType.doubleDamage:
                power /= 100;
                break;
            case TalismanType.speed:
                power /= 100;
                break;
            case TalismanType.megaArmor:
                power *= 1.5f;
                break;
            case TalismanType.chain:
                break;
            case TalismanType.trapDamage:
                power *= 2;
                break;
            case TalismanType.trapAOE:
                break;
            case TalismanType.trapFreez:
                break;
            case TalismanType.bloodDamage:
                power *= 3f;
                break;
            case TalismanType.cleave:
                break;
            case TalismanType.energyVamp:
                break;
            case TalismanType.splitter:
                power *= 2f;
                break;
        }
    }

    public TalismanItem(float power1, float costShoot1, TalismanType type)
    {
        this.power = power1;
        this.costShoot = 1;//costShoot1;
        this.TalismanType = type;
        Slot = Slot.Talisman;
        MaxCharges = GetMaxCharges(type);
    }

    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero)
    {
        
    }

    public override string Name
    {
        get { return TalismanType.ToString(); }
    }

    public override void LoadTexture()
    {
        IconSprite = DataBaseController.Instance.TalismanIcon(TalismanType);
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

