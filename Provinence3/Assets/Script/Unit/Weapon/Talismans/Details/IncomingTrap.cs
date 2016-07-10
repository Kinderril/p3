using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class IncomingTrap : LifeTimeTrap
{
    protected float power;
    protected Dictionary<BaseMonster,DateTime> monstersInside = new Dictionary<BaseMonster, DateTime>();
    public Bullet bulletPrefab;
    private TalismanTrapDamage talic;
    private TimeSpan deltaSpan = TimeSpan.FromSeconds(2.5);
    public const float LIFE_TIME2 = 1f;
    private TimerManager.ITimer timer2;
    void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            bool canFire = false;
            if (monstersInside.ContainsKey(monster))
            {
                canFire = CanAttackMonster(monster);
            }
            else
            {
                monstersInside.Add(monster, DateTime.Now);
                canFire = true;
            }
            if (canFire)
            {
                Attack(monster);
            }
        }
    }

    private void Attack(BaseMonster monster)
    {
        var bullet = DataBaseController.GetItem<Bullet>(bulletPrefab);
        bullet.Init(monster, talic, transform.position);

    }

    private bool CanAttackMonster(BaseMonster monster)
    {
        var delta = DateTime.Now - monstersInside[monster];
        return (delta > deltaSpan);
    }

    public void Init(float power, TalismanTrapDamage talic)
    {
        base.Init();
        timer2 = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(LIFE_TIME2),true);
        timer2.OnTimer += OnCheck;
        this.talic = talic;
        this.power = power;
    }

    private void OnCheck()
    {
        List<BaseMonster> list = new List<BaseMonster>();
        foreach (var value in monstersInside)
        {
            if (value.Key == null || value.Key.IsDead)
            {
                continue;
            }
            if (CanAttackMonster(value.Key))
            {
                list.Add(value.Key);
                Attack(value.Key);
            }
        }
        foreach (var baseMonster in list)
        {
            monstersInside[baseMonster] = DateTime.Now;
        }
        list = null;
    }

    protected override void subDestroy()
    {
        if (timer2 != null)
        {
            timer2.Stop();
        }
    }
}

