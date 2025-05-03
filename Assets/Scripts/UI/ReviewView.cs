using TMPro;
using UnityEngine;

namespace Scamazon.UI
{
    public class ReviewView : View
    {
        public struct PresenterModel
        {
            public string Reviewer { get; set; }
            public string ReviewText { get; set; }
        }

        [SerializeField] private TMP_Text reviewer = default;
        [SerializeField] private TMP_Text reviewText = default;

        public void Setup(PresenterModel model)
        {
            reviewer.text = model.Reviewer;
            reviewText.text = model.ReviewText;
        }
    }
}