using System.Collections.Generic;
using UnityEngine.Events;

namespace Scamazon.Offers
{
    public class Marketplace
    {
        private float currency = default;
        private Offer[] offers = new Offer[4];
        private List<Offer> purchases = new List<Offer>();
        private List<Offer> skips = new List<Offer>();

        public float CurrencyAmount { get; }
        public IReadOnlyList<Offer> Offers { get; }

        public UnityAction OnValueChanged;

        public Marketplace(float startingCurrency)
        {
            currency = startingCurrency;
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
                    offers[i] = null;
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
                if (offers[i] != null)
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
