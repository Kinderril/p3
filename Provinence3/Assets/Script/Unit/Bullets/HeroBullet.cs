using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class HeroBullet : Bullet
{
    protected override void OnBulletHit(Collider other)
    {
        var trg = other.GetComponent<BaseMonster>();
        if (trg != null)
        {
            Hit(trg);
        }
    }
}

