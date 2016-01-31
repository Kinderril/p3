using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MoveAction : BaseAction
{
    private Vector3 trg;
    private Vector3 dir;

    public MoveAction(BaseMonster owner, Vector3 trg,Action callback)
        : base(owner, callback)
    {
        this.trg = new Vector3(trg.x, owner.transform.position.y, trg.z);
        //GameObject.Instantiate(DataBaseController.Instance.debugCube, this.trg, Quaternion.identity);
        owner.Control.MoveTo(trg);
    }
    public override void Update()
    {
        dir = trg - owner.transform.position;
        var sqrM = dir.sqrMagnitude;
    //    Debug.DrawRay(owner.transform.position, Direction, Color.yellow, 5);
  //      Debug.Log("Direction " + sqrM + "  " + trg + "   " + owner.transform.position);
        if (sqrM < 1f)
        {
            //owner.Control.MoveToPoint(Vector3.zero, false, false);
            End("Direction " + sqrM);
        }
    }

}

