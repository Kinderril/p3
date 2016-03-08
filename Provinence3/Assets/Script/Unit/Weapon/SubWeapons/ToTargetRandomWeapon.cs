using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ToTargetRandomWeapon : Weapon
{
    public int countBullets;
    public int maxRad;
    public int minRad;
    public override void DoShoot(Vector3 dir, float additionalPower = 0, Unit target = null)
    {
        for (int i = 0; i < countBullets; i++)
        {
            Bullet bullet1 = InstantiateBullet();
            var hero = MainController.Instance.level.MainHero;
            var heroPos = hero.transform.position;
            var dX = UnityEngine.Random.Range(-1f, 1f);
            var dY = UnityEngine.Random.Range(-1f, 1f);
            var dirRandom = (new Vector3(dX,0,dY)).normalized;
            var pos1 = heroPos + dirRandom * UnityEngine.Random.Range(minRad, maxRad);
            bullet1.Init(hero, this, pos1);
        }
        if (pSystemOnShot != null)
        {
            pSystemOnShot.Play();
        }
    }
}

