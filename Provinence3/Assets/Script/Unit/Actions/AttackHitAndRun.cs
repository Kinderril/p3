using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AttackHitAndRun : AttackAction
{
    private AttackStatus status;
    private Vector3 backPosition;

    public AttackHitAndRun(BaseMonster owner, Unit target, Action endCallback) : base(owner, target, endCallback)
    {
    }

    public override void Update()
    {
        base.Update();
        UpdateHitAndRun();
//        Debug.Log(this);
    }

    public void UpdateHitAndRun()
    {
//        switch (status)
//        {
//            case AttackStatus.comeIn:
//                curRange = (owner.transform.position - target.transform.position).magnitude;
//                isInRange = (curRange < rangeAttack);
//                if (isInRange)
//                {
//                    DoShoot();
//                    backPosition = ((BaseMonster)owner).bornPosition;
//                    status = AttackStatus.comeOut;
//                }
//                else
//                {
//                    MoveToTarget(target.transform.position);
//                }
//                break;
//            case AttackStatus.comeOut:
//                curRange = (owner.transform.position - backPosition).magnitude;
//                isInRange = (curRange < rangeAttack);
//                if (isInRange)
//                {
//                    status = AttackStatus.comeIn;
//                }
//                else
//                {
//                    MoveToTarget(backPosition);
//                }
//                break;
//        }
    }

    public override string ToString()
    {
        return "H&R: " + status + " Back:" + backPosition + "  isInRange:" + isInRange;
    }
}

