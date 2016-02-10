using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaitingBullet : Bullet
{
    public float waitTime = 1f;
    private List<Unit> unitsInside = new List<Unit>(); 
    public override void Init(Unit target, IBulletHolder weapon,Vector3 startPosition)
    {

        start = startPosition;
        transform.position = start;
        this.weapon = weapon;
        subInit();
        updateAction = updateWaitBullet;
        time = Time.time + waitTime;
    }

    protected override void OnBulletHit(Collider other)
    {
        var unit = other.GetComponent<Unit>();
        if (unit != null && !unitsInside.Contains(unit))
        {
            unitsInside.Add(unit);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            unitsInside.Remove(unit);
        }
    }

    private void updateWaitBullet()
    {
        if (time < Time.time)
        {
            foreach (var unit in unitsInside)
            {
                if (unit != null && !unit.IsDead)
                {
                    if ((weapon.Owner is Hero && unit is BaseMonster) 
                        || (weapon.Owner is BaseMonster && unit is Hero))
                    {
                        unit.GetHit(this);
                    }
                }
            }
            Death();
        }
    }
}

