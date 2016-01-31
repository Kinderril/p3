using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;


public class IEndEffect
{
    public event Action GetEndAction;

    public void Do()
    {
        if (GetEndAction != null)
        {
            GetEndAction();
        }
    }
}

