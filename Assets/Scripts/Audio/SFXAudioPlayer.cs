using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class SFXAudioPlayer : AudioPlayerBase, IAudioPlayer
    {
        public SFXAudioPlayer(AudioSourceWrapper source, VolumeChannel channel) : base(source, channel)
        {

        }
    }
}
