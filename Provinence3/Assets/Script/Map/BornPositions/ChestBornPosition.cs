using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ChestBornPosition : BaseBornPosition
{
    private bool withCrystal;
    public void Init(Map map,Level level)
    {
        base.Init(map);
        BornChest(level);
    }
    public override BornPositionType GetBornPositionType()
    {
        return BornPositionType.chest;
    }

    public void BornChest(Level level)
    {
        var p = transform.position;
        var b = new Vector3(p.x + UnityEngine.Random.Range(-radius, radius), p.y, p.z + UnityEngine.Random.Range(-radius, radius));
        var chest = DataBaseController.GetItem<Chest>(DataBaseController.Instance.chestPrefab, b);
        chest.Init(withCrystal, level.difficult);
        chest.transform.SetParent(map.miscContainer,true);
    }

    public void SetCrystal()
    {
        withCrystal = true;
    }
}

