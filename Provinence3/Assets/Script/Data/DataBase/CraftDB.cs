using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CraftItemCoefs
{
    public CraftItemType CraftItemType;
    public int def;
    public float coef;
    public int start;

    public CraftItemCoefs(CraftItemType CraftItemType, int def, float coef, int start)
    {
        this.CraftItemType = CraftItemType;
        this.def = def;
        this.coef = coef;
        this.start = start;
    }
    public int Get(int lvl)
    {
        if (lvl < start)
            return 0;

        return (int)((lvl - start)*coef) + def;
    }
}

public class CraftDB
{
    Dictionary<Slot,List<CraftItemCoefs>> crafts = new Dictionary<Slot, List<CraftItemCoefs>>(); 
     
    public CraftDB()
    {
        crafts.Add(Slot.physical_weapon, new List<CraftItemCoefs>()
        {
            new CraftItemCoefs(CraftItemType.Iron, 18,7,0),
            new CraftItemCoefs(CraftItemType.Wood, 13,5,0),
            new CraftItemCoefs(CraftItemType.Bone, 4,3,5),
            new CraftItemCoefs(CraftItemType.Mercury, 3,2.5f,7),
        });
        crafts.Add(Slot.magic_weapon, new List<CraftItemCoefs>()
        {
            new CraftItemCoefs(CraftItemType.Wood, 21,8,0),
            new CraftItemCoefs(CraftItemType.Thread, 5,3,0),
            new CraftItemCoefs(CraftItemType.Silver, 3, 3.5f ,6),
            new CraftItemCoefs(CraftItemType.Gems, 3, 2.3f ,8),
        });
        crafts.Add(Slot.body, new List<CraftItemCoefs>()
        {
            new CraftItemCoefs(CraftItemType.Iron, 16,6,0),
            new CraftItemCoefs(CraftItemType.Leather, 12,5,0),
            new CraftItemCoefs(CraftItemType.Gems, 5, 1.9f ,5),
            new CraftItemCoefs(CraftItemType.Mercury, 2, 3.2f ,8),
        });
        crafts.Add(Slot.helm, new List<CraftItemCoefs>()
        {
            new CraftItemCoefs(CraftItemType.Thread, 8,5,0),
            new CraftItemCoefs(CraftItemType.Leather, 18,7,0),
            new CraftItemCoefs(CraftItemType.Gems, 4, 2.8f ,6),
            new CraftItemCoefs(CraftItemType.Mercury, 2, 1.9f ,7),
        });
        crafts.Add(Slot.Talisman, new List<CraftItemCoefs>()
        {
            new CraftItemCoefs(CraftItemType.Thread, 8,5,0),//todo
            new CraftItemCoefs(CraftItemType.Leather, 18,7,0),//todo
            new CraftItemCoefs(CraftItemType.Gems, 4, 2.8f ,6),//todo
            new CraftItemCoefs(CraftItemType.Mercury, 2, 1.9f ,7),//todo
        });
        
    }

    public List<ExecCraftItem> GetRecipe(Slot s,int l)
    {
        List<ExecCraftItem> list = new List<ExecCraftItem>();
        
        foreach (var craftItemType in crafts[s])
        {
            list.Add(new ExecCraftItem(craftItemType.CraftItemType, craftItemType.Get(l)));
        }
        return list;
    }
}

