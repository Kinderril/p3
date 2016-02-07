using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class IncomingTrap : MonoBehaviour
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
                bullet.Init(monster, talic,monster.transform.position );
            }
        }
    }
    public void Init(float power, TalismanTrapDamage talic)
    {
        this.talic = talic;
        this.power = power;
    }
}

