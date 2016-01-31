using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TimerManager
{
    public interface ITimer
    {
        event Action OnTimer;

        TimeSpan Duration { get; }
        DateTime EndTime { get; }
        TimeSpan TimeLeft { get; }
        bool IsLoopped { get; }
        bool IsActive { get; }

        void Stop();
    }

    private class Timer: ITimer
    {
        private Action onStop;
        private Action onStart;

        public event Action OnTimer;

        public DateTime EndTime { get; private set; }
        public TimeSpan Duration { get; private set; }
        public TimeSpan TimeLeft 
        { 
            get
            {
                if(!IsActive)
                {

                }
                return EndTime - DateTime.Now;
            }
        }
        public bool IsLoopped { get; private set; }
        public bool IsActive { get; private set; }

        public void Init(Action onStop, Action onRestart)
        {
            this.onStop = onStop;
            this.onStart = onRestart;
        }

        public void Start(DateTime endTime, TimeSpan duration, bool isLopped = false)
        {
            EndTime = endTime;
            Duration = duration;
            IsLoopped = isLopped;
            onStart();
            IsActive = true;
        }

        public void Stop()
        {
            if (!IsActive)
            {

            }
            else
            {
                onStop();
                IsActive = false;
            }
        }

        public void Fire()
        {
            if (OnTimer != null)
            {
                OnTimer();
            }
        }
    }

    private LinkedList<Timer> timers = new LinkedList<Timer>();

    public ITimer MakeTimer(DateTime endTime, TimeSpan duration, bool isLopped = false)
    {
        Timer timer = new Timer();
        timer.Init(() =>
        {
            timers.Remove(timer);
        }, () =>
        {
            AddTimer(timer);
        });
        timer.Start(endTime, duration, isLopped);
        return timer;
    }

    public ITimer MakeTimer(TimeSpan duration, bool isLopped = false)
    {
        return MakeTimer(DateTime.Now + duration, duration, isLopped);
    }

    public ITimer MakeTimer(DateTime endTime)
    {
        return MakeTimer(endTime, endTime - DateTime.Now);
    }

    public void Update()
    {
        var node = timers.First;
        while (node != null)
        {
            var next = node.Next;
            if (DateTime.Now > node.Value.EndTime)
            {
                var timer = node.Value;
                timer.Stop();
                timer.Fire();
                if (timer.IsLoopped)
                {
                    timer.Start(DateTime.Now + timer.Duration, timer.Duration, true);
                }
            }
            else
            {
                break;
            }
            node = next;
        }
    }

    private void AddTimer(Timer timer)
    {
        var node = timers.First;
        if (node == null)
        {
            timers.AddFirst(timer);
        }
        else
        {
            while (node != null)
            {
                var next = node.Next;
                if (node.Value.EndTime > timer.EndTime)
                {
                    timers.AddBefore(node, timer);
                    return;
                }
                node = next;
            }
            timers.AddLast(timer);
        }
    }
}