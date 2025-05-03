using SLS.Core.Timers;
using System;
using TMPro;
using UnityEngine;

namespace Scamazon.UI
{
    public class ScreenTextPopupView : MonoBehaviour
    {
        public struct Style
        {
            public Color Color;
            public int FontSize;
            public string Format;
            public float FadeInTime;
            public float FadeOutTime;
            public float Delay;
            public Vector2 Movement;
            public Vector2 Offset;
            public Quaternion Rotation;

            public static Style Default => new Style
            {
                Color = Color.white,
                FontSize = 24,
                Format = "{0}",
                FadeInTime = 1f,
                FadeOutTime = 0.7f,
                Delay = 0f,
                Movement = new Vector3(0, 50f), // Move upward in UI space
                Offset = Vector2.zero,
                Rotation = Quaternion.identity,
            };
        }

        [SerializeField] private TextMeshProUGUI textComponent = default;
        [SerializeField] private RectTransform rectTransform = default;

        private ITimer timer = default;

        private float fadeInTime;
        private float fadeOutTime;
        private float delayTime;
        private Vector3 moveVector;
        private bool fadingIn;

        private void Awake()
        {
            if (rectTransform == null) { rectTransform = GetComponent<RectTransform>(); }
            if (textComponent == null) { textComponent = GetComponent<TextMeshProUGUI>(); }
        }

        public ScreenTextPopupView Create(Canvas root, Transform parent, Vector2 screenPosition, string text, Style? style = null)
        {
            Style st = style.GetValueOrDefault(Style.Default);
            RectTransform canvasRect = root.GetComponent<RectTransform>();

            // Convert screen position to UI local position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, root.worldCamera, out Vector2 localPosition);

            ScreenTextPopupView popup = Instantiate(this);
            popup.transform.SetParent(parent);
            popup.rectTransform.anchoredPosition = localPosition + st.Offset;
            popup.ApplyStyle(text, st);
            popup.StartPopup();
            return popup;
        }

        private void ApplyStyle(string _text, Style _style)
        {
            textComponent.fontSize = _style.FontSize;
            textComponent.color = _style.Color;
            textComponent.text = string.Format(_style.Format, _text);

            moveVector = _style.Movement;
            fadeInTime = _style.FadeInTime;
            fadeOutTime = _style.FadeOutTime;
            delayTime = _style.Delay;
        }

        private void HandleMovement()
        {
            rectTransform.anchoredPosition += (Vector2)(moveVector * Time.deltaTime);
            moveVector -= moveVector * 1.5f * Time.deltaTime;
        }

        private void HandleSize()
        {
            if (fadingIn && (1f - (float)timer.Elapsed.TotalSeconds) > fadeInTime * 0.5f)
            {
                float increaseScaleAmount = 0.5f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                float decreaseScaleAmount = 0.5f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
        }

        private void HandleColor()
        {
            float alpha = (1f - (float)timer.Elapsed.TotalSeconds) / fadeOutTime;
            Color textColor = textComponent.color;
            textColor.a = alpha;
            textComponent.color = textColor;
        }

        // ------------------------------------------ FADE IN ------------------------------------------
        private void StartPopup()
        {
            // AppLogger.LogDebug($"[Popup] Start popup");
            timer = Timer.CreateScaledTimer($"POPUP_TEXT_{Guid.NewGuid()}", TimeSpan.FromSeconds(delayTime));
            timer.ID = $"{textComponent.text} text_popup";
            timer.OnEnd.AddListener(StartFadingIn);
            timer.Start();
        }

        private void StartFadingIn()
        {
            // AppLogger.LogDebug($"[Popup] Delay done, start fading in");
            gameObject.SetActive(true);
            fadingIn = true;
            timer.OnTick.RemoveAllListeners();
            timer.OnEnd.RemoveAllListeners();

            timer.OnTick.AddListener(OnFadingInTimerTick);
            timer.OnEnd.AddListener(OnFadingInTimerEnd);
            timer.Interval = TimeSpan.FromSeconds(fadeInTime);
            timer.Restart();
        }

        private void OnFadingInTimerTick(TimeSpan tick)
        {
            HandleMovement();
            HandleSize();
        }

        private void OnFadingInTimerEnd()
        {
            StartFadingOut();
        }

        // ------------------------------------------ FADE OUT -----------------------------------------
        private void StartFadingOut()
        {
            // AppLogger.LogDebug($"[Popup] Fading in done, start fading out");
            fadingIn = false;
            timer.OnTick.RemoveAllListeners();
            timer.OnEnd.RemoveAllListeners();

            timer.OnTick.AddListener(OnFadingOutTimerTick);
            timer.OnEnd.AddListener(OnFadingOutTimerEnd);
            timer.Interval = TimeSpan.FromSeconds(fadeOutTime);
            timer.Restart();
        }

        private void OnFadingOutTimerTick(TimeSpan tick)
        {
            HandleMovement();
            HandleSize();
            HandleColor();
        }

        private void OnFadingOutTimerEnd()
        {
            // AppLogger.LogDebug($"[Popup] Fading out done");
            timer.Dispose();
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            if (rectTransform == null) { rectTransform = GetComponent<RectTransform>(); }
            if (textComponent == null) { textComponent = transform.GetComponent<TextMeshProUGUI>(); }
        }
    }
}