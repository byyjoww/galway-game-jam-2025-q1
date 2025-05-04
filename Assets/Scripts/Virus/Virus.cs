using SLS.Core.Timers;
using System;
using UnityEngine.Events;

namespace Scamazon.Virus
{
    public abstract class Virus : IDisposable
    {
        private ITimer timer = default;
        private float duration = default;

        public bool isExecuting = false;

        public event UnityAction OnEnd;

        public Virus(float duration)
        {
            this.duration = duration;
            timer = Timer.CreateScaledTimer(TimeSpan.FromSeconds(duration));
            timer.OnEnd.AddListener(OnTimerEnd);
        }

        private void OnTimerEnd()
        {            
            OnEnded();
            isExecuting = false;
            OnEnd?.Invoke();
        }

        public void Execute()
        {
            isExecuting = true;
            OnStarted();
            timer.Start();
        }

        protected abstract void OnStarted();
        protected abstract void OnEnded();

        public virtual void Dispose()
        {
            if (timer.IsRunning)
            {
                OnTimerEnd();
            }

            timer?.Dispose();
        }
    }
}
