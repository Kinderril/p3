using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MapObjectWithDeath : MonoBehaviour
{
    private const string key_death = "death";
    public DeathAnimationCatch DeathAnimationCatcher;

    public virtual void SetDeath()
    {
        if (DeathAnimationCatcher != null && DeathAnimationCatcher.DeathAnimator != null)
        {
            DeathAnimationCatcher.SetAction(EndDeathAnimation);
            DeathAnimationCatcher.DeathAnimator.SetTrigger(key_death);
        }
        else
        {
            EndDeathAnimation();
        }
    }

    public void EndDeathAnimation()
    {
        Destroy(gameObject);
        Debug.Log("Destroy " + name);
    }
}

