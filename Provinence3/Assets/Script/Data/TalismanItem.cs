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


    public TalismanItem(int totalPoints, TalismanType type)
    {
        this.TalismanType = type;
        costShoot = 1;//power*1.3f;
        subInit(totalPoints);
       // Debug.Log("cost " + costShoot);
    }

    private void subInit(int totalPoints)
    {
        Slot = Slot.Talisman;
        this.power = totalPoints;
        switch (TalismanType)
        {
            case TalismanType.firewave:
                MaxCharges = 2;
                break;
            case TalismanType.massPush:
                MaxCharges = 2;
                break;
            case TalismanType.massFreez:
                MaxCharges = 2;
                break;
            case TalismanType.heal:
                MaxCharges = 1;
                power *= 1.5f;
                break;
            case TalismanType.doubleDamage:
                MaxCharges = 1;
                power /= 100;
                break;
            case TalismanType.speed:
                MaxCharges = 1;
                power /= 100;
                break;
            case TalismanType.megaArmor:
                MaxCharges = 1;
                power *= 1.5f;
                break;
            case TalismanType.chain:
                MaxCharges = 2;
                break;
            case TalismanType.trapDamage:
                MaxCharges = 3;
                power *= 2;
                break;
            case TalismanType.trapAOE:
                MaxCharges = 3;
                break;
            case TalismanType.trapFreez:
                MaxCharges = 3;
                break;
            case TalismanType.bloodDamage:
                MaxCharges = 2;
                power *= 3f;
                break;
            case TalismanType.cleave:
                MaxCharges = 1;
                break;
            case TalismanType.energyVamp:
                MaxCharges = 1;
                break;
            case TalismanType.splitter:
                MaxCharges = 2;
                power *= 2f;
                break;
        }
    }

    public TalismanItem(float power1, float costShoot1, global::TalismanType type)
    {
        this.power = power1;
        this.costShoot = 1;//costShoot1;
        this.TalismanType = type;
        subInit();
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

