using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AgentControl : BaseControl
{
    UnityEngine.AI.NavMeshAgent agent;

    protected override void Init()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        base.Init();
    }

    public override bool MoveTo(Vector3 v)
    {
        var vv = (new Vector3(v.x, transform.position.y, v.z) - transform.position).normalized;
        SetToDirection(vv);
        if (agent.isOnNavMesh)
        {
            var movingOk = agent.SetDestination(v);
            //Debug.Log("AGENT control move to:" + v + " from:" + transform.position + "   movingOk:" + movingOk);
            return movingOk;
        }
        else
        {
            Debug.LogError("Problems with: " + name);
        }
        return false;
    }
    public override void SetToDirection(Vector3 dir)
    {
        targetDirection = dir;
        ThisByQuaterhnion.SetLookDir(targetDirection);
    }
    
    public override bool IsPathComplete()
    {
        var isComplete = agent.remainingDistance < 1;
        return isComplete;
    }

    protected override void UpdateCharacter()
    {
        UpdateAnimator(agent.velocity);
    }
    public override void SetSpeed(float speed)
    {

        if (agent != null)
            agent.speed = speed;
    }


    public override void Stop(bool setSpeedToZero = true)
    {
        if (agent != null)
        {
            if (setSpeedToZero)
            {
                agent.Stop();
                agent.speed = 0;
            }
            else
            {
                agent.ResetPath();
            }
        }

    }
}

