using Scamazon.Offers;
using Scamazon.Timers;
using UnityEngine.SceneManagement;

namespace Scamazon.UI
{
    public class EndGameViewController : ViewController<EndGameView, App.App>
    {
        private TimeLimit timeLimit = default;
        private Marketplace marketplace = default;

        public EndGameViewController(EndGameView view, App.App model, 
            TimeLimit timeLimit, Marketplace marketplace) : base(view, model)
        {
            this.timeLimit = timeLimit;
            this.marketplace = marketplace;
        }

        public override void Init()
        {
            model.OnGameEnded += UpdateView;
            HideView();
        }

        private void UpdateView()
        {
            ShowView();

            var time = timeLimit.RemainingTime.ToString(@"mm\:ss");
            var budget = marketplace.CurrencyAmount.ToString("F2");
            view.Setup(new EndGameView.PresenterModel
            {
                TimeRemaining = $"Time Remaining: {time}",
                BudgetRemaining = $"${budget}",
                TotalScore = $"{marketplace.Score}",
                OnContinue = delegate
                {
                    HideView();
                    model.ResetGame();
                },
            });
        }

        public override void Dispose()
        {
            model.OnGameEnded -= UpdateView;
        }
    }
}