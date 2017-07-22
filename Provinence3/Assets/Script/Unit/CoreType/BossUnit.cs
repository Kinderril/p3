using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BossUnit : BaseMonster
{
    public string BossName;

    public ArrowTarget Arrow { get; set; }
    protected override void Death()
    {
        base.Death();
        StartCoroutine(Wait4Dead());
    }

    private IEnumerator Wait4Dead()
    {
        yield return new WaitForSeconds(1);
        MainController.Instance.level.PreEndLevel(EndlevelType.normal);

    }

    public void CheckArrow()
    {
        Arrow.UpdateByBoss();
    }

    public void ModificateParams(int difficult)
    {
        if (Parameters != null)
        {
            var delta = (Level - difficult)/10f;
            if (delta <= 1)
            {
                delta = 1f;
            }
            Parameters.SetAbsolute(ParamType.PPower,Parameters[ParamType.PPower] * delta);
            Parameters.SetAbsolute(ParamType.PPower,Parameters[ParamType.MPower] * delta);
            Parameters.SetAbsolute(ParamType.PPower,Parameters[ParamType.MDef] * delta);
            Parameters.SetAbsolute(ParamType.PPower,Parameters[ParamType.PDef] * delta);
          
            Parameters.MaxHp *= delta;
        }

    }

    protected override void DropItems()
    {

    }
}

