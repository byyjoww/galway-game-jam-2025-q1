using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.UI
{
    public class EndGameView : View
    {
        public struct PresenterModel
        {
            public string TimeRemaining { get; set; }
            public string BudgetRemaining { get; set; }
            public string TotalScore { get; set; }
            public UnityAction OnContinue { get; set; }
        }

        [SerializeField] private TMP_Text time = default;
        [SerializeField] private TMP_Text budget = default;
        [SerializeField] private TMP_Text score = default;
        [SerializeField] private ButtonViewBase continueBtn = default;

        public void Setup(PresenterModel model)
        {
            time.text = model.TimeRemaining;
            budget.text = model.BudgetRemaining;
            score.text = model.TotalScore;
            SetButtonAction(continueBtn, model.OnContinue);
        }
    }
}