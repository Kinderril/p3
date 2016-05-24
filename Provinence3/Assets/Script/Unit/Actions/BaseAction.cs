using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EndCause
{
    no,
    runAway
}

public class BaseAction
{
    protected BaseMonster owner;
    protected Action<EndCause> endCallback;

    public BaseAction(BaseMonster owner,Action<EndCause> endCallback)
    {
        this.endCallback = endCallback;
        this.owner = owner;
    }
    public virtual void Update()
    {
        
    }

    public virtual void End(EndCause cause = EndCause.no, string msg = " end action ")
    {
        //Debug.Log(msg);
        if (owner != null && endCallback != null && !owner.IsDead)
            endCallback(cause);
    }

    public virtual void Dispose()
    {
        endCallback = null;
    }

    public virtual void Stop()
    {

    }
}

