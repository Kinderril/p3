using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TriplsShotWeapon : Weapon
{
    public int sideCount = 1;
    private Vector3 outPosVector3;
    public override void DoShoot(Vector3 v, Unit target = null)
    {
        outPosVector3 = GetStartPos();
        var dir = v - outPosVector3;

        Bullet bullet1 = Instantiate(bullet.gameObject).GetComponent<Bullet>();
        bullet1.transform.position = outPosVector3 + dir.normalized;
        bullet1.Init(v, this);

        if (pSystemOnShot != null)
        {
            pSystemOnShot.Play();
        }
        DoSIdeBullets(dir, 1);
        DoSIdeBullets(dir, -1);
    }

    private void DoSIdeBullets(Vector3 dir ,int side)
    {

        for (int i = 0; i < sideCount; i++)
        {
            var anglePlus = 20 * (i+1) * Mathf.Deg2Rad * side;
            var cosAP = Mathf.Cos(anglePlus);
            var sinAP = Mathf.Sin(anglePlus);
            float vx = dir.x * cosAP - dir.z * sinAP;
            float vz = dir.x * sinAP + dir.z * cosAP;
            var v = new Vector3(vx,dir.y,vz);
            Bullet bullet1 = Instantiate(bullet.gameObject).GetComponent<Bullet>();
            bullet1.transform.position = outPosVector3 + v.normalized;
            bullet1.Init(v + outPosVector3, this);
        }
    }
}

