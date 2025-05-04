using SLS.Core.Extensions;
using System;
using System.Linq;
using UnityEngine;

namespace Scamazon.Audio
{
    [CreateAssetMenu(fileName = "TracklistSO", menuName = "Scriptable Objects/Audio/Tracklist")]
    public class TracklistSO : ScriptableObject, ITracklist
    {
        [SerializeField] private AudioTrack[] tracks = default;

        public bool TryGetRandomTrack(out AudioTrack clip)
        {
            var eligible = GetEligibleTracks();
            clip = eligible.Length > 0 ? eligible.Random() : null;
            return clip != null;
        }

        public bool TryGetNextTrack(AudioClip lastClip, out AudioTrack next)
        {
            var lastTrack = FindTrackByClip(lastClip);
            return lastTrack != null
                ? TryGetNextTrack(lastTrack, out next)
                : TryGetRandomTrack(out next);
        }

        public bool TryGetNextTrack(AudioTrack last, out AudioTrack next)
        {
            if (last == null)
            {
                return TryGetRandomTrack(out next);
            }                

            return TryGetAdjacentTrack(last, forward: true, out next);
        }

        public bool TryGetPrevTrack(AudioClip lastClip, out AudioTrack next)
        {
            var lastTrack = FindTrackByClip(lastClip);
            return lastTrack != null
                ? TryGetPrevTrack(lastTrack, out next)
                : TryGetRandomTrack(out next);
        }

        public bool TryGetPrevTrack(AudioTrack last, out AudioTrack next)
        {
            if (last == null)
            {
                return TryGetRandomTrack(out next);
            }

            return TryGetAdjacentTrack(last, forward: false, out next);
        }

        private bool TryGetAdjacentTrack(AudioTrack last, bool forward, out AudioTrack next)
        {
            var eligible = GetEligibleTracks();
            if (eligible.Length == 0)
            {
                next = null;
                return false;
            }

            int currentIndex = Array.IndexOf(eligible, last);
            if (currentIndex == -1)
            {
                next = null;
                return false;
            }

            int offset = forward ? 1 : -1;
            int newIndex = (currentIndex + offset + eligible.Length) % eligible.Length;
            next = eligible[newIndex];
            return true;
        }

        private AudioTrack[] GetEligibleTracks()
        {
            return tracks;
        }

        private AudioTrack FindTrackByClip(AudioClip clip)
        {
            return tracks.FirstOrDefault(x => x.Clip == clip);
        }
    }
}
