﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;


public enum TalismanType
{
    //W - need to work
    //2-work
    //1-work - need test
    //0-Cancel
    firewave,//1
    massPush,// W 
    massFreez,//0
    heal,//2
    doubleDamage,//2
    megaArmor,// W
    chain,// W
    trapDamage,// 1
    trapAOE,// 1
    trapFreez,// 1
    bloodDamage,// 1
    energyVamp,//0
    splitter,//1
}

public class TalismanItem : BaseItem
{
    public float power;
    public TalismanType TalismanType;
    public float costShoot;
    public const char FIRSTCHAR = '=';
    public int MaxCharges = 1;
    public int Enchant = 0;


    public TalismanItem(int power, TalismanType type)
    {
        Slot = Slot.Talisman;
        this.TalismanType = type;
        costShoot = power*1.3f;
//        subInit(power);
        IconSprite = DataBaseController.Instance.TalismanIcon(type);
        MaxCharges = GetMaxCharges(type);
        // Debug.Log("cost " + costShoot);
    }
    public TalismanItem(float power1, float costShoot1, TalismanType type)
    {
        this.power = power1;
        this.costShoot = costShoot1;
        this.TalismanType = type;
        IconSprite = DataBaseController.Instance.TalismanIcon(type);
        Slot = Slot.Talisman;
        MaxCharges = GetMaxCharges(type);
    }

    private int GetMaxCharges(TalismanType t)
    {
        switch (TalismanType)
        {
            case TalismanType.firewave:
                return 2;
            case TalismanType.massPush:
                return 1;
            case TalismanType.massFreez:
                return 2;
            case TalismanType.heal:
                return 1;
            case TalismanType.doubleDamage:
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
            case TalismanType.energyVamp:
                return  1;
            case TalismanType.splitter:
                return  2;
        }
        return 1;
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

    public static TalismanItem Create(string subStr)
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

