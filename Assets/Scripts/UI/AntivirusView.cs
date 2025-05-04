using Scamazon.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scamazon.UI
{
    public class AntivirusView : View
    {
        [SerializeField] private float timeToMove = 1f;
        [SerializeField] private float timeVisible = 2f;
        [SerializeField] private Image detected = default;
        [SerializeField] private Image quarantine = default;
        [SerializeField] private Transform originTransform = default;
        [SerializeField] private Transform destinationTransform = default;
        [SerializeField] private GameObject freezePanel = default;

        [Header("Audio")]
        [SerializeField] private SoundlistSO onVirusDetected = default;
        [SerializeField] private SoundlistSO onVirusQuarantined = default;

        private int? tweenId = default;

        private Vector3 origin => originTransform.position;
        private Vector3 destination => destinationTransform.position;

        public GameObject FreezePanel => freezePanel;
        
        private void Awake()
        {
            quarantine.transform.position = origin;
            detected.transform.position = origin;
        }

        [ContextMenu("Show Detected")]
        public void ShowDetected()
        {
            if (tweenId.HasValue)
            {
                LeanTween.cancel(tweenId.Value);
                quarantine.transform.position = origin;
                detected.transform.position = origin;
            }

            PlayOneShotAudio(onVirusDetected);
            detected.transform.position = origin;
            var tween = LeanTween
                .moveY(detected.gameObject, destination.y, timeToMove)
                .setOnComplete(() =>
                {                    
                    LeanTween.cancel(tweenId.Value);
                    detected.transform.position = destination;

                    var tween = LeanTween
                        .moveY(detected.gameObject, origin.y, timeToMove)
                        .setDelay(timeVisible)
                        .setOnComplete(() =>
                        {
                            LeanTween.cancel(tweenId.Value);
                            detected.transform.position = origin;                            
                        });

                    tweenId = tween.id;
                });

            tweenId = tween.id;
        }

        [ContextMenu("Show Quarantined")]
        public void ShowQuarantined()
        {
            if (tweenId.HasValue)
            {
                LeanTween.cancel(tweenId.Value);
                quarantine.transform.position = origin;
                detected.transform.position = origin;
            }

            PlayOneShotAudio(onVirusQuarantined);
            quarantine.transform.position = origin;
            var tween = LeanTween
                .moveY(quarantine.gameObject, destination.y, timeToMove)
                .setOnComplete(() =>
                {
                    LeanTween.cancel(tweenId.Value);
                    quarantine.transform.position = destination;
                    
                    var tween = LeanTween
                        .moveY(quarantine.gameObject, origin.y, timeToMove)
                        .setDelay(timeVisible)
                        .setOnComplete(() =>
                        {
                            LeanTween.cancel(tweenId.Value);
                            quarantine.transform.position = origin;                            
                        });

                    tweenId = tween.id;
                });

            tweenId = tween.id;
        }
    }
}