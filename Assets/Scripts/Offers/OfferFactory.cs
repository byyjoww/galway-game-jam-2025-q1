using SLS.Core.Extensions;
using SLS.Core.Tools;
using System;
using System.Collections.Generic;

namespace Scamazon.Offers
{
    public class OfferFactory
    {
        private IEnumerable<IProduct> products = default;

        public OfferFactory(IEnumerable<IProduct> products)
        {
            this.products = products;
        }

        public Offer CreateOffer()
        {
            var template = products.Random();
            var offerType = GenerateOfferType();
            var product = GenerateProduct(template, offerType);
            var url = GenerateURL(product, offerType);
            var headers = GenerateHeaders(product, offerType);
            return new Offer
            {
                ID = Guid.NewGuid().ToString(),
                Product = product,
                Price = GeneratePrice(product, offerType),
                HyperlinkText = GenerateHyperlink(product, offerType, url),
                Url = url,
                Rating = GenerateRating(product, offerType),
                Delivery = GenerateDeliveryDate(product, offerType),
                Duration = GenerateOfferDuration(product, offerType),
                Type = offerType,
                ImageHeader1 = headers.Item1,
                ImageHeader2 = headers.Item2,
            };
        }

        private OfferType GenerateOfferType()
        {
            if (RNG.RollSuccess(0.35f))
            {
                if (RNG.RollSuccess(0.5f))
                {
                    return OfferType.Scam;
                }
                else
                {
                    return OfferType.Virus;
                }
            }

            return OfferType.Legit;
        }

        private Product GenerateProduct(IProduct product, OfferType type)
        {
            return new Product
            {
                Name = product.ProductName,
                Description = product.Description,
                Icon = product.Icon,
                Score = product.Score,
            };
        }

        private Rating GenerateRating(Product product, OfferType type)
        {
            return new Rating
            {
                NumOfReviews = 20,
                Stars = 4,
                Reviews = new Review[]
                {
                    new Review
                    {
                        Reviewer = "Timmy",
                        Text = "I like it lots!",
                    },
                    new Review
                    {
                        Reviewer = "RogerMike93",
                        Text = "Very good product, recommended.",
                    },
                },
            };
        }

        private float GeneratePrice(Product product, OfferType type)
        {
            var basePrice = product.Score * 1.00f;
            float variance = 0.5f;
            if (type != OfferType.Legit)
            {
                variance = 0.8f;
            }

            return RNG.RollVariance(basePrice, variance);
        }

        private string GenerateHyperlink(Product product, OfferType type, string url)
        {
            return url.Replace("https://www.", "");
        }

        private string GenerateURL(Product product, OfferType type)
        {
            return "https://www.amazin.com";
        }

        private DateTime GenerateDeliveryDate(Product product, OfferType type)
        {
            return DateTime.Now.AddDays(7);
        }

        private float GenerateOfferDuration(Product product, OfferType type)
        {
            return 15f;
        }

        private (string, string) GenerateHeaders(Product product, OfferType type)
        {
            return ("Limited Time Offer!", "Great Deal!");
        }
    }
}
