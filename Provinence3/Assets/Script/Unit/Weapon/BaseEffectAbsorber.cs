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


    public IEnumerator DestroyPS(float waitTime = 4f,string reason = "")
    {
        yield return new WaitForSeconds(waitTime);
//        try
//        {

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
//        }
//        catch (Exception)
//        {
//            
//            Debug.Log(reason);
//        }
    }
}

