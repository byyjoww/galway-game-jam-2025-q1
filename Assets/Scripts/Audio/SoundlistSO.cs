using SLS.Core.Extensions;
using System;
using UnityEngine;

namespace Scamazon.Audio
{
    [CreateAssetMenu(fileName = "SoundlistSO", menuName = "Scriptable Objects/Audio/Soundlist")]
    public class SoundlistSO : ScriptableObject, ISoundlist
    {
        [SerializeField] private AudioClip[] tracks = default;

        public bool TryGetRandomClip(out AudioClip clip)
        {
            clip = tracks.Length > 0
                ? tracks.Random()
                : null;

            return clip != null;
        }

        public bool TryGetNextClip(AudioClip last, out AudioClip next)
        {
            var index = last != null
                ? Array.IndexOf(tracks, last)
                : 0;

            return TryGetNextClipByIndex(index, out next);
        }

        public bool TryGetNextClipByIndex(int index, out AudioClip next)
        {
            if (index == -1)
            {
                next = null;
                return false;
            }

            var nextIndex = (index + 1) % tracks.Length;
            next = tracks[nextIndex];
            return true;
        }
    }
}