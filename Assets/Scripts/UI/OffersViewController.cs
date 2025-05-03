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
                Offers = model.Offers.Select((x, i) => CreateOffer(x, i)),
            });
        }

        private OfferView.PresenterModel CreateOffer(Offer offer, int index)
        {
            if (offer.Type == OfferType.None)
            {
                return new OfferView.PresenterModel
                {
                    OfferID = offer.ID,
                    OfferIndex = index,
                    IsNull = true,
                };
            }

           return new OfferView.PresenterModel
           {
               OfferID = offer.ID,
               OfferIndex = index,
               IsNull = false,
               ProductName = offer.Product.Name,
               ProductDescription = offer.Product.Description,
               HyperlinkText = offer.HyperlinkText,
               DeliveryDate = offer.Delivery.ToString("d"),
               URL = offer.Url,
               Price = $"${offer.Price}",
               ProductIcon = offer.Product.Icon,
               ImagePrice = $"${offer.Price}",
               ImageHeader1 = offer.ImageHeader1,
               ImageHeader2 = offer.ImageHeader2,
               Stars = offer.Rating.Stars,
               NumOfReviews = $"({offer.Rating.NumOfReviews})",
               Reviews = offer.Rating.Reviews.Select(x => CreateReview(x)).ToArray(),
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

        private ReviewView.PresenterModel CreateReview(Review review)
        {
            return new ReviewView.PresenterModel
            {
                Reviewer = review.Reviewer,
                ReviewText = review.Text,
            };
        }

        public override void Dispose()
        {
            model.OnValueChanged -= ShowOffers;
        }
    }
}