using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MonsterBullet : Bullet
{
    protected override void OnBulletHit(Collider other)
    {
        var trg = other.GetComponent<Hero>();
        if (trg != null)
        {
            Hit(trg);
        }
    }
}

