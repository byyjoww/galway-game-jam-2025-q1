using Scamazon.Cursor;
using SLS.Core.Tools;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Virus
{
    public class Antivirus : IDisposable
    {
        private MonoBehaviour coroutineStarter = default;
        private RectTransform popupRoot = default;
        private GameObject[] popups = default; 
        private PlayerCursor cursor = default;

        public Virus Current { get; private set; }

        public event UnityAction<Virus> OnVirusDetected;
        public event UnityAction<Virus> OnVirusQuarantined;

        public Antivirus(PlayerCursor cursor, RectTransform popupRoot, GameObject[] popups, MonoBehaviour coroutineStarter)
        {
            this.cursor = cursor;
            this.popupRoot = popupRoot;
            this.popups = popups;
            this.coroutineStarter = coroutineStarter;
        }

        public void CreateVirus()
        {
            Current?.Dispose();

            Current = Create();
            Current.OnEnd += OnVirusEnded;
            Current.Execute();
            OnVirusDetected?.Invoke(Current);
        }

        public void Reset()
        {
            Current?.Dispose();
        }

        private Virus Create()
        {
            return RNG.RollSuccess(0.5f)
                ? new FreezeVirus(cursor, 10f)
                : new PopupVirus(coroutineStarter, popupRoot, popups, 3f);
        }

        private void OnVirusEnded()
        {
            Current.OnEnd -= OnVirusEnded;
            OnVirusQuarantined?.Invoke(Current);
            Current = null;
        }

        public void Dispose()
        {
            if (Current != null)
            {
                Current.OnEnd -= OnVirusEnded;
                Current?.Dispose();
                Current = null;
            }            
        }
    }
}
