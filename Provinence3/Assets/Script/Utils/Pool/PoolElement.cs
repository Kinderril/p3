using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PoolElement : MonoBehaviour
{
    protected bool isUsing;

    public bool IsUsing
    {
        get { return isUsing; }
    }

    public virtual void Init()
    {
        isUsing = true;
        gameObject.SetActive(IsUsing);
    }

    public void EndUse()
    {
        isUsing = false;
        gameObject.SetActive(IsUsing);
        
    }
}

