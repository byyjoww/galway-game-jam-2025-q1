using SLS.Core.Timers;
using System;
using UnityEngine.Events;

namespace Scamazon.Virus
{
    public abstract class Virus : IDisposable
    {
        private ITimer timer = default;
        private float duration = default;

        public event UnityAction OnEnd;

        public Virus(float duration)
        {
            this.duration = duration;
            timer = Timer.CreateScaledTimer(TimeSpan.FromSeconds(duration));
            timer.OnEnd.AddListener(delegate
            {
                timer?.Dispose();
                OnEnded();
                OnEnd?.Invoke();
            });
        }

        public void Execute()
        {
            OnStarted();            
            timer.Start();
        }

        protected abstract void OnStarted();
        protected abstract void OnEnded();

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
