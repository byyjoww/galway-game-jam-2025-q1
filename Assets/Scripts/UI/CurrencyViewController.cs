using Scamazon.Offers;

namespace Scamazon.UI
{
    public class CurrencyViewController : ViewController<CurrencyView, Marketplace>
    {
        public CurrencyViewController(CurrencyView view, Marketplace model) : base(view, model)
        {

        }

        public override void Init()
        {
            model.OnValueChanged += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            view.Setup(new CurrencyView.PresenterModel
            {
                Value = $"${model.CurrencyAmount.ToString("F2")}",
            });
        }

        public override void Dispose()
        {
            model.OnValueChanged -= UpdateView;
        }
    }
}