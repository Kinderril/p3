using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaitingBullet : Bullet
{
    public float waitTime = 1f;
    public override void Init(Unit target, IBulletHolder weapon,Vector3 startPosition)
    {
        start = startPosition;
        this.weapon = weapon;
        subInit();
        updateAction = updateWaitBullet;
        time = Time.time + waitTime;
    }

    private void updateWaitBullet()
    {
        if (time > Time.time)
        {
            TryHitTragetUnitAndDEath();
        }
    }
}

