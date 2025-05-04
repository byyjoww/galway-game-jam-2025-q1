using Scamazon.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scamazon.Virus
{
    public class FreezeVirus : Virus
    {
        private PlayerCursor cursor = default;
        private EventSystem eventSystem = default;

        public FreezeVirus(PlayerCursor cursor, float duration) : base(duration)
        {
            this.cursor = cursor;
            this.eventSystem = EventSystem.current;
        }

        protected override void OnStarted()
        {
            cursor.SetFrozenCursor();
            eventSystem.enabled = false;
        }

        protected override void OnEnded()
        {
            if (eventSystem != null)
            {
                eventSystem.enabled = true;
            }

            cursor?.SetOriginalCursor();            
        }
    }
}
