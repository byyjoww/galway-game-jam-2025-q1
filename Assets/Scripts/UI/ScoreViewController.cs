using Scamazon.Offers;

namespace Scamazon.UI
{
    public class ScoreViewController : ViewController<ScoreView, Marketplace>
    {
        public ScoreViewController(ScoreView view, Marketplace model) : base(view, model)
        {

        }

        public override void Init()
        {
            model.OnValueChanged += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            view.Setup(new ScoreView.PresenterModel
            {
                Value = $"{model.Score}",
            });
        }

        public override void Dispose()
        {
            model.OnValueChanged -= UpdateView;
        }
    }
}