using UnityEngine;
using UnityEngine.Audio;

namespace Scamazon.Audio
{
    public interface ISoundlist
    {
        bool TryGetRandomClip(out AudioClip clip);
        bool TryGetNextClip(AudioClip last, out AudioClip next);
    }
}