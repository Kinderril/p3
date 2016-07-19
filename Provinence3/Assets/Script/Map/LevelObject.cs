using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelObject : MonoBehaviour
{
    public List<DynamicElement> DynamicElements = new List<DynamicElement>();
    public Terrain Terrain;

    public void Init(Hero hero)
    {
        foreach (var dynamicElement in DynamicElements)
        {
            dynamicElement.Init(hero);
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

