using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelObject : MonoBehaviour
{
    public const string FOG_START_LEVEL = "_Fog_Start_Level";
    public const string FOG_DIFF = "_Fog_Diff";
    public const string FOG_START_LEVEL_TERRAIN = "_Fog_Start_Level_Terrain";
    public const string FOG_DIFF_TERRAIN = "_Fog_Diff_Terrain";
    public List<DynamicElement> DynamicElements = new List<DynamicElement>();
    public Terrain Terrain;
    public float GlobalFogLevel = 31;
    public float GlobalFogDiff = 1.7f;
    public float PrecentMonsterToBoss_0_1 = 0.25f;
    public event Action<Hero> OnInited;
    public int QuestCount = 5;

    public void Init(Hero hero)
    {
        foreach (var dynamicElement in DynamicElements)
        {
            dynamicElement.Init(hero);
        }
        Shader.SetGlobalFloat(FOG_START_LEVEL, GlobalFogLevel);
        Shader.SetGlobalFloat(FOG_DIFF, GlobalFogDiff);
        Shader.SetGlobalFloat(FOG_START_LEVEL_TERRAIN, GlobalFogLevel);
        Shader.SetGlobalFloat(FOG_DIFF_TERRAIN, GlobalFogDiff * 3);
        if (OnInited != null)
        {
            OnInited(hero);
        }
    }

    public void UpdateByMap()
    {
        foreach (var dynamicElement in DynamicElements)
        {
            dynamicElement.UpdateByMap();
        }
    }
}

