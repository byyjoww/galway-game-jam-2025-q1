using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Scamazon.Audio
{
    public class AudioSystem : IDisposable
    {
        private BGMAudioPlayer bgmAudioPlayer = default;
        private SFXAudioPlayer sfxAudioPlayer = default;
        private VolumeChannel bgmChannel = default;
        private VolumeChannel sfxChannel = default;
        private VolumeChannel masterChannel = default;
        private float bgmVolume = default;
        private float sfxVolume = default;

        public IAudioPlayer SFXPlayer => sfxAudioPlayer;
        public IMusicPlayer BGMPlayer => bgmAudioPlayer;

        public AudioSystem(AudioMixer mixer, AudioSourceWrapper bgm, AudioSourceWrapper sfx, float bgmVolume, float sfxVolume, 
            ITracklist bgmTracklist, ISoundlist trackChangeSfx)
        {
            this.bgmVolume = bgmVolume;
            this.sfxVolume = sfxVolume;

            bgmChannel = CreateVolumeChannel(mixer, "BGM", bgmVolume /*- 12.5f*/);
            sfxChannel = CreateVolumeChannel(mixer, "SFX", sfxVolume /*-15f*/);
            masterChannel = CreateVolumeChannel(mixer, "Master", 0f);

            sfxAudioPlayer = new SFXAudioPlayer(sfx, sfxChannel);
            bgmAudioPlayer = new BGMAudioPlayer(bgm, bgmChannel, bgmTracklist, trackChangeSfx, sfxAudioPlayer);            

            bgmChannel.OnVolumeChanged += OnBGMVolumeUpdated;
            sfxChannel.OnVolumeChanged += OnSFXVolumeUpdated;
            // bgmVolume.OnValueChanged += SetBGMVolume;
            // sfxVolume.OnValueChanged += SetSFXVolume;
        }                

        public void Init()
        {
            bgmAudioPlayer.StartRandom();
        }

        public void SkipCurrentBGMTrack()
        {
            bgmAudioPlayer.Skip();
        }

        public void SetBGMVolume()
        {
            bgmChannel.SetVolume(bgmVolume);
        }

        public void SetSFXVolume()
        {
            sfxChannel.SetVolume(sfxVolume);
        }

        private void OnBGMVolumeUpdated(float prev, float vol)
        {
            bgmVolume = vol;
        }

        private void OnSFXVolumeUpdated(float prev, float vol)
        {
            sfxVolume = vol;
        }

        private VolumeChannel CreateVolumeChannel(AudioMixer mixer, string name, float volume)
        {
            return new VolumeChannel(mixer, mixer.FindMatchingGroups(name).First(), new VolumeChannel.Config
            {
                Volume = volume,
            });
        }

        public void Dispose()
        {
            bgmChannel.OnVolumeChanged -= OnBGMVolumeUpdated;
            sfxChannel.OnVolumeChanged -= OnSFXVolumeUpdated;
            // bgmVolume.OnValueChanged -= SetBGMVolume;
            // sfxVolume.OnValueChanged -= SetSFXVolume;
            bgmAudioPlayer?.Dispose();
            sfxAudioPlayer?.Dispose();
        }
    }
}
