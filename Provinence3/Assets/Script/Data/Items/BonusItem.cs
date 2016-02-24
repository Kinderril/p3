using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Bonustype
{
    damage,
    money
}

public class BonusItem : BaseItem
{
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
        Slot = Slot.bonus;
    }

    public override void Activate(Hero hero)
    {
        remainUsetime--;
        switch (Bonustype)
        {
            case Bonustype.damage:
                hero.damageBonusFromItem = power;
                break;
            case Bonustype.money:
                hero.moneyBonusFromItem = power;
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

    public static BaseItem Creat(string subStr)
    {
        var mstr = subStr.Split(MDEL);
        var type = (Bonustype) Convert.ToInt32(mstr[1]);
        var power = Convert.ToSingle(mstr[0]);
        var remainUsetime = Convert.ToInt32(mstr[2]);
        return new BonusItem(type,power, remainUsetime);
    }
}

