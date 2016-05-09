using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MapObjectWithDeath : MonoBehaviour
{
    public Animator DeathAnimator;
    private const string key_death = "death";

    public virtual void SetDeath()
    {
        if (DeathAnimator == null)
        {
            EndDeathAnimation();
        }
        else
        {
            DeathAnimator.SetTrigger(key_death);
        }
    }

    public void EndDeathAnimation()
    {
        Destroy(gameObject);
    }
}

