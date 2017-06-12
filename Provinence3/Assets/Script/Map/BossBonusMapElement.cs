using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BossBonusMapElement : MapObjectWithDeath
{
    private bool isActive = true;
    public PSAbsorber effect;
    private BossSpawner bossSpawner;

    public void Init(BossSpawner bossSpawner)
    {
        this.bossSpawner = bossSpawner;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            var heroBullet = other.GetComponent<Bullet>();
            if (heroBullet != null && heroBullet.bulletHolder.Owner is Hero)
            {
                isActive = false;
                if (effect != null)
                {
                    effect.Play();
                }
                bossSpawner.GetBonus();
                SetDeath();
            }
        }
    }
}

