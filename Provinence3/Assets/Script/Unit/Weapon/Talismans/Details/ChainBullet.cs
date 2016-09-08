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
    private TimerManager.ITimer timer;
    

    public void Init(TalismanChain sourseItem, Hero hero,int targetsCount)
    {
        power = sourseItem.Power;
        transform.position = hero.transform.position;
        if (hitEffect != null)
        {
            hitEffect.Stop();
        }
        maxTargets = targetsCount;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromMilliseconds(800), true);
        timer.OnTimer += OnTimer;
        StartCoroutine(Wait1Frame());
    }
    protected IEnumerator Wait1Frame()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        DoAction();
    }

    void OnDestroy()
    {
        if (timer != null)
        {
            timer.Stop();
        }
    }

    private void OnTimer()
    {
        DoAction();
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
    
    protected IEnumerator WaitForStop()
    {
        yield return new WaitForSeconds(0.5f);
        if (hitEffect != null)
        {
            hitEffect.Stop();
        }
    }

    protected void DoAction()
    {
        var target = monstersInside.RandomElement();
//        Debug.Log("monstersInside:" + monstersInside.Count);
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.SetParent(target.transform,false);
        transform.localPosition = Vector3.zero;
//        Debug.Log("Do hit");
        if (hitEffect != null)
        {
            hitEffect.Play();
            StartCoroutine(WaitForStop());
        }
        target.GetHit(power,WeaponType.magic, new DeathInfo(power, WeaponType.magic, SourceType.talisman));
        affectedList.Add(target);
        monstersInside.Remove(target);
        if (affectedList.Count > maxTargets)
        {
            Destroy(gameObject);
        }
//        else
//        {
//            StartCoroutine(WaitForAction());
//        }
    }
}

