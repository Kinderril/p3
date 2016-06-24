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
        transform.SetParent(baseParent,false);
    }

    public virtual void Init()
    {
        isUsing = true;
        if (baseParent == null)
        {
            baseParent = DataBaseController.Instance.transform;
        }
        gameObject.SetActive(IsUsing);
    }

    public virtual void EndUse()
    {
        isUsing = false;
        transform.SetParent(baseParent,false);
        gameObject.SetActive(IsUsing);
        
    }
}

