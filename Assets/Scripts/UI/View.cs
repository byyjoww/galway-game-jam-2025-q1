using Scamazon.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scamazon.UI
{
    public abstract class View : MonoBehaviour
    {
        [SerializeField] private SoundlistSO onShowSFX = default;
        [SerializeField] private SoundlistSO onHideSFX = default;

        protected IAudioPlayer audioPlayer = default;
        protected ISoundlist buttonSfx = default;

        public event UnityAction<bool> OnActiveStateChanged;

        public virtual void Init()
        {

        }

        public void SetAudioPlayer(IAudioPlayer audioPlayer)
        {
            this.audioPlayer = audioPlayer;
        }

        public void SetButtonAudio(ISoundlist sfx)
        {
            this.buttonSfx = sfx;
        }

        public virtual void SetActive(bool active)
        {
            if (gameObject.activeSelf == active) { return; }

            if (active)
            {
                PlayShowSFX();
            }
            else
            {
                PlayHideSFX();
            }

            gameObject.SetActive(active);
            TriggerOnActiveStateChanged(active);
        }

        protected void TriggerOnActiveStateChanged(bool active)
        {
            OnActiveStateChanged?.Invoke(active);
        }

        protected void SetButtonAction(ButtonViewBase button, UnityAction action, bool canExecute = true)
        {
            button.Init(audioPlayer);
            button.Button.onClick.RemoveAllListeners();
            button.Button.onClick.AddListener(PlayButtonClickSFX);
            if (action != null)
            {
                button.Button.onClick.AddListener(action);
            }
            
            button.Button.interactable = canExecute;
        }

        protected void SetButtonNavigation(ButtonViewBase button, ButtonViewBase up = null, ButtonViewBase down = null, ButtonViewBase left = null, ButtonViewBase right = null)
        {
            button.Button.navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = up?.Button,
                selectOnDown = down?.Button,
                selectOnLeft = left?.Button,
                selectOnRight = right?.Button,
            };
        }

        public void PlayOneShotAudio(ISoundlist soundlist)
        {
            if (soundlist != null && soundlist.TryGetRandomClip(out var clip))
            {
                audioPlayer?.PlayOneShot(clip);
            }
        }

        private void PlayShowSFX()
        {
            if (onShowSFX != null && onShowSFX.TryGetRandomClip(out var clip))
            {
                audioPlayer?.PlayOneShot(clip);
            }
        }

        private void PlayHideSFX()
        {
            if (onHideSFX != null && onHideSFX.TryGetRandomClip(out var clip))
            {
                audioPlayer?.PlayOneShot(clip);
            }
        }

        internal void PlayButtonClickSFX()
        {
            if (buttonSfx == null)
            {
                Debug.LogWarning($"No tracklist present when attempting to play ButtonClickSFX.{gameObject.name}");
                return;
            }

            if (buttonSfx.TryGetRandomClip(out AudioClip clip))
            {
                audioPlayer.PlayOneShot(clip);
            }
        }
    }
}