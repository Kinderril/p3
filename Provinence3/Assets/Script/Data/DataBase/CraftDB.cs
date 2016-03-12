using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CraftDB
{
    Dictionary<Slot,List<CraftItemType>> crafts = new Dictionary<Slot, List<CraftItemType>>(); 
    public CraftDB()
    {
        crafts.Add(Slot.physical_weapon, new List<CraftItemType>()
        {
            CraftItemType.Leather,
            CraftItemType.Coal,
            CraftItemType.Oil,
            CraftItemType.Thread,
        });
        crafts.Add(Slot.magic_weapon, new List<CraftItemType>()
        {
            CraftItemType.Iron,
            CraftItemType.Hardner,
            CraftItemType.Silver,
            CraftItemType.Thread,
        });
        crafts.Add(Slot.helm, new List<CraftItemType>()
        {
            CraftItemType.Leather,
            CraftItemType.Coal,
            CraftItemType.Oil,
            CraftItemType.Silver,
        });
        crafts.Add(Slot.body, new List<CraftItemType>()
        {
            CraftItemType.Iron,
            CraftItemType.Hardner,
            CraftItemType.Stem,
            CraftItemType.Bone,
        });
    }

    public List<ExecCraftItem> GetRecipe(Slot s,int l)
    {
        List<ExecCraftItem> list = new List<ExecCraftItem>();
        
        foreach (var craftItemType in crafts[s])
        {
            int count = 0;
            switch (craftItemType)
            {
                case CraftItemType.Hardner:
                case CraftItemType.Iron:
                case CraftItemType.Leather:
                case CraftItemType.Coal:
                    count = (int)(l*1.5f);
                    break;
                case CraftItemType.Bone:
                case CraftItemType.Thread:
                case CraftItemType.Stem:
                case CraftItemType.Oil:
                case CraftItemType.Silver:
                    count = (int)(Mathf.Clamp(4-l,0,Int32.MaxValue) + 5);
                    break;
                case CraftItemType.Splinter:
                    count = l;
                    break;
            }
            list.Add(new ExecCraftItem(craftItemType,count));
        }
        return list;
    }
}

