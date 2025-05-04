using UnityEngine;

namespace Scamazon.Cursor
{
    public class PlayerCursor
    {
        private Texture2D original = default;
        private Texture2D frozen = default;

        public PlayerCursor(Texture2D original, Texture2D frozen)
        {
            this.original = original;
            this.frozen = frozen;
        }

        public void SetOriginalCursor()
        {
            UnityEngine.Cursor.SetCursor(original, Vector2.one, CursorMode.Auto);
        }

        public void SetFrozenCursor()
        {
            UnityEngine.Cursor.SetCursor(frozen, Vector2.one, CursorMode.Auto);
        }
    }
}