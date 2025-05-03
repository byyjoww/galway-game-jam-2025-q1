namespace Scamazon.Audio
{
    public class PlayOneShotAudioEvent : AudioEvent
    {
        public PlayOneShotAudioEvent(AudioSourceWrapper source) : base(source)
        {
            OnEnd.AddListener(Dispose);
        }
    }
}