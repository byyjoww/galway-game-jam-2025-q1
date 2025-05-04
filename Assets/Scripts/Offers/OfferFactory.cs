using SLS.Core.Extensions;
using SLS.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scamazon.Offers
{
    public class OfferFactory
    {
        [System.Serializable]
        public class Config
        {
            public string[] headers = new string[]
            {
                "Limited Time Offer!",
                "Great Deal!",
            };

            public string[] realUrls = new string[]
            {
                "https://www.azamon.com"
            };

            public string[] fakeUrls = new string[]
            {
                "https://www.azamon.com"
            };

            public string[] fakeHyperlinks = new string[]
            {
                "azamon.com"
            };

            public int legitMinRating = 100;
            public int legitMaxRating = 1000;
            public int legitMinStars = 6;
            public int legitMaxStars = 10;
            public int legitMinReviews = 2;
            public int legitMaxReviews = 3;

            public string[] legitReviewers = new string[]
            {
                "Timmy",
                "RogerMike93",
                "Timmy",
                "RogerMike93",
            };

            public string[] legitReviews = new string[]
            {
                "I like it lots!",
                "I like it lots!",
                "Very good product, recommended.",
                "Very good product, recommended.",
            };

            public int fakeMinRating = 0;
            public int fakeMaxRating = 20;
            public int fakeMinStars = 0;
            public int fakeMaxStars = 6;
            public int fakeMinReviews = 0;
            public int fakeMaxReviews = 2;

            public string[] fakeReviewers = new string[]
            {
                "Timmy",
                "RogerMike93",
                "Timmy",
                "RogerMike93",
            };

            public string[] fakeReviews = new string[]
            {
                "I like it lots!",
                "I like it lots!",
                "Very good product, recommended.",
                "Very good product, recommended.",
            };
        }

        private IEnumerable<IProduct> products = default;
        private Config config = default;

        private enum FakeIndicator
        {
            URL,
            Reviews,
            Delivery,
            Price,
        }

        public OfferFactory(IEnumerable<IProduct> products, Config config)
        {
            this.products = products;
            this.config = config;
        }

        public Offer CreateOffer()
        {
            var template = products.Random();
            var offerType = GenerateOfferType();
            var indicator = GetFakeIndicator(offerType);
            var product = GenerateProduct(template, offerType, indicator);
            var url = GenerateURL(product, offerType, indicator);
            var headers = GenerateHeaders(product, offerType, indicator);
            var priceAndVariance = GeneratePrice(product, offerType, indicator);
            var hyperlink = GenerateHyperlink(product, offerType, url, indicator);
            var rating = GenerateRating(product, offerType, indicator);
            var delivery = GenerateDeliveryDate(product, offerType, indicator);
            var duration = GenerateOfferDuration(product, offerType, priceAndVariance.Item2, indicator);

            return new Offer
            {
                ID = Guid.NewGuid().ToString(),
                Product = product,
                Price = priceAndVariance.Item1,
                HyperlinkText = hyperlink,
                Url = url,
                Rating = rating,
                Delivery = delivery,
                Duration = duration,
                Type = offerType,
                ImageHeader1 = headers.Item1,
                ImageHeader2 = headers.Item2,
            };
        }

        private OfferType GenerateOfferType()
        {
            if (RNG.RollSuccess(0.4f))
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

        private FakeIndicator[] GetFakeIndicator(OfferType type)
        {
            if (type == OfferType.Legit)
            {
                return new FakeIndicator[0];
            }

            List<FakeIndicator> possible;
            if (type == OfferType.Virus)
            {
                possible = new List<FakeIndicator>()
                {
                    FakeIndicator.URL,
                    FakeIndicator.Reviews,
                    FakeIndicator.Delivery,
                    FakeIndicator.Price,
                };
            }
            else
            {
                possible = new List<FakeIndicator>()
                {
                    FakeIndicator.URL,
                    FakeIndicator.Reviews,
                    FakeIndicator.Delivery,
                    FakeIndicator.Price,
                };
            }

            int numOfIndicators = RNG.RollBetween(1, 3);
            var indicators = new List<FakeIndicator>();
            for (int i = 0; i < numOfIndicators; i++)
            {
                var selected = possible.RandomOrDefault();
                possible.Remove(selected);
                indicators.Add(selected);
            }

            return indicators.ToArray();
        }

        private Product GenerateProduct(IProduct product, OfferType type, FakeIndicator[] indicators)
        {
            return new Product
            {
                Name = product.ProductName,
                Description = product.Description,
                Icon = product.Icon,
                Score = product.Score,
                BasePrice = product.Score * 1.00f,
            };
        }

        private Rating GenerateRating(Product product, OfferType type, FakeIndicator[] indicators)
        {
            if (type == OfferType.Legit || !indicators.Contains(FakeIndicator.Reviews))
            {
                var reviews = new List<Review>();
                var reviewerOpts = config.legitReviewers.ToList();
                var reviewOpts = config.legitReviews.ToList();
                int numOfReviews = RNG.RollBetween(config.legitMinReviews, config.legitMaxReviews);
                for (int i = 0; i < numOfReviews; i++)
                {
                    var selectedReviewer = reviewerOpts.RandomOrDefault();
                    var selectedReview = reviewOpts.RandomOrDefault();
                    reviewerOpts.Remove(selectedReviewer);
                    reviewOpts.Remove(selectedReview);
                    reviews.Add(new Review
                    {
                        Reviewer = selectedReviewer,
                        Text = selectedReview,
                    });
                }

                return new Rating
                {
                    NumOfReviews = RNG.RollBetween(config.legitMinRating, config.legitMaxRating),
                    Stars = RNG.RollBetween(config.legitMinStars, config.legitMaxStars),
                    Reviews = reviews.ToArray(),
                };
            }
            else if (RNG.RollSuccess(0.9f))
            {
                var reviews = new List<Review>();
                var reviewerOpts = config.fakeReviewers.ToList();
                var reviewOpts = config.fakeReviews.ToList();
                int numOfReviews = RNG.RollBetween(config.fakeMinReviews, config.fakeMaxReviews);
                for (int i = 0; i < numOfReviews; i++)
                {
                    var selectedReviewer = reviewerOpts.RandomOrDefault();
                    var selectedReview = reviewOpts.RandomOrDefault();
                    reviewerOpts.Remove(selectedReviewer);
                    reviewOpts.Remove(selectedReview);
                    reviews.Add(new Review
                    {
                        Reviewer = selectedReviewer,
                        Text = selectedReview,
                    });
                }

                return new Rating
                {
                    NumOfReviews = RNG.RollBetween(config.fakeMinRating, config.fakeMaxRating),
                    Stars = RNG.RollBetween(config.fakeMinStars, config.fakeMaxStars),
                    Reviews = reviews.ToArray(),
                };
            }
            else
            {
                var reviews = new List<Review>();
                var reviewOpts = config.fakeReviews.ToList();
                int numOfReviews = RNG.RollBetween(4, 5);
                for (int i = 0; i < numOfReviews; i++)
                {
                    var selectedReviewer = "bot_";
                    var selectedReview = reviewOpts.RandomOrDefault();
                    reviewOpts.Remove(selectedReview);
                    reviews.Add(new Review
                    {
                        Reviewer = $"{selectedReviewer}{RNG.RollBetween(1000, 5000)}",
                        Text = selectedReview,
                    });
                }

                return new Rating
                {
                    NumOfReviews = RNG.RollBetween(1000, 100000),
                    Stars = RNG.RollBetween(8, 10),
                    Reviews = reviews.ToArray(),
                };
            }
        }

        private (float, float) GeneratePrice(Product product, OfferType type, FakeIndicator[] indicators)
        {
            if (type != OfferType.Legit && indicators.Contains(FakeIndicator.Price))
            {
                return (product.BasePrice * 0.05f, 0.05f);
            }

            float variance = 0.5f;
            if (type != OfferType.Legit)
            {
                variance = 0.8f;
            }

            float min = product.BasePrice * (1 - variance);
            float max = product.BasePrice;
            float value = RNG.RollBetween(min, max);
            float weight = (value - product.BasePrice) / product.BasePrice;
            return (value, weight);
        }

        private string GenerateHyperlink(Product product, OfferType type, string url, FakeIndicator[] indicators)
        {
            if (type == OfferType.Legit || !indicators.Contains(FakeIndicator.URL) || RNG.RollSuccess(0.5f))
            {
                return url.Replace("https://www.", "");
            }

            return config.fakeHyperlinks.RandomOrDefault();
        }

        private string GenerateURL(Product product, OfferType type, FakeIndicator[] indicators)
        {
            return type == OfferType.Legit || !indicators.Contains(FakeIndicator.URL)
                ? config.realUrls.RandomOrDefault() 
                : config.fakeUrls.RandomOrDefault();
        }

        private DateTime GenerateDeliveryDate(Product product, OfferType type, FakeIndicator[] indicators)
        {
            if (indicators.Contains(FakeIndicator.Delivery))
            {
                var pastDays = RNG.RollVariance(500, 0.9f);
                return DateTime.Now.AddDays(-pastDays);
            }

            var date = RNG.RollVariance(6, 0.5f);
            return DateTime.Now.AddDays(date);
        }

        private float GenerateOfferDuration(Product product, OfferType type, float weight, FakeIndicator[] indicators)
        {
            var baseDuration = 15f;
            var weightRange = baseDuration / 3;
            var weightedDuration = baseDuration + (weightRange * weight);
            return RNG.RollVariance(weightedDuration, 0.3f);
        }

        private (string, string) GenerateHeaders(Product product, OfferType type, FakeIndicator[] indicators)
        {
            var options = config.headers.ToList();
            var opt1 = options.Random();
            options.Remove(opt1);
            var opt2 = options.Random();
            return (opt1, opt2);
        }
    }
}
