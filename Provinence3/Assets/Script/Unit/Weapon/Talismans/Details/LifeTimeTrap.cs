using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LifeTimeTrap : MonoBehaviour
{
    public float lifeTimeSec = 10;
    private TimerManager.ITimer timer;
    public BaseEffectAbsorber dieEffect;

    public virtual void Init()
    {
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(lifeTimeSec));
        timer.OnTimer += OnTimer;
    }

    private void OnTimer()
    {
        if (gameObject != null)
        {
            if (dieEffect != null)
            {
                Map.Instance.LeaveEffect(dieEffect);
                dieEffect.Play();
            }
            timer = null;
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (timer != null)
        {
            timer.Stop();
        }
    }
}

