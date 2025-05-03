using System;

namespace Scamazon.Offers
{
    public class Offer
    {
        public string ID { get; set; }
        public Product Product { get; set; }
        public float Price { get; set; }
        public string HyperlinkText { get; set; }
        public string Url { get; set; }
        public Rating Rating { get; set; }
        public DateTime Delivery { get; set; }
        public OfferType Type { get; set; }
        public float Duration { get; set; }

        // Image
        public string ImageHeader1 { get; set; }
        public string ImageHeader2 { get; set; }
    }
}
