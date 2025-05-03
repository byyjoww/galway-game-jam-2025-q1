using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class CrossFader
    {
        public bool USE_UNSCALED_DELTA_TIME = true;

        private AudioSource audioSource;
        private float fadeDuration;
        private float startVolume;
        private float targetVolume;
        private float fadeTimer;
        private bool isFading;
        private UnityAction onComplete;

        /// <summary>
        /// Checks if the CrossFader is currently fading.
        /// </summary>
        public bool IsFading => isFading;

        public CrossFader(AudioSource source)
        {
            audioSource = source;
        }

        /// <summary>
        /// Starts fading the audio to the target volume over a duration.
        /// </summary>
        public void StartFade(float target, float duration, UnityAction onComplete = null)
        {
            this.onComplete = onComplete;
            startVolume = audioSource.volume;
            targetVolume = target;
            fadeDuration = duration;
            fadeTimer = 0f;
            isFading = true;
        }

        public void Stop()
        {
            isFading = false;
            onComplete = null;
        }

        /// <summary>
        /// Updates the fading logic. Should be called from MonoBehaviour's Update.
        /// </summary>
        public void Update()
        {
            if (!isFading) { return; }

            fadeTimer += USE_UNSCALED_DELTA_TIME
                ? Time.unscaledDeltaTime
                : Time.deltaTime;

            float t = Mathf.Clamp01(fadeTimer / fadeDuration);
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            if (t >= 1f)
            {
                onComplete?.Invoke();
                Stop();
            }
        }
    }
}