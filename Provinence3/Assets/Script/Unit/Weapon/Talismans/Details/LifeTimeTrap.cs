using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LifeTimeTrap : MonoBehaviour
{
    public const float LIFE_TIME = 10;
    private TimerManager.ITimer timer;
    public BaseEffectAbsorber dieEffect;

    public virtual void Init()
    {
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(LIFE_TIME));
        timer.OnTimer += OnTimer;
    }

    private void OnTimer()
    {
        if (gameObject != null)
        {
            if (dieEffect != null)
            {
                Map.Instance.LeaveEffect(dieEffect,transform);
                dieEffect.Play();
            }
            timer = null;
            Destroy(gameObject);
        }
    }

    protected virtual void subDestroy()
    {
        
    }

    void OnDestroy()
    {
        subDestroy();
        if (timer != null)
        {
            timer.Stop();
        }
    }
}

