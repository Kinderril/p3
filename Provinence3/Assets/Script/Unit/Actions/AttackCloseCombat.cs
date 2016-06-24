using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AttackCloseCombat :AttackAction
{
    public AttackCloseCombat(BaseMonster owner, Unit target, Action<EndCause> endCallback,bool byHit) 
        : base(owner, target, endCallback)
    {
        if (!byHit && UnityEngine.Random.Range(0, 100) < 50)
        {
            isActivated = false;
            var dir = target.transform.position - owner.transform.position;
            owner.Control.ThisByQuaterhnion.SetLookDir(dir);
            var waitTime = UnityEngine.Random.Range(1.5f, 3f);
            activateTime = Time.time + waitTime;
//            Debug.Log("tupiiim:" + waitTime);
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
//        Debug.Log("isInRange:" + isInRange);
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

