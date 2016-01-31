using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PortalsController
{
    private const int PortalOpenTime = 30;
    private TimerManager.ITimer timer;
    private int nextPortalOpenTime;
    private int deltaPortalTime;
    private Action OnPortalOpen;

    public void Start(int levelTimeSec,Action OnPortalOpen)
    {
        this.OnPortalOpen = OnPortalOpen;
        timer = MainController.Instance.TimerManager.MakeTimer(new TimeSpan(levelTimeSec/2));
        timer.OnTimer += OnMiddleGame;
        deltaPortalTime = PortalOpenTime/10;
    }

    private void OnMiddleGame()
    {
        nextPortalOpenTime = PortalOpenTime;
        OpenNewPortalAction();

    }

    private void OpenNewPortalAction()
    {
        OpenPortal();
        nextPortalOpenTime -= deltaPortalTime;
        timer = MainController.Instance.TimerManager.MakeTimer(new TimeSpan(nextPortalOpenTime));
        timer.OnTimer += OpenNewPortalAction;

    }

    private void OpenPortal()
    {
        OnPortalOpen();
    }

    public void Stop()
    {
        if (timer != null)
            timer.Stop();
    }
}

