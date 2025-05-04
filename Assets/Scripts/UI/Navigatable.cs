using Scamazon.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Scamazon.UI
{
    public class Navigatable : MonoBehaviour
    {
        [SerializeField] private SoundlistSO onSelectAudio = default;
        [HideInInspector] public UnityEvent onSelect = default;
        [HideInInspector] public UnityEvent onDeselect = default;
        [HideInInspector] public UnityEvent onPointerEnter = default;
        [HideInInspector] public UnityEvent onPointerExit = default;

        private IAudioPlayer audioPlayer = default;
        private EventTrigger eventTrigger = default;
        private bool initialized = false;

        public virtual GameObject NavigatableObj => gameObject;

        public virtual void Init(IAudioPlayer audioPlayer)
        {
            this.audioPlayer = audioPlayer;

            if (initialized) { return; }
            initialized = true;
            SetupEventTrigger();
        }

        private void SetupEventTrigger()
        {
            AddEmptyEventTrigger();

            var onSelect = new EventTrigger.TriggerEvent();
            onSelect.AddListener(OnSelect);

            eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.Select,
                callback = onSelect,
            });

            var onDeselect = new EventTrigger.TriggerEvent();
            onDeselect.AddListener(OnDeselect);

            eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.Deselect,
                callback = onDeselect,
            });

            var onPointerEnter = new EventTrigger.TriggerEvent();
            onPointerEnter.AddListener(OnPointerEnter);

            eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = onPointerEnter,
            });

            var onPointerExit = new EventTrigger.TriggerEvent();
            onPointerExit.AddListener(OnPointerExit);

            eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
                callback = onPointerExit,
            });
        }

        private void AddEmptyEventTrigger()
        {
            eventTrigger = NavigatableObj.GetComponent<EventTrigger>();
            if (eventTrigger != null)
            {
                eventTrigger.triggers.Clear();
            }
            else
            {
                eventTrigger = NavigatableObj.AddComponent<EventTrigger>();
            }
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            if (onSelectAudio != null && onSelectAudio.TryGetRandomClip(out var clip))
            {
                audioPlayer?.PlayOneShot(clip);
            }

            onSelect?.Invoke();
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();
        }

        public virtual void OnPointerEnter(BaseEventData eventData)
        {
            onPointerEnter?.Invoke();
        }

        public virtual void OnPointerExit(BaseEventData eventData)
        {
            onPointerExit?.Invoke();
        }
    }
}