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
        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            isActivated = false;
            var dir = target.transform.position - owner.transform.position;
            owner.Control.ThisByQuaterhnion.SetLookDir(dir);
            activateTime = Time.time + UnityEngine.Random.Range(0.4f, 1f);
        }
        else
        {
            isActivated = true;
        }
    }

    public override void Update()
    {
        if (isActivated)
        {
            base.Update();
            UpdateCloseCombat();
        }
        else
        {
            if (Time.time > activateTime)
            {
                isActivated = true;
            }
        }
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

