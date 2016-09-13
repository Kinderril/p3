using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class HeroBornPosition : BaseBornPosition
{
    private bool isPositionOpend;
    public GameObject OpenGameObject;

    public void Init(Map map,bool isOpen)
    {
        base.Init(map);
        isPositionOpend = isOpen;
        if (OpenGameObject != null)
        {
            OpenGameObject.gameObject.SetActive(isPositionOpend);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPositionOpend)
        {
            var unit = other.GetComponent<Hero>();
            if (unit != null)
            {
                var names = DataBaseController.Instance.RespawnPositionsNames[MainController.Instance.level.MissionIndex];
                MainController.Instance.PlayerData.OpenBornPosition(ID);
                MainController.Instance.level.MessageAppear( "Born point opened","Name:" + names[ID], Color.blue, DataBaseController.Instance.ItemIcon(ItemId.crystal));
                isPositionOpend = true;
                if (OpenGameObject != null)
                {
                    OpenGameObject.gameObject.SetActive(isPositionOpend);
                }
            }
        }
    }
    public override BornPositionType GetBornPositionType()
    {
        return BornPositionType.hero;
    }
}

