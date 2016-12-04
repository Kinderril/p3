using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class KeyCatcher : MonoBehaviour
{
    private Action callbackClose;
    private Action callbackOpen;
    public void Init(Action callbackClose,Action callbackOpen)
    {
        this.callbackClose = callbackClose;
        this.callbackOpen = callbackOpen;
    }
    public void EndAnimationKey()
    {
        if (callbackClose != null)
        {
            callbackClose();
        }
    }
    public void StartAnimationKey()
    {
        if (callbackOpen != null)
        {
            callbackOpen();
        }
    }
}

