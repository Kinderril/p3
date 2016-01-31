using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BonusBornPosition : BaseBornPosition
{
    public void Init(Map map, Level level)
    {
        base.Init(map);
        BornBonus(level);
    }
    public override BornPositionType GetBornPositionType()
    {
        return BornPositionType.bonus;
    }

    public void BornBonus(Level level)
    {
        var p = transform.position;
        var b = new Vector3(p.x + UnityEngine.Random.Range(-radius, radius), p.y, p.z + UnityEngine.Random.Range(-radius, radius));
        var bonusMapElement = DataBaseController.GetItem<BaseBonusMapElement>(DataBaseController.Instance.DataStructs.BaseBonusMapElement.RandomElement(), b);
        bonusMapElement.Init();
        bonusMapElement.transform.SetParent(map.miscContainer, true);
    }

}

