using Scamazon.Virus;
using SLS.Core.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Scamazon.Offers
{
    public class Marketplace : IDisposable
    {
        public class TimedOffer
        {
            public Offer Offer;
            public ITimer Timer;
        }

        private const float INITIAL_OFFER_CREATION_DELAY = 2f;
        private const float STANDARD_OFFER_CREATION_DELAY = 5f;

        private OfferFactory offerFactory = default;
        private Antivirus antivirus = default;
        private RandomEventTimer creationTimer = default;
        private ITimer delay = default;
        private float currency = default;
        private TimedOffer[] offers = default;
        private List<Offer> purchases = default;
        private List<Offer> skips = default;
        private float startingCurrency = default;

        public int Score => purchases.Where(x => x.Type == OfferType.Legit).Sum(x => x.Product.Score);
        public float CurrencyAmount => currency;
        public IReadOnlyList<Offer> Offers => offers.Select(x => x.Offer).ToList();

        public UnityAction<Offer> OnPurchase;
        public UnityAction OnValueChanged;

        public Marketplace(OfferFactory offerFactory, Antivirus antivirus, float startingCurrency)
        {
            this.offerFactory = offerFactory;
            this.antivirus = antivirus;
            this.startingCurrency = startingCurrency;
            Reset();

            creationTimer = new RandomEventTimer(CreateOffer, new RandomEventTimer.Config
            {
                Interval = STANDARD_OFFER_CREATION_DELAY,
                Randomness = 0.5f,
            });

            delay = Timer.CreateScaledTimer(TimeSpan.FromSeconds(INITIAL_OFFER_CREATION_DELAY));
            delay.OnEnd.AddListener(delegate
            {
                CreateOffer();
                creationTimer.Start();
            });
        }

        public void StartShowingOffers()
        {
            delay.Restart();
        }

        public void StopShowingOffers()
        {
            delay?.Reset();
            creationTimer?.Stop();
        }

        public void Reset()
        {
            currency = startingCurrency;
            purchases = new List<Offer>();
            skips = new List<Offer>();
            offers = new TimedOffer[]
            {
                new TimedOffer{ Offer = new Offer{ ID = $"{0}", Type = OfferType.None } },
                new TimedOffer{ Offer = new Offer{ ID = $"{1}", Type = OfferType.None } },
                new TimedOffer{ Offer = new Offer{ ID = $"{2}", Type = OfferType.None } },
                new TimedOffer{ Offer = new Offer{ ID = $"{3}", Type = OfferType.None } },
            };
            OnValueChanged?.Invoke();
        }

        public void Purchase(string offerID)
        {
            Offer offer = ClearAndReturnOffer(offerID);
            purchases.Add(offer);

            if (offer.Type == OfferType.Virus)
            {
                antivirus.CreateVirus();
            }
            else
            {
                currency -= offer.Price;
            }

            OnPurchase?.Invoke(offer);
            OnValueChanged?.Invoke();
        }

        public void Decline(string offerID)
        {
            Offer offer = ClearAndReturnOffer(offerID);
            skips.Add(offer);
            OnValueChanged?.Invoke();
        }

        private Offer ClearAndReturnOffer(string offerID)
        {
            for (int i = 0; i < offers.Length; i++)
            {
                var off = offers[i];
                if (off != null && off.Offer.ID == offerID)
                {
                    offers[i] = new TimedOffer
                    {
                        Offer = new Offer
                        {
                            ID = $"{i}",
                            Type = OfferType.None
                        },
                    };
                    return off.Offer;
                }
            }

            return null;
        }

        private void CreateOffer()
        {
            var offer = offerFactory.CreateOffer();
            Create(offer);
        }

        private void Create(Offer offer)
        {
            if (TryGetFirstAvailableIndex(out int index))
            {
                var timer = Timer.CreateScaledTimer(TimeSpan.FromSeconds(offer.Duration));
                timer.OnEnd.AddListener(delegate
                {
                    timer?.Dispose();
                    Decline(offer.ID);
                });

                offers[index] = new TimedOffer 
                {
                    Offer = offer,
                    Timer = timer,
                };

                OnValueChanged?.Invoke();
                timer.Start();
            }
        }

        private bool TryGetFirstAvailableIndex(out int index)
        {
            for (int i = 0; i < offers.Length; i++)
            {
                if (offers[i].Offer.Type == OfferType.None)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        public void Dispose()
        {
            creationTimer?.Stop();
            creationTimer?.Dispose();
            delay?.Stop();
            delay?.Dispose();
        }
    }
}
