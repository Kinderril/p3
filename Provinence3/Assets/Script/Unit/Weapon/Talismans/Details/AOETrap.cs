using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AOETrap : Trap
{
    public const string WAY_CHAIN_BULLET = "prefabs/talisman/BulletAOETrap";
    public bool isDamage = false;
    private Bullet cacheGameObject;
    private IBulletHolder talic;

    public void Init(float power,bool isDamage, IBulletHolder talic)
    {
        Init(power);
        this.talic = talic;
        cacheGameObject = Resources.Load(WAY_CHAIN_BULLET, typeof(Bullet)) as Bullet;
        this.isDamage = isDamage;
    }
    protected override void DoAction()
    {
        foreach (var monster in monstersInside)
        {
            if (isDamage)
            {
                var bullet = DataBaseController.GetItem<Bullet>(cacheGameObject);
                bullet.Init(monster, talic, monster.transform.position);
            }
            else
            {
                TimeEffect.Creat(monster, EffectType.freez);
            }
        }
    }
}

