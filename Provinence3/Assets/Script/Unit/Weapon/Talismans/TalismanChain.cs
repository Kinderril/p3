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
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 3.8f - Talisman.LVL_1_AV_MONSTER_HP / 4f;

    private int targetsCount = 3;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);

        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.Enchant);

        targetsCount += 0;//TODO calc
    }
    public override string PowerInfo()
    {
        return "Stretches from the hero to a targeted nearby enemy and chains on to additional " +targetsCount + " targets. Deal " + Power.ToString("0") + " magical damage.";
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

