using System.Collections.Generic;
using UnityEngine.Events;

namespace Scamazon.Offers
{
    public class Marketplace
    {
        private float currency = default;
        private Offer[] offers = default;
        private List<Offer> purchases = default;
        private List<Offer> skips = default;

        public float CurrencyAmount => currency;
        public IReadOnlyList<Offer> Offers => offers;

        public UnityAction OnValueChanged;

        public Marketplace(float startingCurrency)
        {
            currency = startingCurrency;
            purchases = new List<Offer>();
            skips = new List<Offer>();
            offers = new Offer[]
            {
                new Offer{ ID = $"{0}", Type = OfferType.None },
                new Offer{ ID = $"{1}", Type = OfferType.None },
                new Offer{ ID = $"{2}", Type = OfferType.None },
                new Offer{ ID = $"{3}", Type = OfferType.None },
            };
        }

        public void Purchase(string offerID)
        {
            Offer offer = ClearAndReturnOffer(offerID);
            currency -= offer.Price;
            purchases.Add(offer);
        }

        public void Decline(string offerID)
        {
            Offer offer = ClearAndReturnOffer(offerID);
            skips.Add(offer);
        }

        private Offer ClearAndReturnOffer(string offerID)
        {
            for (int i = 0; i < offers.Length; i++)
            {
                var off = offers[i];
                if (off != null && off.ID == offerID)
                {
                    offers[i] = new Offer { ID = $"{i}", Type = OfferType.None };
                    OnValueChanged?.Invoke();
                    return off;
                }
            }

            return null;
        }

        public void Create(Offer offer)
        {
            if (TryGetFirstAvailableIndex(out int index))
            {
                offers[index] = offer;
                OnValueChanged?.Invoke();
            }
        }

        private bool TryGetFirstAvailableIndex(out int index)
        {
            for (int i = 0; i < offers.Length; i++)
            {
                if (offers[i].Type == OfferType.None)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}
