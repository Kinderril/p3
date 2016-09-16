using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Formuls
{
    public const float BASE_MOSTER_ENERGY = 5;
    public const int SpeedCoef = 100;
    public static float calcResist(float curResist)
    {
        return 1 - curResist / (150 + curResist);
    }

    public const int av_mosters_kills = 45;
    private const int av_chestes = 13;
    private const int base_gold_chest = 109;
    private const int base_monster_gold = 23;

    public static int LevelGoldAv(float lvl)
    {
        return av_mosters_kills* GoldInMonster(lvl) + av_chestes*GoldInChest(lvl);
    }

    public static int OpenLevelCost(int index)
    {
        return 15 + index;
    }

    public static int LevelUpCost(int index)
    {
        return (1 + index) * LevelGoldAv(index) / 2;
    }

    public static float ModifyBossHP(BossUnit boss,int bonuses)
    {
        return boss.CurHp * (1 - 0.03f * bonuses);
    }
    public static int ShopPlayerItemCost(float lvl)
    {
        return (int)(2 * Mathf.Pow(lvl, 0.37f) * LevelGoldAv(lvl));
    }
    public static int ShopPlayerItemCost(int lvl)
    {
        return ShopPlayerItemCost((float)lvl);
    }
    public static int RecepiCost(int lvl)
    {
        return (int)(ShopPlayerItemCost(lvl) * 0.65f);
    }
    public static int BonusCost(int lvl)
    {
        return (int)(ShopPlayerItemCost(lvl) * 0.35f);
    }
    public static int ExecutableCost(int lvl)
    {
        return (int)(ShopPlayerItemCost(lvl) * 0.4f);
    }
    
    public static int GoldInChest(int lvl)
    {
        return GoldInChest((float)lvl);
    }
    public static int GoldInChest(float lvl)
    {
        return (int)(Mathf.Pow(lvl, 0.6f) * base_gold_chest);
    }

    public static int CostByExecutableType(ExecutableType type)
    {
        switch (type)
        {
            case ExecutableType.craft:
                return 1;
            case ExecutableType.enchant:
                return 1000;
            case ExecutableType.catalys:
                return 1;
            default:
                return 1;
        }
    }

    public static int CostCraftItemType(CraftItemType ctrType)
    {
        switch (ctrType)
        {
            case CraftItemType.Iron:
                return 45;
            case CraftItemType.Wood:
                return 23;
            case CraftItemType.Leather:
                return 56;
            case CraftItemType.Thread:
                return 28;
            case CraftItemType.Bone:
                return 38;
            case CraftItemType.Mercury:
                return 70;
            case CraftItemType.Gems:
                return 90;
            case CraftItemType.Silver:
                return 87;
            case CraftItemType.Splinter:
                return 102;
        }
        return 1;
    }

    public static int CostBonus(Bonustype Bonustype, int lvl)
    {
        return 500*lvl;
    }
    public static int CostCatalys(CatalysItemType t)
    {
        return 900;
    }
    public static int CostEnchant(EnchantType t,int lvl)
    {
        switch (t)
        {
            case EnchantType.weaponUpdate:
                return 1600;
            case EnchantType.powerUpdate:
                return 1100;
            case EnchantType.armorUpdate:
                return 1400;
//            case EnchantType.healthUpdate:
//                return 2600;
        }
        return 900;
    }

    public static int PlayerItemCost(float points, Rarity isRare)
    {
        float c = 1;
        switch (isRare)
        {
            case Rarity.Magic:
                c = 1.4f;
                break;
            case Rarity.Rare:
                c = 1.9f;
                break;
            case Rarity.Uniq:
                c = 3f;
                break;
        }
        var lvlCost = GetPlayerItemLvlByPoints(points) ;
        var p = ShopPlayerItemCost(lvlCost);
        var totalCost = (int)(p * c / 4);
        Debug.Log("points: " + points + "  totalCost:"+ totalCost + "  lvlCost:" + lvlCost);
        return totalCost;
    }

    public static int GoldInMonster(int lvl,float coef = 1f)
    {
        return (int)GoldInMonster((float)lvl,coef);
    }
    public static int GoldInMonster(float lvl, float coef = 1f)
    {
        return (int)(lvl * base_monster_gold * coef);
    }

    private const int BonusCoef = 2;
    public static int GetBonusPointsByLvl(int lvl)
    {
        return lvl * BonusCoef;
    }

    private const int TalismanCoef = 10;
    public static int GetTalismanPointsByLvl(int lvl)
    {
        return lvl * TalismanCoef;
    }

    public static int CostTalismanBypoints(int points)
    {
        var lvl = (int)((float)points/ (float)TalismanCoef);
        return ExecutableCost(lvl)/4;
    }

    private static float GetPlayerItemLvlByPoints(float points)
    {
        return (points - 20f)/7f;
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

    public const int PATTACK_COEF = 8;
    public const int MATTACK_COEF = 9;
    public const int PDEF_COEF = 6;
    public const int MDEF_COEF = 5;
    public const int HP_COEF = 40;

    public static float AffectMainParam(float curVal ,Dictionary<MainParam, int> MainParameters,ParamType type)
    {
        switch (type)
        {
            case ParamType.Speed:
                curVal += 400;
                break;
            case ParamType.MPower:
                curVal += MainParameters[MainParam.ATTACK] * PATTACK_COEF + 12;
                break;
            case ParamType.PPower:
                curVal += MainParameters[MainParam.ATTACK] * MATTACK_COEF + 26;
                break;
            case ParamType.PDef:
                curVal += MainParameters[MainParam.DEF] * PDEF_COEF + 20;
                break;
            case ParamType.MDef:
                curVal += MainParameters[MainParam.DEF] * MDEF_COEF + 10;
                break;
            case ParamType.Heath:
                curVal += MainParameters[MainParam.HP] * HP_COEF + 300;//200
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

    public static QuestDifficulty RandomQuestDifficulty()
    {
        WDictionary<QuestDifficulty> w = new WDictionary<QuestDifficulty>(new Dictionary<QuestDifficulty, float>()
        {
            {QuestDifficulty.easy,3},{QuestDifficulty.normal,4},{QuestDifficulty.hard,3},
        });
        return w.Random();
    }
    public static GiftType CalcGiftType(QuestDifficulty difficulty)
    {
        Dictionary<GiftType, float> d;
        switch (difficulty)
        {
            case QuestDifficulty.easy:
                d = new Dictionary<GiftType, float>()
                {
                    {GiftType.catalys, 9},
                    {GiftType.recepi, 8},
                    {GiftType.item, 2},
                    {GiftType.enchant, 8},
                    {GiftType.bonus, 4},
                };
                break;
            case QuestDifficulty.hard:
                d = new Dictionary<GiftType, float>()
                {
                    {GiftType.catalys, 7},
                    {GiftType.recepi, 6},
                    {GiftType.item, 2},
                    {GiftType.enchant, 9},
                    {GiftType.bonus, 5},
                };
                break;
            default:
                d = new Dictionary<GiftType, float>()
                {
                    {GiftType.catalys, 3},
                    {GiftType.recepi, 2},
                    {GiftType.item, 2},
                    {GiftType.enchant, 4},
                    {GiftType.bonus, 3},
                };
                break;
        }
        WDictionary<GiftType> gifts = new WDictionary<GiftType>(d);
        return gifts.Random();
    }

    public static QuestRewardType RandomQuestReward(QuestDifficulty d)
    {
        WDictionary<QuestRewardType> w = null;
        switch (d)
        {
            case QuestDifficulty.easy:
                w = new WDictionary<QuestRewardType>(new Dictionary<QuestRewardType, float>()
                {
                    { QuestRewardType.money, 70},{ QuestRewardType.materials, 40},{ QuestRewardType.crystal, 15},{ QuestRewardType.item, 5}
                });
                break;
            case QuestDifficulty.normal:
                w = new WDictionary<QuestRewardType>(new Dictionary<QuestRewardType, float>()
                {
                    { QuestRewardType.money, 70},{ QuestRewardType.materials, 50},{ QuestRewardType.crystal, 25},{ QuestRewardType.item, 10}
                });
                break;
            case QuestDifficulty.hard:
                w = new WDictionary<QuestRewardType>(new Dictionary<QuestRewardType, float>()
                {
                    { QuestRewardType.money, 50},{ QuestRewardType.materials, 30},{ QuestRewardType.crystal, 25},{ QuestRewardType.item, 15}
                });
                break;
            default:
                return QuestRewardType.money;
        }
        return w.Random();
    }

}

