using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BulletEffect
{
    private TimerManager.ITimer timer;
    private Action undoAction;
    private Unit trg;
    public BulletEffect(Unit unit,float leightMS,Action setAction,Action undoAction)
    {
        trg = unit;
        this.undoAction = undoAction;
        unit.OnDead += OnDead;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromMilliseconds(leightMS));
        timer.OnTimer += OnEnd;
        setAction();
    }

    private void OnEnd()
    {
        undoAction();
        trg.OnDead -= OnDead;
    }

    private void OnDead(Unit obj)
    {
        trg.OnDead -= OnDead;
        timer.Stop();
    }
}

