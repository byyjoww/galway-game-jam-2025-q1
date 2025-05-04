using System.Collections;
using UnityEngine;
using System;

namespace Scamazon.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelPopIn : MonoBehaviour
    {
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private bool _destroyOnPopOut = false;

        [Header("PopIn Settings")]
        [Range(0, 2)]
        [SerializeField] private float _startingXScale = .8f;
        [Range(0, 2)]
        [SerializeField] private float _startingYScale = .8f;
        [Range(0, 1)]
        [SerializeField] private float _scaleSpeed = 0.2f; // in seconds
        [Range(0, 1)]
        [SerializeField] private float _overshootScaleAmount = 0;
        [Range(0, 1)]
        [SerializeField] private float _overshootReturnSpeed = 0;
        [Range(0, 1)]
        [SerializeField] private float _startingOpacity = 0.5f;
        [Range(0, 1)]
        [SerializeField] private float _opacityChangeSpeed = 0.2f; // in seconds
        [SerializeField] private Vector2 _startPosOffset = new Vector2(0, 0);
        [Range(0, 1)]
        [SerializeField] private float _moveInSpeed = 0;

        [Header("PopOut Settings")]
        [Range(0, 2)]
        [SerializeField] private float _endingXScale = 1.2f;
        [Range(0, 2)]
        [SerializeField] private float _endingYScale = 1.2f;
        [Range(0, 1)]
        [SerializeField] private float _scaleOutSpeed = 0.2f; // in seconds
        [Range(0, 1)]
        [SerializeField] private float _endingOpacity = 0.0f;
        [Range(0, 1)]
        [SerializeField] private float _opacityOutSpeed = 0.2f; // in seconds
        [SerializeField] private Vector2 _endPosOffset = new Vector2(0, 0);
        [Range(0, 1)]
        [SerializeField] private float _moveOutSpeed = 0;

        public event Action AnimationStarted = delegate { };

        private Coroutine _popInRoutine = null;
        private Coroutine _fadeInRoutine = null;
        private Coroutine _moveInRoutine = null;
        private Coroutine _popOutRoutine = null;
        private Coroutine _fadeOutRoutine = null;
        private Coroutine _moveOutRoutine = null;

        private CanvasGroup _canvasGroup = null;
        private RectTransform _panelToAnimate = null;

        private Vector2 _startPos;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _panelToAnimate = GetComponent<RectTransform>();

            SetInitialValues();
        }

        private void OnEnable()
        {
            if (_playOnEnable) { PlayPopIn(); }
        }

        private void OnDisable()
        {
            Stop();
        }

        private void OnDestroy()
        {
            Stop();
        }

        public void PlayPopIn()
        {
            if (_popInRoutine != null) { StopCoroutine(_popInRoutine); }
            _popInRoutine = StartCoroutine(PopInRoutine());

            if (_fadeInRoutine != null) { StopCoroutine(_fadeInRoutine); }
            _fadeInRoutine = StartCoroutine(FadeInRoutine());

            if (_moveInRoutine != null) { StopCoroutine(_moveInRoutine); }
            _moveInRoutine = StartCoroutine(MoveInRoutine());

            AnimationStarted.Invoke();
        }

        public void PlayPopOut()
        {
            _startPos = _panelToAnimate.anchoredPosition;

            if (_popOutRoutine != null) { StopCoroutine(_popOutRoutine); }
            _popOutRoutine = StartCoroutine(PopOutRoutine());

            if (_fadeOutRoutine != null) { StopCoroutine(_fadeOutRoutine); }
            _fadeOutRoutine = StartCoroutine(FadeOutRoutine());

            if (_moveOutRoutine != null) { StopCoroutine(_moveOutRoutine); }
            _moveOutRoutine = StartCoroutine(MoveOutRoutine());

            if (_destroyOnPopOut)
            {
                Destroy(gameObject, 2f);
            }            
        }

        public void Stop()
        {
            if (_popInRoutine != null)
            {
                StopCoroutine(_popInRoutine);
                _popInRoutine = null;
            }

            if (_fadeInRoutine != null)
            {
                StopCoroutine(_fadeInRoutine);
                _fadeInRoutine = null;
            }

            if (_moveInRoutine != null)
            {
                StopCoroutine(_moveInRoutine);
                _moveInRoutine = null;
            }

            if (_popOutRoutine != null)
            {
                StopCoroutine(_popOutRoutine);
                _popOutRoutine = null;
            }

            if (_fadeOutRoutine != null)
            {
                StopCoroutine(_fadeOutRoutine);
                _fadeOutRoutine = null;
            }

            if (_moveOutRoutine != null)
            {
                StopCoroutine(_moveOutRoutine);
                _moveOutRoutine = null;
            }
        }

        private IEnumerator PopInRoutine()
        {
            float newXScale;
            float newYScale;

            float destinationXScale = 1 + _overshootScaleAmount;
            float destinationYScale = 1 + _overshootScaleAmount;

            // growth cycle
            if (_scaleSpeed > 0)
            {
                for (float t = 0; t <= _scaleSpeed; t += Time.unscaledDeltaTime)
                {
                    // adjust current size
                    newXScale = Mathf.Lerp(_startingXScale, destinationXScale, t / _scaleSpeed);
                    newYScale = Mathf.Lerp(_startingYScale, destinationYScale, t / _scaleSpeed);
                    _panelToAnimate.localScale = new Vector3(newXScale, newYScale, 1);
                    yield return null;
                }
            }

            // our starting point was the previous destination
            float overshootStartXScale = destinationXScale;
            float overshootStartYScale = destinationYScale;
            // and our new destination is our standard scale/size
            destinationXScale = 1;
            destinationYScale = 1;
            // bounce back, with overshoot
            if (_overshootReturnSpeed > 0)
            {
                for (float t = 0; t <= _overshootReturnSpeed; t += Time.unscaledDeltaTime)
                {
                    // adjust current size
                    newXScale = Mathf.Lerp(overshootStartXScale, destinationXScale, t / _overshootReturnSpeed);
                    newYScale = Mathf.Lerp(overshootStartYScale, destinationYScale, t / _overshootReturnSpeed);
                    _panelToAnimate.localScale = new Vector3(newXScale, newYScale, 1);
                    yield return null;
                }
            }

            // ensure that we've hit our normal scale, just in case
            _panelToAnimate.localScale = new Vector3(1, 1, 1);
        }

        private IEnumerator PopOutRoutine()
        {
            float newXScale;
            float newYScale;

            float destinationXScale = _endingXScale;
            float destinationYScale = _endingYScale;

            // shrink cycle
            if (_scaleOutSpeed > 0)
            {
                for (float t = 0; t <= _scaleOutSpeed; t += Time.unscaledDeltaTime)
                {
                    // adjust current size
                    newXScale = Mathf.Lerp(1, destinationXScale, t / _scaleOutSpeed);
                    newYScale = Mathf.Lerp(1, destinationYScale, t / _scaleOutSpeed);
                    _panelToAnimate.localScale = new Vector3(newXScale, newYScale, 1);
                    yield return null;
                }
            }

            // ensure that we've hit our target scale, just in case
            _panelToAnimate.localScale = new Vector3(destinationXScale, destinationYScale, 1);
        }

        private IEnumerator FadeInRoutine()
        {
            float newOpacityValue;
            // fade in
            if (_opacityChangeSpeed > 0)
            {
                for (float t = 0; t <= _opacityChangeSpeed; t += Time.unscaledDeltaTime)
                {
                    newOpacityValue = Mathf.Lerp(_startingOpacity, 1, t / _opacityChangeSpeed);
                    _canvasGroup.alpha = newOpacityValue;
                    yield return null;
                }
            }

            // ensure that we've ended fully opaque, just in case
            _canvasGroup.alpha = 1;
        }

        private IEnumerator FadeOutRoutine()
        {
            float newOpacityValue;
            // fade out
            if (_opacityOutSpeed > 0)
            {
                for (float t = 0; t <= _opacityOutSpeed; t += Time.unscaledDeltaTime)
                {
                    newOpacityValue = Mathf.Lerp(1, _endingOpacity, t / _opacityOutSpeed);
                    _canvasGroup.alpha = newOpacityValue;
                    yield return null;
                }
            }

            // ensure that we've ended fully transparent, just in case
            _canvasGroup.alpha = _endingOpacity;
        }

        private IEnumerator MoveInRoutine()
        {
            float startPosX = _startPos.x + _startPosOffset.x;
            float startPosY = _startPos.y + _startPosOffset.y;
            float targetPosX = _startPos.x;
            float targetPosY = _startPos.y;
            // animate
            if (_moveInSpeed > 0)
            {
                float currentPosX = startPosX;
                float currentPosY = startPosY;

                for (float t = 0; t <= _moveInSpeed; t += Time.unscaledDeltaTime)
                {
                    currentPosX = Mathf.Lerp(startPosX, targetPosX, t / _moveInSpeed);
                    currentPosY = Mathf.Lerp(startPosY, targetPosY, t / _moveInSpeed);
                    _panelToAnimate.anchoredPosition = new Vector2(currentPosX, currentPosY);
                    yield return null;
                }
            }

            // ensure we've hit our end point
            _panelToAnimate.anchoredPosition = new Vector2(targetPosX, targetPosY);
        }

        private IEnumerator MoveOutRoutine()
        {
            float startPosX = _startPos.x;
            float startPosY = _startPos.y;
            float targetPosX = _startPos.x + _endPosOffset.x;
            float targetPosY = _startPos.y + _endPosOffset.y;
            // animate
            if (_moveOutSpeed > 0)
            {
                float currentPosX = startPosX;
                float currentPosY = startPosY;

                for (float t = 0; t <= _moveOutSpeed; t += Time.unscaledDeltaTime)
                {
                    currentPosX = Mathf.Lerp(startPosX, targetPosX, t / _moveOutSpeed);
                    currentPosY = Mathf.Lerp(startPosY, targetPosY, t / _moveOutSpeed);
                    _panelToAnimate.anchoredPosition = new Vector2(currentPosX, currentPosY);
                    yield return null;
                }
            }

            // ensure we've hit our end point
            _panelToAnimate.anchoredPosition = new Vector2(targetPosX, targetPosY);
        }

        private void SetInitialValues()
        {
            _panelToAnimate.localScale = new Vector3(_startingXScale, _startingYScale, 1);
            _canvasGroup.alpha = _startingOpacity;
            _startPos = _panelToAnimate.anchoredPosition;
        }
    }
}