using SLS.Core.Extensions;
using System.Collections;
using UnityEngine;

namespace Scamazon.Virus
{
    public class PopupVirus : Virus
    {
        private const int NUM_OF_POPUPS = 10;

        private MonoBehaviour coroutineStarter = default;
        private RectTransform root = default;
        private GameObject[] popups = default;

        public PopupVirus(MonoBehaviour coroutineStarter, RectTransform root, GameObject[] popups, float duration) : base(duration)
        {
            this.coroutineStarter = coroutineStarter;
            this.root = root;
            this.popups = popups;
        }

        protected override void OnStarted()
        {
            coroutineStarter.StartCoroutine(SpawnWithDelay());
        }

        private IEnumerator SpawnWithDelay()
        {
            for (int i = 0; i < NUM_OF_POPUPS; i++)
            {
                var obj = GameObject.Instantiate(popups.Random(), root);
                var pos = GetRandomLocalPositionInside(root);
                (obj.transform as RectTransform).anchoredPosition = pos;
                yield return new WaitForSeconds(0.2f);
            }
        }

        protected override void OnEnded()
        {

        }

        public Vector2 GetRandomLocalPositionInside(RectTransform rectTransform)
        {
            Rect rect = rectTransform.rect;
            float x = Random.Range(rect.xMin, rect.xMax);
            float y = Random.Range(rect.yMin, rect.yMax);
            return new Vector2(x, y);
        }
    }
}
