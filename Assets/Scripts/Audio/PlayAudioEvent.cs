using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class PlayAudioEvent : AudioEvent
    {
        public PlayAudioEvent(AudioSourceWrapper source) : base(source)
        {
            this.source = source;
        }

        public void SetVolume(float normalized, float duration = 0f, UnityAction onComplete = null)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.SetVolume(normalized, duration, onComplete);
        }

        public void SetPrefix(string prefix)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Prefix = prefix;
        }

        public void SetName(string name)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Name = name;
        }

        public void SetSuffix(string suffix)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Suffix = suffix;
        }

        public void ChangeClip(AudioClip clip, PlayOptions opts)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.PlayClip(clip, opts);
        }

        public void Stop()
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Stop();
        }

        public void Pause()
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Pause();
        }

        public void Resume()
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Resume();
        }

        public void Rewind()
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Rewind();
        }

        public void Rewind(float time)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.Rewind(time);
        }

        public void FastForward(float time)
        {
            if (IsDisposed)
            {
                // AppLogger.LogError("This audio source has already been disposed");
                return;
            }

            source.FastForward(time);
        }
    }
}