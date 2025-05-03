using Scamazon.App;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Scamazon.UI
{
    public class TimeLimitView : View
    {
        public struct PresenterModel
        {
            public string TimeText { get; set; }
            public string TimeRemaining { get; set; }
        }

        [SerializeField] private TMP_Text timeText = default;
        [SerializeField] private TMP_Text time = default;

        public void Setup(PresenterModel model)
        {
            if (timeText != null)
            {
                timeText.text = model.TimeText;
            }
            
            time.text = model.TimeRemaining;
        }
    }
}