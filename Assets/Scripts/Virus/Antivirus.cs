using Scamazon.Cursor;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.Virus
{
    public class Antivirus : IDisposable
    {
        private PlayerCursor cursor = default;

        private Virus current = default;

        public event UnityAction<Virus> OnVirusStarted;
        public event UnityAction<Virus> OnVirusQuarantined;

        public Antivirus(PlayerCursor cursor)
        {
            this.cursor = cursor;
        }

        public void CreateVirus()
        {
            current?.Dispose();
            
            current = Create();
            current.OnEnd += OnVirusEnded;
            current.Execute();
            OnVirusStarted?.Invoke(current);
        }

        private Virus Create()
        {
            return new FreezeVirus(cursor, 5f);
        }

        private void OnVirusEnded()
        {
            current.OnEnd -= OnVirusEnded;
            OnVirusQuarantined?.Invoke(current);
            current = null;
        }

        public void Dispose()
        {
            if (current != null)
            {
                current.OnEnd -= OnVirusEnded;
                current?.Dispose();
                current = null;
            }            
        }
    }
}
