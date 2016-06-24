using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Formuls
{
    public const float BASE_MOSTER_ENERGY = 5;
    public static float calcResist(float curResist)
    {
        return 1 - curResist / (150 + curResist);
    }

    private const int av_mosters_kills = 45;
    private const int av_chestes = 13;
    private const int base_gold_chest = 109;
    private const int base_monster_gold = 23;

    public static int LevelGoldAv(int lvl)
    {
        return av_mosters_kills* GoldInMonster(lvl) + av_chestes*GoldInChest(lvl);
    }

    public static int ChestItemCost(int lvl)
    {
        return (int) (2*Mathf.Pow(lvl, 0.37f)*LevelGoldAv(lvl));
    }
    public static int RecepiCost(int lvl)
    {
        return (int)(ChestItemCost(lvl) * 0.65f);
    }
    public static int BonusCost(int lvl)
    {
        return (int)(ChestItemCost(lvl) * 0.35f);
    }
    public static int ExecutableCost(int lvl)
    {
        return (int)(ChestItemCost(lvl) * 0.4f);
    }

    public static int GoldInChest(int lvl)
    {
        //return (int)(base_gold_chest*(1 + 0.1f*lvl)); 
        return (int) (Mathf.Pow(lvl, 0.6f)* base_gold_chest);
    }

    public static int GoldInMonster(int lvl,float coef = 1f)
    {
        return (int) (lvl*base_monster_gold*coef);
    }

    public static int GetTalismanPointsByLvl(int lvl)
    {
        return lvl * 10;
    }

    public static int GetPlayerItemPointsByLvl(int lvl)
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

    public static float AffectMainParam(float curVal ,Dictionary<MainParam, int> MainParameters,ParamType type)
    {
        switch (type)
        {
            case ParamType.Speed:
                curVal += 4;
                break;
            case ParamType.MPower:
                curVal += MainParameters[MainParam.ATTACK] * 8 + 12;
                break;
            case ParamType.PPower:
                curVal += MainParameters[MainParam.ATTACK] * 9 + 26;
                break;
            case ParamType.PDef:
                curVal += MainParameters[MainParam.DEF] * 6 + 20;
                break;
            case ParamType.MDef:
                curVal += MainParameters[MainParam.DEF] * 5 + 10;
                break;
            case ParamType.Heath:
                curVal += MainParameters[MainParam.HP] * 40 + 300;//200
#if UNITY_EDITOR
                if (DebugController.Instance.MAIN_HERO_MEGAHP)
                {
                    curVal += 999999;
                }
#endif
                break;
        }
        return curVal;
    }

    public static float DiffOfTen()
    {
        var points1 = Formuls.GetTalismanPointsByLvl(1);
        var points10 = Formuls.GetTalismanPointsByLvl(10);
        return points10 - points1;
    }

}

