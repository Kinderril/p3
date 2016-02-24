using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseMapItem : MonoBehaviour
{
    public BaseEffectAbsorber OpenEffect;
    public Animator animator;
    private bool canBeTaken = false;

    public void Init()
    {
        Utils.SetRandomRotation(transform);
    }
    void OnTriggerEnter(Collider other)
    {
        if (canBeTaken)
        {
            var unit = other.GetComponent<Hero>();
            if (unit != null)
            {
                canBeTaken = false;
                Take(unit);
                if (OpenEffect != null)
                {
                    OpenEffect.Play();
                    Map.Instance.LeaveEffect(OpenEffect);
                }
                Destroy(gameObject);
            }
        }
    }

    protected virtual void Take(Hero unit)
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
    public void EndAnimation()
    {
        canBeTaken = true;//STUB
    }

    public void StartFly(Transform parentTransform)
    {
        transform.rotation = parentTransform.rotation;
        if (animator != null)
        {
            animator.SetBool("isOpen", true);
        }
        else
        {
            EndAnimation();
        }
    }
}

