using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PoolElement : MonoBehaviour
{
    protected bool isUsing;
    private Transform baseParent;

    public bool IsUsing
    {
        get { return isUsing; }
    }

    public void SetBaseParent(Transform baseParent)
    {
        this.baseParent = baseParent;
    }

    public virtual void Init()
    {
        isUsing = true;
        gameObject.SetActive(IsUsing);
    }

    public virtual void EndUse()
    {
        isUsing = false;
        transform.SetParent(baseParent);
        gameObject.SetActive(IsUsing);
        
    }
}

