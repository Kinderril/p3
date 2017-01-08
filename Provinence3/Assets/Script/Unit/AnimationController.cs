using System;
using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    private Action action;
    public bool playImmidiatly = false;

    public void EndPlayAttack()//THIS CATCH IN ANIMATION
    {
        if(!playImmidiatly)
            action();
    }

    public void StartPlayAttack(Action action)
    {
        this.action = action;
        if (playImmidiatly)
        {
            action();
        }
    }
}
