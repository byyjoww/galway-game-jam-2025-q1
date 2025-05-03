using SLS.Core.Timers;
using System;
using UnityEngine.Events;

namespace Scamazon.Timers
{
    public class TimeLimit : IDisposable
    {
        [System.Serializable]
        public struct Config
        {
            public int TimeLimitInSeconds;
        }

        private ITimer timer = default;

        public TimeSpan TotalTime => timer.Interval;
        public TimeSpan ElapsedTime => timer.Elapsed;
        public TimeSpan RemainingTime => TotalTime - ElapsedTime;

        public event UnityAction<TimeSpan, TimeSpan> OnTimeElapsedChanged;
        public event UnityAction<TimeSpan, TimeSpan> OnTimeRemainingChanged;
        public event UnityAction OnExpire;

        public TimeLimit(Config config)
        {
            TimeSpan interval = TimeSpan.FromSeconds(config.TimeLimitInSeconds);
            timer = Timer.CreateScaledTimer("TIME_LIMIT", interval);
            timer.AutoRestart = false;
            timer.OnTick.AddListener(OnTimerUpdate);
            timer.OnEnd.AddListener(OnTimerEnd);
        }

        public void Start()
        {
            timer.Restart();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Resume()
        {
            timer.Start();
        }

        public void Increase(float seconds)
        {
            if (!timer.IsRunning) { return; }

            TimeSpan prevElapsed = ElapsedTime;
            TimeSpan prevRemaining = RemainingTime;
            TimeSpan increase = TimeSpan.FromSeconds(seconds);
            ElapsedTime.Add(increase);
            OnTimeElapsedChanged?.Invoke(prevElapsed, ElapsedTime);
            OnTimeRemainingChanged?.Invoke(prevRemaining, RemainingTime);
        }

        public void Decrease(float seconds)
        {
            if (!timer.IsRunning) { return; }

            TimeSpan prevElapsed = ElapsedTime;
            TimeSpan prevRemaining = RemainingTime;
            TimeSpan decrease = TimeSpan.FromSeconds(seconds);
            ElapsedTime.Subtract(decrease);
            OnTimeElapsedChanged?.Invoke(prevElapsed, ElapsedTime);
            OnTimeRemainingChanged?.Invoke(prevRemaining, RemainingTime);
        }

        private void OnTimerUpdate(TimeSpan tick)
        {
            TimeSpan prevElapsed = ElapsedTime.Subtract(tick);
            TimeSpan prevRemaining = TotalTime - prevElapsed;
            OnTimeElapsedChanged?.Invoke(prevElapsed, ElapsedTime);
            OnTimeRemainingChanged?.Invoke(prevRemaining, RemainingTime);
        }

        private void OnTimerEnd()
        {
            OnExpire?.Invoke();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}