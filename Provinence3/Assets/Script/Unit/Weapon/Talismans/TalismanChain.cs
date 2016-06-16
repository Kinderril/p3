using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanChain : Talisman
{
    public const string WAY_CHAIN_BULLET = "";
    private ChainBullet cacheGameObject;

    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 4f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 3.8f;

    private int targetsCount = 3;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);

        var pointPower = (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = sourseItem.points * pointPower * EnchntCoef();

        targetsCount += 0;//TODO calc
    }

    public TalismanChain() 
    {
        cacheGameObject = Resources.Load(WAY_CHAIN_BULLET, typeof (ChainBullet)) as ChainBullet;
    }
    public override void Use()
    {
        var bullet = DataBaseController.GetItem<ChainBullet>(cacheGameObject);
        bullet.Init(this,hero, targetsCount);
        base.Use();
    }

    public float Power
    {
        get { return power; }
    }

}

