using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



public class AttackDistance : AttackAction
{

    public AttackDistance(BaseMonster owner, Unit target, Action endCallback) 
        : base(owner, target, endCallback)
    {
    }

    public override void Update()
    {
        base.Update();
        UpdateDistance();
    }


    public void UpdateDistance()
    {
        TestTargetDist();

    }

    private void TestTargetDist()
    {
        isInRange = (curRangeSqr < rangeAttackSqr);
        if (isInRange)
        {
            if (owner.curWeapon.CanShoot())
            {
                if (attackStatus != AttackStatus.shoot)
                    DoShoot(true);
            }
        }
        else
        {
            if (attackStatus == AttackStatus.none)
            {
                MoveToTarget();
            }
        }
    }

    protected override void TargetMove()
    {
        TestTargetDist();
    }

    protected override void OnShootEnd(Unit obj)
    {
        base.OnShootEnd(obj);
        TestTargetDist();
    }
}