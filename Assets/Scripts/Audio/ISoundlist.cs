using UnityEngine;
using UnityEngine.Audio;

namespace Scamazon.Audio
{
    public interface ISoundlist
    {
        bool TryGetRandomClip(out AudioClip clip);
        bool TryGetNextClip(AudioClip last, out AudioClip next);
    }

    public interface IAudioPlayer
    {
        float Volume { get; }
        AudioMixerGroup AudioMixerGroup { get; }

        PlayAudioEvent Play(AudioClip clip, PlayOptions opts);
        PlayOneShotAudioEvent PlayOneShot(AudioClip clip);
        void SetVolume(float volume);
    }
}