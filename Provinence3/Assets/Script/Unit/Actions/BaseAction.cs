using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseAction
{
    protected BaseMonster owner;
    protected Action<bool> endCallback;

    public BaseAction(BaseMonster owner,Action<bool> endCallback)
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
        if (owner != null && endCallback != null && !owner.IsDead)
            endCallback(true);
    }

    public virtual void Dispose()
    {
        endCallback = null;
    }

    public virtual void Stop()
    {

    }
}

