using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ChainBullet : MonoBehaviour
{
    protected List<BaseMonster> monstersInside = new List<BaseMonster>();
    protected List<BaseMonster> affectedList = new List<BaseMonster>();
    private float power;
    public int maxTargets = 8;

    public void Init(TalismanItem sourseItem, Hero hero)
    {
        power = sourseItem.power;
        StartCoroutine(WaitForAction());
}

    void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            if (!affectedList.Contains(monster))
                monstersInside.Add(monster);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            monstersInside.Remove(monster);
        }
    }
    
    protected IEnumerator WaitForAction()
    {
        yield return new WaitForSeconds(0.5f);
        DoAction();
    }

    protected void DoAction()
    {
        var target = monstersInside.RandomElement();
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        target.GetHit(power,WeaponType.magic);
        affectedList.Add(target);
        monstersInside.Remove(target);
        if (affectedList.Count >= maxTargets)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(WaitForAction());
    }
}

