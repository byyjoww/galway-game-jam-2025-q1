using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Scamazon.Audio
{
    public class AudioPlayerBase : IDisposable
    {
        private int prewarm = 10;
        private AudioSourceWrapper source = default;
        private VolumeChannel channel = default;
        private List<AudioSourceWrapper> sourcesPool = default;

        public virtual string Prefix => "[SFX]";
        public float Volume => channel.TryGetVolume(out float vol)
            ? vol
            : 0;

        protected AudioSourceWrapper Source => source;
        public AudioMixerGroup AudioMixerGroup => Source.AudioMixerGroup;

        public AudioPlayerBase(AudioSourceWrapper source, VolumeChannel channel)
        {
            this.channel = channel;
            this.source = source;
            sourcesPool = new List<AudioSourceWrapper>();
            Prewarm(prewarm);
        }

        protected virtual void Prewarm(int amount)
        {
            source.gameObject.SetActive(false);
            var prewarmed = new List<AudioSourceWrapper>();
            for (int i = 0; i < amount; i++)
            {
                var s = Create();
                prewarmed.Add(s);
            }

            prewarmed.ForEach(x => Return(x));
            prewarmed.Clear();
        }

        protected virtual AudioSourceWrapper Create()
        {
            if (sourcesPool.Count == 0)
            {
                AudioSourceWrapper newSource = GameObject.Instantiate(source, source.transform.parent);
                newSource.OnDestroyed += Remove;
                newSource.SetMixerGroup(channel.MixerGroup);                
                sourcesPool.Add(newSource);
            }

            AudioSourceWrapper s = sourcesPool.First();
            sourcesPool.Remove(s);
            s.OnRequest();
            s.OnDispose.AddListener(delegate { Return(s); });
            s.Prefix = Prefix;
            return s;
        }

        protected virtual void Return(AudioSourceWrapper source)
        {
            source.OnReturn();
            sourcesPool.Add(source);
        }

        protected virtual void Remove(AudioSourceWrapper source)
        {
            source.OnDestroyed -= Remove;
            sourcesPool.Remove(source);
        }

        public PlayAudioEvent Play(AudioClip clip, PlayOptions opts)
        {
            if (clip == null || clip.length <= 0)
            {
                return null;
            }

            var wrapper = Create();            
            wrapper.PlayClip(clip, opts);
            return new PlayAudioEvent(wrapper);
        }

        public PlayOneShotAudioEvent PlayOneShot(AudioClip clip)
        {
            if (clip == null || clip.length <= 0)
            {
                return null;
            }

            var wrapper = Create();
            wrapper.PlayOneShot(clip);
            return new PlayOneShotAudioEvent(wrapper);
        }

        public void SetVolume(float volume)
        {
            channel.SetVolume(volume);
        }

        public virtual void Dispose()
        {
            
        }
    }
}
