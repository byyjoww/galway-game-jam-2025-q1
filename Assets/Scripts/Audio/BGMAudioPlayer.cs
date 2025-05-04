using SLS.Core.Logging;
using SLS.Core.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class BGMAudioPlayer : AudioPlayerBase, IMusicPlayer, IDisposable
    {
        private const float TRACK_CHANGE_DELAY_IN_SECONDS = 0.1f;
        private const float TRACK_RESTART_THRESHOLD = 5f;

        private IUnityLogger logger = new UnityLogger();
        private ITracklist bgmTracklist = default;
        private ITimer trackDelayTimer = default;        
        private ISoundlist trackChangeSfx = default;
        private IAudioPlayer sfxAudioPlayer = default;
        private AudioTrack last = default;
        private int currentIndex = 0;
        private bool isPaused = default;
        private UnityAction onUnpause = default;

        private PlayAudioEvent active = default;
        private List<PlayAudioEvent> audioEvents = default;
                
        public override string Prefix => "[BGM]";
        public string CurrentTrack { get; private set; }

        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (currentIndex == value) { return; }
                currentIndex = value;
                SetAudioEventVolumes();
            }
        }

        public event UnityAction OnTrackChanged;

        public BGMAudioPlayer(AudioSourceWrapper source, VolumeChannel channel, ITracklist bgmTracklist, ISoundlist trackChangeSfx, IAudioPlayer sfxAudioPlayer) : base(source, channel)
        {
            this.bgmTracklist = bgmTracklist;
            this.trackChangeSfx = trackChangeSfx;
            this.sfxAudioPlayer = sfxAudioPlayer;
            this.audioEvents = new List<PlayAudioEvent>();
            trackDelayTimer = Timer.CreateUnscaledTimer("BGM_PLAYER_TRACK_CHANGE_DELAY");            
        }

        public void StartRandom()
        {
            if (isPaused)
            {
                onUnpause = StartRandom;
                return;
            }

            if (bgmTracklist.TryGetRandomTrack(out AudioTrack track))
            {
                Start(track);
            }
        }

        public void StartNext()
        {
            if (isPaused)
            {
                onUnpause = StartNext;
                return;
            }

            if (bgmTracklist.TryGetNextTrack(last, out AudioTrack track))
            {
                Start(track);
            }
        }

        public void StartPrev()
        {
            if (isPaused)
            {
                onUnpause = StartPrev;
                return;
            }

            if (bgmTracklist.TryGetPrevTrack(last, out AudioTrack track))
            {
                Start(track);
            }
        }

        private void Start(AudioTrack track)
        {
            trackDelayTimer.Stop();
            trackDelayTimer.OnEnd.RemoveAllListeners();

            CurrentTrack = track.Name;
            last = track;

            logger.LogDebug($"Changing audio track to [{track.Name}]");

            audioEvents.AddRange(new PlayAudioEvent[]
            {
                Play(track.Clip, new PlayOptions { Volume = 0f }),
            }.Where(x => x != null));
            SetAudioEventVolumes();

            if (audioEvents.Count < 1)
            {
                logger.LogError("Trying to play a null clip");
                audioEvents.Clear();
                StartRandom();
                return;
            }

            audioEvents.ForEach(x => x?.OnEnd.AddListener(delegate
            {
                FinishAndChangeTracks(true);
            }));
            OnTrackChanged?.Invoke();
        }

        private void FinishAndChangeTracks(bool moveNext)
        {
            logger.LogDebug("Current audio track has ended.");
            Stop();
            audioEvents.ForEach(x => x?.Dispose());
            audioEvents.Clear();
            StartChangeTrackDelay(moveNext);
        }

        private void StartChangeTrackDelay(bool moveNext)
        {
            float changeTrackTotalDelay = TRACK_CHANGE_DELAY_IN_SECONDS;
            if (trackChangeSfx.TryGetRandomClip(out AudioClip clip))
            {
                sfxAudioPlayer.PlayOneShot(clip);
                changeTrackTotalDelay += clip.length;
            }

            trackDelayTimer.Interval = TimeSpan.FromSeconds(changeTrackTotalDelay);
            trackDelayTimer.OnEnd.AddListener(moveNext ? StartNext : StartPrev);
            trackDelayTimer.Restart();
        }

        public float[] GetSpectrumData(int samples, int channel, FFTWindow window)
        {
            return active?.Source?.GetSpectrumData(samples, channel, window) ?? new float[samples];
        }

        public void Stop()
        {
            audioEvents.ForEach(x => x.Stop());
        }

        public void Pause()
        {
            isPaused = true;
            audioEvents.ForEach(x => x.Pause());
        }

        public void Resume()
        {
            isPaused = false;
            audioEvents.ForEach(x => x.Resume());
            onUnpause?.Invoke();
            onUnpause = null;
        }

        public void Rewind(float time)
        {
            audioEvents.ForEach(x => x.Rewind(time));
        }

        public void FastForward(float time)
        {
            audioEvents.ForEach(x => x.FastForward(time));
        }

        public void Skip()
        {
            if (isPaused || audioEvents.Count < 1) { return; }
            
            audioEvents.FirstOrDefault()?.OnEnd?.Invoke();
        }

        public void Back()
        {
            if (isPaused || audioEvents.Count < 1) { return; }

            float progress = audioEvents.FirstOrDefault()?.Source?.Time ?? 0;
            audioEvents.ForEach(x =>
            {
                x.Pause();
                x.Rewind();
            });

            if (progress > TRACK_RESTART_THRESHOLD)
            {
                Resume();
            }
            else
            {
                FinishAndChangeTracks(false);
            }
        }

        private float GetVolume(int trackIndex)
        {
            int closestIndex = Mathf.Clamp(CurrentIndex, 0, audioEvents.Count - 1);
            return trackIndex == closestIndex
                ? 1.0f 
                : 0.0f;
        }

        private void SetAudioEventVolumes()
        {
            active = null;
            for (int i = 0; i < audioEvents.Count; i++)
            {
                PlayAudioEvent audioEv = audioEvents[i];
                float vol = GetVolume(i);
                audioEv.SetVolume(vol, 1f);
                bool isActive = vol == 1f;
                audioEv.SetSuffix(isActive ? "(ACTIVE)" : string.Empty);

                if (isActive)
                {
                    active = audioEv;
                }
            }
        }

        public override void Dispose()
        {            
            trackDelayTimer?.Dispose();
            base.Dispose();
        }        
    }
}
