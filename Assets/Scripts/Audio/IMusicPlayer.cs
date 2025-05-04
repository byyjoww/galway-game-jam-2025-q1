using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public interface IMusicPlayer : IAudioPlayer
    {
        string CurrentTrack { get; }
        int CurrentIndex { get; set; }

        event UnityAction OnTrackChanged;

        void Skip();
        void Back();
        void Pause();
        void Resume();
        void Rewind(float time);
        void FastForward(float time);
        float[] GetSpectrumData(int samples, int channel, FFTWindow window);        
    }
}
