﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct ColorUI
{
    public Color color;
    public ItemId type;
}
[Serializable]
public struct EffectVisualsBehaviour
{
    public EffectType type;
    public VisualEffectBehaviour beh;
}
[Serializable]
public struct CostItemsByLevel
{
    public int level;
    public int crystal;
    public int money;
}
public class DataStructs : MonoBehaviour
{
    public ColorUI[] ColorsOfUI;
    public Color CraftItemColor;
    public CostItemsByLevel[] CostItemsByLevel;
    public EffectVisualsBehaviour[] EffectVisualsBehaviours;
    public int[] costParameterByLvl;
    public const int MISSION_LAST_INDEX = 2;
    public List<BaseBonusMapElement> BaseBonusMapElement;
//    public HeroShopBonusItem HeroShopBonusItemPrefab;
//    public HeroShopExecutableItem HeroShopExecutableItemPrefab;
//    public HeroShopRandomItem HeroShopRandomItemPrefab;
    public int GetRespawnPointsCountByMission(int mission)
    {
        switch (mission)
        {
//            case 0:
//                return 1;
            case 1:
                return 1;
            case 2:
                return 4;
        }
        return 0;
    }
}

