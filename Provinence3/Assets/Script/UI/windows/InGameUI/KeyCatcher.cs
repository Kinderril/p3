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
        callbackClose();
    }
    public void StartAnimationKey()
    {
        callbackOpen();
    }
}

