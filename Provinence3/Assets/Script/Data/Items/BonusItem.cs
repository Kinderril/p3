using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Bonustype
{
    damage,
    defence,
    money,
    cryslats,
    energy,
    speed,
    maxHp,
}

public class BonusItem : BaseItem
{
    public const int BONUS_USE_TIME = 2;
    public float power;
    public Bonustype Bonustype;
    public int remainUsetime;
    public const char FIRSTCHAR = '+';
    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public BonusItem(Bonustype Bonustype,float power, int remainUsetime)
    {
        this.Bonustype = Bonustype;
        this.power = power;
        this.remainUsetime = remainUsetime;
        cost = Formuls.CostBonus(Bonustype, MainController.Instance.PlayerData.Level) * (remainUsetime/ BONUS_USE_TIME);
        IconSprite = UnityEngine.Resources.Load<Sprite>("sprites/BonusItem/" + Bonustype.ToString());
        Slot = Slot.bonus;
    }

    public override void Activate(Hero hero, Level lvl)
    {
        remainUsetime--;
        switch (Bonustype)
        {
            case Bonustype.defence:
                hero.Parameters[ParamType.MDef] *= power;
                hero.Parameters[ParamType.PDef] *= power;
                break;
            case Bonustype.maxHp:
                hero.Parameters[ParamType.Heath] *= power;
                break;
            case Bonustype.speed:
                hero.Parameters[ParamType.Speed] += power;
                break;
            case Bonustype.damage:
                hero.Parameters[ParamType.MPower] *= power;
                hero.Parameters[ParamType.PPower] *= power;
                break;
            case Bonustype.energy:
                lvl.Energy.SpeedEnergyFallCoef = 0.9f;
                break;
            case Bonustype.cryslats:
                lvl.CrystalsBonus = power;
                break;
            case Bonustype.money:
                lvl.MoneyBonusFromItem = power;
                break;
        }
        if (remainUsetime <= 0)
        {
            MainController.Instance.PlayerData.RemoveItem(this);
        }
    }



    public override string Save()
    {
        return power.ToString() + MDEL + (int)Bonustype + MDEL + remainUsetime;
    }

    public static BaseItem Create(string subStr)
    {
        var mstr = subStr.Split(MDEL);
        var type = (Bonustype) Convert.ToInt32(mstr[1]);
        var power = Convert.ToSingle(mstr[0]);
        var remainUsetime = Convert.ToInt32(mstr[2]);
        return new BonusItem(type,power, remainUsetime);
    }
}

