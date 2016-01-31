using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseAction
{
    protected BaseMonster owner;
    protected Action endCallback;

    public BaseAction(BaseMonster owner,Action endCallback)
    {
        this.endCallback = endCallback;
        this.owner = owner;
    }
    public virtual void Update()
    {
        
    }

    public virtual void End(string msg = " end action ")
    {
        //Debug.Log(msg);
        if (endCallback != null && !owner.IsDead)
            endCallback();
    }

    public virtual void Dispose()
    {
        endCallback = null;
    }
}

