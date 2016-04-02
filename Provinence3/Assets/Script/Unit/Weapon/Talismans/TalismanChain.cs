using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanChain : Talisman
{
    public const string WAY_CHAIN_BULLET = "";
    private ChainBullet cacheGameObject;
    public TalismanChain() 
    {
        cacheGameObject = Resources.Load(WAY_CHAIN_BULLET, typeof (ChainBullet)) as ChainBullet;
    }
    public override void Use()
    {
        var bullet = DataBaseController.GetItem<ChainBullet>(cacheGameObject);
        bullet.Init(sourseItem,hero);
        base.Use();
    }
}

