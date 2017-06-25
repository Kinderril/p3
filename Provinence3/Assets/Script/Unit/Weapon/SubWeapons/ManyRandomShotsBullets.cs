using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ManyRandomShotsBullets : Weapon
{
    public int countBullets = 1;
    private Vector3 outPosVector3;
    public override void DoShoot(Vector3 v, float additionalPower = 0, Unit target = null)
    {
        outPosVector3 = GetStartPos();
        var dir = v;

        Bullet bullet1 = InstantiateBullet();
        bullet1.transform.position = outPosVector3 + dir.normalized;
        bullet1.Init(v, this);

        if (pSystemOnShot != null)
        {
            pSystemOnShot.Play();
        }
        DoSIdeBullets(dir, 1f);
    }

    private void DoSIdeBullets(Vector3 dir ,float rnd)
    {

        for (int i = 0; i < countBullets; i++)
        {
            var xx = Range  + SMUtils.Range(-rnd, rnd);
            var zz = Range  + SMUtils.Range(-rnd, rnd);

            var direction = new Vector3(xx, outPosVector3.y, zz) - outPosVector3;

            Bullet bullet1 = InstantiateBullet();
            bullet1.transform.position = outPosVector3;
            bullet1.Init(direction, this);
        }
    }
}

