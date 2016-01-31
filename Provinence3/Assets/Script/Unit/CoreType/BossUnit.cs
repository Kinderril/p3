using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BossUnit : BaseMonster
{
    protected override void Dead()
    {
        base.Dead();
        StartCoroutine(Wait4Dead());
    }

    private IEnumerator Wait4Dead()
    {
        yield return new WaitForSeconds(1);
        MainController.Instance.EndLevel(true);

    } 
}

