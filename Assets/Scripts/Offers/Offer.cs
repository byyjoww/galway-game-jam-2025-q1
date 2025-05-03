using System;

namespace Scamazon.Offers
{
    public class Offer
    {
        public string ID { get; }
        public Product Product { get; }
        public float Price { get; }
        public string HyperlinkText { get; }
        public string Url { get; }
        public Rating Rating { get; }
        public DateTime Delivery { get; }
        public OfferType Type { get; }

        // Image
        public string ImageHeader1 { get; }
        public string ImageHeader2 { get; }
    }
}
