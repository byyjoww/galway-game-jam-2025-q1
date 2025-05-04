using SLS.Core.Extensions;
using SLS.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scamazon.Offers
{
    public class OfferFactory
    {
        private IEnumerable<IProduct> products = default;

        private string[] headers = new string[]
        {
            "Limited Time Offer!",
            "Great Deal!",
        };

        private string[] realUrls = new string[]
        {
            "https://www.azamon.com"
        };

        private string[] fakeUrls = new string[]
        {
            "https://www.azamon.com"
        };

        private string[] fakeHyperlinks = new string[]
        {
            "azamon.com"
        };
                
        private int legitMinRating = 100;
        private int legitMaxRating = 1000;
        private int legitMinStars = 6;
        private int legitMaxStars = 10;
        private int legitMinReviews = 2;
        private int legitMaxReviews = 3;

        private string[] legitReviewers = new string[]
        {
            "Timmy",
            "RogerMike93",
            "Timmy",
            "RogerMike93",
        };

        private string[] legitReviews = new string[]
        {
            "I like it lots!",
            "I like it lots!",
            "Very good product, recommended.",
            "Very good product, recommended.",
        };

        private int fakeMinRating = 0;
        private int fakeMaxRating = 20;
        private int fakeMinStars = 0;
        private int fakeMaxStars = 6;
        private int fakeMinReviews = 0;
        private int fakeMaxReviews = 2;

        private string[] fakeReviewers = new string[]
        {
            "Timmy",
            "RogerMike93",
            "Timmy",
            "RogerMike93",
        };

        private string[] fakeReviews = new string[]
        {
            "I like it lots!",
            "I like it lots!",
            "Very good product, recommended.",
            "Very good product, recommended.",
        };

        private enum FakeIndicator
        {
            URL,
            Reviews,
            Delivery,
            Price,
        }

        public OfferFactory(IEnumerable<IProduct> products)
        {
            this.products = products;
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
                var reviewerOpts = legitReviewers.ToList();
                var reviewOpts = legitReviews.ToList();
                int numOfReviews = RNG.RollBetween(legitMinReviews, legitMaxReviews);
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
                    NumOfReviews = RNG.RollBetween(legitMinRating, legitMaxRating),
                    Stars = RNG.RollBetween(legitMinStars, legitMaxStars),
                    Reviews = reviews.ToArray(),
                };
            }
            else if (RNG.RollSuccess(0.9f))
            {
                var reviews = new List<Review>();
                var reviewerOpts = fakeReviewers.ToList();
                var reviewOpts = fakeReviews.ToList();
                int numOfReviews = RNG.RollBetween(fakeMinReviews, fakeMaxReviews);
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
                    NumOfReviews = RNG.RollBetween(fakeMinRating, fakeMaxRating),
                    Stars = RNG.RollBetween(fakeMinStars, fakeMaxStars),
                    Reviews = reviews.ToArray(),
                };
            }
            else
            {
                var reviews = new List<Review>();
                var reviewOpts = fakeReviews.ToList();
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

            float value = RNG.RollVariance(product.BasePrice, variance);
            float weight = (value - product.BasePrice) / product.BasePrice;
            return (value, weight);
        }

        private string GenerateHyperlink(Product product, OfferType type, string url, FakeIndicator[] indicators)
        {
            if (type == OfferType.Legit || !indicators.Contains(FakeIndicator.URL) || RNG.RollSuccess(0.5f))
            {
                return url.Replace("https://www.", "");
            }

            return fakeHyperlinks.RandomOrDefault();
        }

        private string GenerateURL(Product product, OfferType type, FakeIndicator[] indicators)
        {
            return type == OfferType.Legit || !indicators.Contains(FakeIndicator.URL)
                ? realUrls.RandomOrDefault() 
                : fakeUrls.RandomOrDefault();
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
            var options = headers.ToList();
            var opt1 = options.Random();
            options.Remove(opt1);
            var opt2 = options.Random();
            return (opt1, opt2);
        }
    }
}
