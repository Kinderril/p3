using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum AttackType
{
    hitAndRun,
    distanceFight,
    closeCombat
}

public enum AttackStatus
{
    none,
    shoot,
    move,
}

public class AttackAction : BaseAction
{
    private const float MAX_OFFSET_SQR = 8.5f;
    private const float RADIUS = 1f;
    private const float DELTA_RECALC = 0.3f;
    protected Unit target;
    private float lastRecalTime;
    protected float rangeAttackSqr;
    protected float curRangeFromBornPosition;
    protected float curRangeSqr;
    protected float lastHitTime;
    protected bool isInRange;
//    protected bool isMoving = false;
    private Vector3 moveTarget;
    private Vector3 lastTargetScanPos;
    protected Vector3 targetUnitDir;
    protected AttackStatus attackStatus = AttackStatus.none;

    public AttackAction(BaseMonster owner, Unit target ,Action endCallback)
        : base(owner, endCallback)
    {
        this.target = target;
        lastTargetScanPos = target.transform.position;
        lastTargetScanPos = new Vector3(lastTargetScanPos.x, owner.transform.position.y, lastTargetScanPos.z);
        rangeAttackSqr = owner.curWeapon.Parameters.range * owner.curWeapon.Parameters.range;
    }


    private void MoveToTarget(Vector3 trg)
    {
        if (lastRecalTime < Time.time + DELTA_RECALC)
        {
            trg = new Vector3(trg.x, owner.transform.position.y, trg.z);
            lastTargetScanPos = trg;

//        bool isTooClose = true;
            trg = ClaclNearTargetPosition(trg);
//        isTooClose = (moveTarget - trg).sqrMagnitude > 1;
//            Debug.Log("Start move to target " + trg);
            if ( /*isTooClose && */owner.Control.MoveTo(trg))
            {
                lastRecalTime = Time.time;
                moveTarget = trg;
                attackStatus = AttackStatus.move;
            }
        }
    }

    private Vector3 ClaclNearTargetPosition(Vector3 trg)
    {
        var ownPos = owner.transform.position;
        var dir = trg - ownPos;
        var dist = dir.magnitude;
        var a = dist - RADIUS;
        return ownPos + dir.normalized*a;
    }

    protected void MoveToTarget()
    {
        MoveToTarget(target.transform.position);
    }

    public override void Update()
    {
        if (attackStatus == AttackStatus.move)
        {
            var isMoving = !owner.Control.IsPathComplete();
            if (!isMoving)
            {
                MoveEnd();
            }
        }
        curRangeFromBornPosition = owner.mainHeroDist;
        if (target != null)
        {
            curRangeSqr = (owner.transform.position - target.transform.position).sqrMagnitude;
            var curp = target.transform.position;
            targetUnitDir = lastTargetScanPos - curp;
            var offset = (targetUnitDir).sqrMagnitude;
//            Debug.Log("Offset test  " + offset);
            if (offset > MAX_OFFSET_SQR)
            {
                //                lastTargetScanPos = curp;
//                Debug.Log("TargetMove By Offset " + offset);
                TargetMove();
            }
        }
    }

    protected virtual void TargetMove()
    {
        
    }

    protected virtual void MoveEnd()
    {
        attackStatus = AttackStatus.none;
     
    }

    protected void DoShoot(bool shallStop = false)
    {
        var dir = target.transform.position - owner.transform.position;
        dir.y = 0;
//        Debug.Log("shhot dir: " + dir);
        owner.Control.SetToDirection(dir);
        //dir = new Vector3(dir.x,owner.transform.position.y,dir.z);
        owner.TryAttack(dir, target);
        owner.OnShootEnd += OnShootEnd;
        attackStatus = AttackStatus.shoot;
        if (shallStop)
        {
            ((AgentControl)owner.Control).Stop(false);
        }
    }

    protected virtual void OnShootEnd(Unit obj)
    {
        attackStatus = AttackStatus.none;
        owner.OnShootEnd -= OnShootEnd;
    }

    public override void End(string msg = " end action ")
    {
        owner.OnShootEnd -= OnShootEnd;
        ((AgentControl)owner.Control).Stop(false);
        base.End(msg);
    }
}

