using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class HeroBornPosition : BaseBornPosition
{
    private bool isPositionOpend;

    public void Init(Map map,bool isOpen)
    {
        base.Init(map);
        isPositionOpend = isOpen;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPositionOpend)
        {
            var unit = other.GetComponent<Hero>();
            if (unit != null)
            {
                MainController.Instance.PlayerData.OpenBornPosition(ID);
                MainController.Instance.level.MessageAppear( "born point opened", Color.blue, DataBaseController.Instance.ItemIcon(ItemId.crystal));
                isPositionOpend = true;
            }
        }
    }
    public override BornPositionType GetBornPositionType()
    {
        return BornPositionType.hero;
    }
}

