using UnityEngine.Audio;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class VolumeChannel
    {
        [System.Serializable]
        public struct Config
        {
            public float Volume { get; set; }
        }

        private AudioMixer mixer = default;
        private AudioMixerGroup group = default;
        private Config config = default;

        public AudioMixerGroup MixerGroup => group;

        public bool IsMuted
        {
            get => TryGetVolume(out float volume) == true && volume <= -80f;
            set
            {
                bool prev = IsMuted;
                float newVolume = value 
                    ? -80f 
                    : config.Volume;

                SetVolume(newVolume);
                if (prev != value) { OnMutedChanged?.Invoke(); }
            }
        }

        public event UnityAction<float, float> OnVolumeChanged;
        public event UnityAction OnMutedChanged;

        public VolumeChannel(AudioMixer mixer, AudioMixerGroup group, Config config)
        {
            this.mixer = mixer;
            this.group = group;
            this.config = config;
            SetVolume(config.Volume);
        }

        public bool TryGetVolume(out float volume)
        {
            return mixer.GetFloat($"{group.name}Volume", out volume);
        }

        public void SetVolume(float volume)
        {
            TryGetVolume(out float prev);
            mixer.SetFloat($"{group.name}Volume", volume);
            OnVolumeChanged?.Invoke(prev, volume);
        }

        // Normalize dB value to normalized range 0 - 1
        public static float DecibelsToNormalized(float db)
        {
            Decibels dB = new Decibels(db);
            return dB.Normalized;
        }

        // Convert the normalized value (0 - 1) back to dB
        public static float NormalizedToDecibels(float normalized)
        {
            Decibels dB = Decibels.FromNormalized(normalized);
            return dB.Value;
        }
    }
}
