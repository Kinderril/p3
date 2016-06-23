using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Formuls
{

    public static float calcResist(float curResist)
    {
        return 1 - curResist / (100 + curResist);
    }
    public static int GetTalismanPointsByLvl(int lvl)
    {
        return lvl * 10;
    }

    public static int GetPointsByLvl(int lvl)
    {
        return lvl * 7 + 20;
    }

    public static float GetSlotCoef(Slot slot)
    {
        float val = 1f;
        switch (slot)
        {
            case Slot.physical_weapon:
                val = 1.1f;
                break;
            case Slot.magic_weapon:
                val = 1.3f;
                break;
            case Slot.body:
                val = 1f;
                break;
            case Slot.helm:
                val = 0.8f;
                break;
        }
        return val;
    }

    public static float PowerTalicStandart(float p1, float p2, float points,int enchantCount)
    {
        var pointPower = (p2) / Formuls.DiffOfTen();
        return (p1 + points * pointPower) * Formuls.EnchntCoef(enchantCount);
    }

    public static float EnchntCoef(int enchantCount)
    {
        return 1 + 0.2f*enchantCount;
    }

    public static float DiffOfTen()
    {
        var points1 = Formuls.GetTalismanPointsByLvl(1);
        var points10 = Formuls.GetTalismanPointsByLvl(10);
        return points10 - points1;
    }

}

