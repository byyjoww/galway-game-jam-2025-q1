using SLS.Core.Attributes;
using SLS.Core.Logging;
using SLS.Core.Timers;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class AudioSourceWrapper : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string guid = default;
        [SerializeField] private AudioSource source = default;

        private IUnityLogger logger = new UnityLogger();
        private ITimer timer = default;
        private CrossFader crossFader = default;

        private string prefix = string.Empty;
        private new string name = string.Empty;
        private string suffix = string.Empty;

        public float Volume => source.volume;
        public float Time => source.time;
        public AudioMixerGroup AudioMixerGroup => source.outputAudioMixerGroup;

        public string Prefix
        {
            get => prefix;
            set
            {
                prefix = value;
                UpdateFullName();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                UpdateFullName();
            }
        }

        public string Suffix
        {
            get => suffix;
            set
            {
                suffix = value;
                UpdateFullName();
            }
        }

        public UnityEvent OnEnd { get; private set; } = null;
        public UnityEvent OnDispose { get; private set; } = null;

        public event UnityAction<AudioSourceWrapper> OnDestroyed = default;

        private void Awake()
        {
            guid = Guid.NewGuid().ToString();
            timer = Timer.CreateUnscaledTimer($"AUDIO_SOURCE_{guid}");
            timer.OnEnd.AddListener(EndTimer);

            crossFader = new CrossFader(source);
        }

        public UnityEvent PlayClip(AudioClip clip, PlayOptions opts)
        {
            timer.Reset();
            timer.Interval = TimeSpan.FromSeconds(clip.length);

            source.clip = clip;
            source.loop = opts.Loop;

            if (opts.Volume.HasValue)
            {
                source.volume = opts.Volume.Value;
            }

            if (opts.Randomize)
            {
                source.time = UnityEngine.Random.Range(0f, source.clip.length);
            }

            source.Play();

            if (!source.loop)
            {
                timer.Start();
            }

            Name = $"PLAY_{clip.name}";
            return OnEnd;
        }

        public UnityEvent PlayOneShot(AudioClip clip)
        {
            timer.Reset();
            timer.Interval = TimeSpan.FromSeconds(clip.length);
            source.PlayOneShot(clip);
            timer.Start();
            Name = $"PLAY_ONE_SHOT_{clip.name}";
            return OnEnd;
        }

        public void SetVolume(float volume, float duration = 0f, UnityAction onComplete = null)
        {
            crossFader.Stop();

            if (duration > 0)
            {
                crossFader.StartFade(volume, duration, onComplete);
            }
            else
            {
                source.volume = volume;
                onComplete?.Invoke();
            }
        }

        public float[] GetSpectrumData(int samples, int channel, FFTWindow window)
        {
            return source.GetSpectrumData(samples, channel, window);
        }

        public void SetMixerGroup(AudioMixerGroup grp)
        {
            source.outputAudioMixerGroup = grp;
        }

        public void Stop()
        {
            timer.Reset();
            source.Stop();
        }

        public void Pause()
        {
            // AppLogger.LogDebug("pausing track");
            timer.Stop();
            source.Pause();
        }

        public void Resume()
        {
            // AppLogger.LogDebug("resuming track");
            timer.Start();
            source.UnPause();
        }

        public void Rewind()
        {
            Rewind(source.time);
        }

        public void Rewind(float time)
        {
            bool wasPlaying = false;
            if (source.isPlaying)
            {
                wasPlaying = true;
                timer.Stop();
                source.Stop();
            }

            float ts = source.time - time;
            float tsClamped = Mathf.Clamp(ts, 0, source.clip.length);
            source.time = tsClamped;
            timer.Interval -= TimeSpan.FromSeconds(time);

            if (wasPlaying)
            {
                timer.Start();
                source.Play();
            }
        }

        public void FastForward(float time)
        {
            bool wasPlaying = false;
            if (source.isPlaying)
            {
                wasPlaying = true;
                timer.Stop();
                source.Stop();
            }

            float ts = source.time + time;
            float tsClamped = Mathf.Clamp(ts, 0, source.clip.length);
            source.time = tsClamped;
            timer.Interval += TimeSpan.FromSeconds(time);

            if (wasPlaying)
            {
                timer.Start();
                source.Play();
            }
        }

        public void Dispose()
        {
            Stop();
            OnDispose.Invoke();
        }

        public void OnRequest()
        {
            OnEnd = new UnityEvent();
            OnDispose = new UnityEvent();
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            source.clip = null;
            source.loop = false;
            source.volume = 1f;
            OnEnd?.RemoveAllListeners();
            OnEnd = null;
            OnDispose?.RemoveAllListeners();
            OnDispose = null;
            timer.Reset();
            timer.Interval = TimeSpan.Zero;
            Prefix = string.Empty;
            Name = "INACTIVE";
            Suffix = string.Empty;
            gameObject.SetActive(false);
        }

        private void EndTimer()
        {
            if (HasTimeRemaining(out float remaining))
            {
                logger.LogDebug($"Audio source {gameObject.name} still has time remaining: {remaining}");
                timer.Interval = TimeSpan.FromSeconds(remaining);
                timer.Restart();
                return;
            }

            source?.Stop();
            OnEnd?.Invoke();
        }

        private void UpdateFullName()
        {
            gameObject.name = $"{prefix} {name} {suffix}";
        }

        private bool HasTimeRemaining(out float remaining)
        {
            if (!source.clip)
            {
                // is play one shot
                remaining = 0f;
                return false;
            }

            remaining = source.clip.length - source.time;
            return source.isPlaying && remaining > 0f;
        }

        private void Update()
        {
            crossFader?.Update();
        }

        private void OnDestroy()
        {
            timer?.Dispose();
            OnDestroyed?.Invoke(this);
        }

        private void OnValidate()
        {
            source ??= GetComponent<AudioSource>();
        }
    }
}