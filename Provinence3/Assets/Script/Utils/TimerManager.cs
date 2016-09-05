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

        float Duration { get; }
        float EndTime { get; }
        float TimeLeft { get; }
        bool IsLoopped { get; }
        bool IsActive { get; }

        void Stop(bool shallFire = true);
    }

    private class Timer: ITimer
    {
        private Action onStop;
        private Action onStart;

        public event Action OnTimer;

        public float EndTime { get; private set; }
        public float Duration { get; private set; }
        public float TimeLeft 
        { 
            get
            {
                if(!IsActive)
                {

                }
                return EndTime - Time.time;
            }
        }
        public bool IsLoopped { get; private set; }
        public bool IsActive { get; private set; }

        public void Init(Action onStop, Action onRestart)
        {
            this.onStop = onStop;
            this.onStart = onRestart;
        }

        public void Start(float endTime, float duration, bool isLopped = false)
        {
            EndTime = endTime;
            Duration = duration;
            IsLoopped = isLopped;
            onStart();
            IsActive = true;
        }

        public void Stop(bool shallFire = true)
        {
            if (!IsActive)
            {

            }
            else
            {
                if (shallFire)
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

    private ITimer MakeTimer(float endTime, float duration, bool isLopped = false)
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

    public ITimer MakeTimer(float duration, bool isLopped)
    {
        return MakeTimer(Time.time + duration, duration, isLopped);
    }

    public ITimer MakeTimer(float endTime)
    {
        return MakeTimer(endTime, endTime + Time.time);
    }
    public ITimer MakeTimer(TimeSpan endTime)
    {
        var ms = (float)endTime.TotalMilliseconds/1000f;
        return MakeTimer(ms, ms + Time.time);
    }
    public ITimer MakeTimer(TimeSpan endTime, bool isLopped)
    {
        var ms = (float)endTime.TotalMilliseconds/1000f;
        return MakeTimer(ms, ms + Time.time);
    }

    public void Update()
    {
        var node = timers.First;
        while (node != null)
        {
            var next = node.Next;
            if (Time.time > node.Value.EndTime)
            {
                var timer = node.Value;
                timer.Stop();
                timer.Fire();
                if (timer.IsLoopped)
                {
                    timer.Start(Time.time + timer.Duration, timer.Duration, true);
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