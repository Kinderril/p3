using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BossUnit : BaseMonster
{
    public ArrowTarget Arrow { get; set; }
    protected override void Death()
    {
        base.Death();
        StartCoroutine(Wait4Dead());
    }

    private IEnumerator Wait4Dead()
    {
        yield return new WaitForSeconds(1);
        MainController.Instance.EndLevel(EndlevelType.normal);

    }

    public void CheckArrow()
    {
        Arrow.UpdateByBoss();
    }

}

