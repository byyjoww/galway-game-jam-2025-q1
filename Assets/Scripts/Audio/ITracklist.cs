using UnityEngine;

namespace Scamazon.Audio
{
    public interface ITracklist
    {
        bool TryGetRandomTrack(out AudioTrack track);
        bool TryGetNextTrack(AudioClip last, out AudioTrack next);
        bool TryGetNextTrack(AudioTrack last, out AudioTrack next);
        bool TryGetPrevTrack(AudioClip last, out AudioTrack next);
        bool TryGetPrevTrack(AudioTrack last, out AudioTrack next);
    }
}
