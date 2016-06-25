using System;
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
public struct ColorRarity
{
    public Color Color;
    public Rarity Rarity;
}
[Serializable]
public struct ColorParameter
{
    public Color Color;
    public ParamType ParamType;
}
[Serializable]
public struct EffectVisualsBehaviour
{
    public EffectType type;
    public VisualEffectBehaviour beh;
}
[Serializable]
public struct EffectVisualsTalisman
{
    public TalismanType type;
    public BaseEffectAbsorber EffectAbsorber;
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

    public EffectVisualsTalisman[] EffectVisualsTalisman;
    public ColorRarity[] ColorRarity;
    public ColorParameter[] ColorParameter;
    public ColorUI[] ColorsOfUI;
    public Color CraftItemColor;
    public CostItemsByLevel[] CostItemsByLevel;
    public EffectVisualsBehaviour[] EffectVisualsBehaviours;
    public int[] costParameterByLvl;
    public const int MISSION_LAST_INDEX = 2;
    public List<BaseBonusMapElement> BaseBonusMapElement;
    public PrefabsStruct PrefabsStruct;
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

