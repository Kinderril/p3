using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseEffectAbsorber : MonoBehaviour
{
    public virtual void Play()
    {
        
    }

    public virtual void Stop()
    {
        
    }


    public IEnumerator DestroyPS(Transform oldTransform,float waitTime = 4f,string reason = "")
    {
        yield return new WaitForSeconds(waitTime);
        Stop();
//        if (gameObject != null)
//        {
//            Destroy(gameObject);
//        }
    }

    public virtual void SetColor(Color color)
    {

    }
}

