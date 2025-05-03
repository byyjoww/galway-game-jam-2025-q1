using Scamazon.Audio;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.UI
{
    public class NotificationView : View
    {
        public struct PresenterModel
        {
            public string NotificationText { get; set; }
            public ISoundlist NotificationAudio { get; set; }
            public Vector2 Position { get; set; }
            public ScreenTextPopupView.Style Style { get; set; }

            public bool IsPopupText { get; set; }
            public Func<GameObject, LTDescr> CreateScreenNotificationTweenFunc { get; set; }
        }

        [SerializeField] private Canvas canvas = default;
        [SerializeField] private TMP_Text notification = default;
        [SerializeField] private ScreenTextPopupView textPopup = default;

        private Vector3 originPosition = default;
        private Quaternion originRotation = default;
        private Vector3 originScale = default;
        
        private int? currentTweenId = default;

        public override void Init()
        {
            originPosition = notification.transform.position;
            originRotation = notification.transform.rotation;
            originScale = notification.transform.localScale;
            notification.gameObject.SetActive(false);
            base.Init();
        }

        public void Setup(PresenterModel model)
        {
            if (model.IsPopupText)
            {
                CreatePopupTextNotification(model);
            }
            else
            {
                CreateOnScreenNotification(model);
                if (currentTweenId.HasValue)
                {
                    LeanTween.cancel(currentTweenId.Value);
                }

                var tween = model.CreateScreenNotificationTweenFunc?.Invoke(notification.gameObject);
                currentTweenId = tween.id;
            }

            if (model.NotificationAudio != null && model.NotificationAudio.TryGetRandomClip(out var clip))
            {
                audioPlayer.PlayOneShot(clip);
            }
        }

        private void CreatePopupTextNotification(PresenterModel model)
        {
            if (model.NotificationText != null && model.NotificationText.Length > 0)
            {
                textPopup.Create(canvas, transform, model.Position, model.NotificationText, model.Style);
            }
        }

        private void CreateOnScreenNotification(PresenterModel model)
        {
            if (model.NotificationText != null && model.NotificationText.Length > 0)
            {
                notification.gameObject.SetActive(false);
                notification.transform.position = originPosition;
                notification.transform.rotation = originRotation;
                notification.transform.localScale = originScale;
                notification.text = model.NotificationText;
                notification.gameObject.SetActive(true);
            }
        }
    }
}