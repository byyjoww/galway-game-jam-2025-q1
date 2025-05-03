using Scamazon.Offers;
using System;
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
                Offers = model.Offers.Select((x, i) => CreateOffers(x, i)),
            });
        }

        private OfferView.PresenterModel CreateOffers(Offer offer, int index)
        {
            if (offer.ID == "")
            {
                return new OfferView.PresenterModel
                {
                    OfferID = offer.ID,
                    OfferIndex = index,
                };
            }

           return new OfferView.PresenterModel
           {
               OfferID = offer.ID,
               OfferIndex = index,
               ProductName = offer.Product.Name,
               ProductDescription = offer.Product.Description,
               HyperlinkText = offer.HyperlinkText,
               DeliveryDate = offer.Delivery.ToString("d"),
               URL = offer.Url,
               ProductIcon = offer.Product.Icon,
               ImageHeader1 = offer.ImageHeader1,
               ImageHeader2 = offer.ImageHeader2,
               Stars = offer.Rating.Stars,
               NumOfReviews = $"({offer.Rating.NumOfReviews})",
               Reviews = new ReviewView.PresenterModel[0],
               CanBuy = model.CurrencyAmount >= offer.Price,
               OnBuy = delegate
               {
                   model.Purchase(offer.ID);
               },
               OnSkip = delegate
               {
                   model.Decline(offer.ID);
               },
           };
        }

        public override void Dispose()
        {
            model.OnValueChanged -= ShowOffers;
        }
    }
}