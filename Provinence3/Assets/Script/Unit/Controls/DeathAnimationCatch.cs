using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DeathAnimationCatch : MonoBehaviour
{
    private Action endDeathAnimation;
    public Animator DeathAnimator;

    private void Awake()
    {
        if (DeathAnimator == null)
        {
            DeathAnimator = GetComponent<Animator>();
        }
    }

    public void SetAction(Action endDeathAnimation)
    {
        this.endDeathAnimation = endDeathAnimation;
    }
    public void EndDeathAnimation()
    {
        if (endDeathAnimation != null)
            endDeathAnimation();
    }
}

