using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ToTargetWeapon : Weapon
{
    public override void DoShoot(Vector3 dir, float additionalPower = 0, Unit target = null)
    {
        Bullet bullet1 = Instantiate(bullet.gameObject).GetComponent<Bullet>();
        var hero = MainController.Instance.level.MainHero;
        bullet1.Init(hero, this, hero.transform.position);
        if (pSystemOnShot != null)
        {
            pSystemOnShot.Play();
        }
    }
}

