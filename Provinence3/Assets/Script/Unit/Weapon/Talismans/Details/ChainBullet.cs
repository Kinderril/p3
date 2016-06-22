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
    public BaseEffectAbsorber hitEffect;
    private float power;
    public int maxTargets = 3;
    

    public void Init(TalismanChain sourseItem, Hero hero,int targetsCount)
    {
        power = sourseItem.Power;
        transform.position = hero.transform.position;
        maxTargets = targetsCount;
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
        yield return new WaitForSeconds(0.8f);
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
        transform.position = target.transform.position;
        if (hitEffect != null)
        {
            hitEffect.Play();
        }
        target.GetHit(power,WeaponType.magic);
        affectedList.Add(target);
        monstersInside.Remove(target);
        if (affectedList.Count > maxTargets)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(WaitForAction());
    }
}

