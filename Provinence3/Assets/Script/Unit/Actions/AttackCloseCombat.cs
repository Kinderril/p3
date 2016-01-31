using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AttackCloseCombat :AttackAction
{
    public AttackCloseCombat(BaseMonster owner, Unit target, Action endCallback) 
        : base(owner, target, endCallback)
    {
    }

    public override void Update()
    {
        base.Update();
        UpdateCloseCombat();
    }
    public void UpdateCloseCombat()
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
                    DoShoot();
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

