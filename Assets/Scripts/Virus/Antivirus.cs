using Scamazon.Cursor;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Virus
{
    public class Antivirus : IDisposable
    {
        private PlayerCursor cursor = default;

        public Virus Current { get; private set; }

        public event UnityAction<Virus> OnVirusDetected;
        public event UnityAction<Virus> OnVirusQuarantined;

        public Antivirus(PlayerCursor cursor)
        {
            this.cursor = cursor;
        }

        public void CreateVirus()
        {
            Current?.Dispose();

            Current = Create();
            Current.OnEnd += OnVirusEnded;
            Current.Execute();
            OnVirusDetected?.Invoke(Current);
        }

        private Virus Create()
        {
            return new FreezeVirus(cursor, 10f);
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
