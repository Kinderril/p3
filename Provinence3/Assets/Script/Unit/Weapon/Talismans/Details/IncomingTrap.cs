﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class IncomingTrap : LifeTimeTrap
{
    protected float power;
    protected List<BaseMonster> monstersInside = new List<BaseMonster>();
    public Bullet bulletPrefab;
    private TalismanTrapDamage talic;
    void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            if (!monstersInside.Contains(monster))
            {
                monstersInside.Add(monster);
                var bullet = DataBaseController.GetItem<Bullet>(bulletPrefab);
                bullet.Init(monster, talic,transform.position );
            }
        }
    }
    public void Init(float power, TalismanTrapDamage talic)
    {
        base.Init();
        this.talic = talic;
        this.power = power;
    }
}

