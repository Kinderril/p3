using System;
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
    firewave,//2
    massPush,//0 
    massFreez,//0
    heal,//2
    doubleDamage,//2
    megaArmor,// 2
    chain,// 2
    trapDamage,// 2
    trapAOE,// 2
    trapFreez,// 1
    bloodDamage,// 2
    energyVamp,//0
    splitter,//2
}

public class TalismanItem : BaseItem
{
    public float points;
    public TalismanType TalismanType;
    public float costShoot;
    public const char FIRSTCHAR = '=';
    public int MaxCharges = 1;
    public int Enchant = 0;


    public TalismanItem(int points, TalismanType type)
    {
        Slot = Slot.Talisman;
        this.TalismanType = type;
        this.points = points;
        costShoot = GreatRandom.RandomizeValue(GetBaseCost(type)*Formuls.BASE_MOSTER_ENERGY)/2;
        IconSprite = DataBaseController.Instance.TalismanIcon(type);
        MaxCharges = GetMaxCharges(type);
        cost = Formuls.CostTalismanBypoints(points);
    }
    public TalismanItem(float power1, float costShoot1, TalismanType type)
    {
        cost = Formuls.CostTalismanBypoints((int)power1);
        this.points = power1;
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

    private int GetBaseCost(TalismanType t)
    {
        switch (TalismanType)
        {
            case TalismanType.firewave:
                return 6;
            case TalismanType.massPush:
                return 6;
            case TalismanType.massFreez:
                return 6;
            case TalismanType.heal:
                return 5;
            case TalismanType.doubleDamage:
                return 4;
            case TalismanType.megaArmor:
                return 4;
            case TalismanType.chain:
                return 6;
            case TalismanType.trapDamage:
                return 5;
            case TalismanType.trapAOE:
                return 7;
            case TalismanType.trapFreez:
                return 7;
            case TalismanType.bloodDamage:
                return 4;
            case TalismanType.energyVamp:
                return 5;
            case TalismanType.splitter:
                return 5;
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
        return points.ToString() + MDEL + costShoot.ToString() + MDEL + (int)TalismanType + MDEL + IsEquped;
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

