using Scamazon.Offers;
using System.Linq;

namespace Scamazon.UI
{
    public class OffersViewController : ViewController<OffersView, Marketplace>
    {
        public OffersViewController(OffersView view, Marketplace model) : base(view, model)
        {

        }

        public override void Init()
        {
            model.OnValueChanged += ShowOffers;
            ShowOffers();
        }

        private void ShowOffers()
        {
            view.Setup(new OffersView.PresenterModel
            {
                Offers = model.Offers.Select(x => new OfferView.PresenterModel
                {
                    OfferID = x.ID,
                    ProductName = x.Product.Name,
                    ProductDescription = x.Product.Description,
                    HyperlinkText = x.HyperlinkText,
                    DeliveryDate = x.Delivery.ToString(),
                    URL = x.Url,
                    ProductIcon = x.Product.Icon,
                    ImageHeader1 = x.ImageHeader1,
                    ImageHeader2 = x.ImageHeader2,
                    Stars = x.Rating.Stars,
                    NumOfReviews = $"({x.Rating.NumOfReviews})",
                    Reviews = new ReviewView.PresenterModel[0],
                    CanBuy = model.CurrencyAmount >= x.Price,
                    OnBuy = delegate
                    {
                        model.Purchase(x.ID);
                    },
                    OnSkip = delegate
                    {
                        model.Decline(x.ID);
                    },
                }).ToArray(),
            });
        }

        public override void Dispose()
        {
            model.OnValueChanged -= ShowOffers;
        }
    }
}