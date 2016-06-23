using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Trap : MonoBehaviour
{
    protected float power;
    public const float WAIT_FOR_EXPLOSION = 1.7f;
    protected List<BaseMonster> monstersInside = new List<BaseMonster>();
    bool isActivated;

    void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            Do(monster);
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

    public void Init(float power)
    {
        this.power = power;

    }

    private void Do(BaseMonster monster)
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(WaitForAction());
        }
    }

    protected IEnumerator WaitForAction()
    {
        yield return new WaitForSeconds(WAIT_FOR_EXPLOSION);
        DoAction();
        Destroy(gameObject);
    }

    protected virtual void DoAction()
    {

    }
}
