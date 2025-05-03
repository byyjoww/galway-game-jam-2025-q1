using System;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class AudioEvent : IDisposable
    {
        protected AudioSourceWrapper source = default;

        public UnityEvent OnEnd => source.OnEnd;
        public bool IsDisposed => !source || source == null;
        public AudioSourceWrapper Source => source;

        public AudioEvent(AudioSourceWrapper source)
        {
            this.source = source;
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                source = null;
                return;
            }

            source?.Dispose();
            source = null;
        }
    }
}