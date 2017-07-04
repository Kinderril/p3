using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ManyRandomShotsBullets : Weapon
{
    public int countBullets = 1;
    public float rndPower = 2.5f;
    private Vector3 outPosVector3;
    private float distDown = 10f;
    public override void DoShoot(Vector3 v, float additionalPower = 0, Unit target = null)
    {
        outPosVector3 = GetStartPos();
//        var dir = v;

//        Bullet bullet1 = InstantiateBullet();
//        bullet1.transform.position = outPosVector3 + dir.normalized;
//        bullet1.Init(v, this);

        if (pSystemOnShot != null)
        {
            pSystemOnShot.Play();
        }
        DoSIdeBullets(v, rndPower);
    }

    private void DoSIdeBullets(Vector3 dir ,float rnd)
    {
        dir.Normalize();
        for (int i = 0; i < countBullets; i++)
        {
            var xx =  SMUtils.Range(-rnd, rnd);
            var zz =  SMUtils.Range(-rnd, rnd);
            var start = outPosVector3 + dir*Range;
            start.x += xx;
            start.z += zz;

            var modifOutP = start;
            modifOutP.y += distDown;

            var direction = start - modifOutP;

            Bullet bullet1 = InstantiateBullet();
            bullet1.transform.position = modifOutP;
            bullet1.Init(direction, this);
        }
    }

    public override Vector3 FindTrgPosition(Vector3 direction, Vector3 start)
    {
        return start + Vector3.down*distDown*1.2f;
    }

    public override Vector3 FindStartPosition(Bullet bullet)
    {
        var modifOutP = bullet.transform.position;
//        modifOutP.y += 10f;
        return modifOutP;
    }
}

