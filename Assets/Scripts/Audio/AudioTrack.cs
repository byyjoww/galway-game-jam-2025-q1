using UnityEngine;

namespace Scamazon.Audio
{
    [System.Serializable]
    public class AudioTrack
    {
        [SerializeField] private string name = default;
        [SerializeField] private AudioClip clip = default;

        public string Name => name;
        public AudioClip Clip => clip;
    }
}
