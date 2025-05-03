using Scamazon.Offers;
using Scamazon.Timers;
using Scamazon.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scamazon.App
{
    public class App : MonoBehaviour
    {
        [SerializeField] private TimeLimit.Config timeConfig = default;
        [SerializeField] private float startingCurrency = default;

        [Header("Views")]
        [SerializeField] private NotificationView notificationView = default;
        [SerializeField] private TimeLimitView timeLimitView = default;
        [SerializeField] private OffersView offersView = default;

        // Models
        private TimeLimit timeLimit = default;
        private Marketplace marketplace = default;

        // View controllers
        private NotificationViewController notificationViewController = default;
        private TimeLimitViewController timeLimitViewController = default;
        private OffersViewController offersViewController = default;

        private void Start()
        {
            timeLimit = new TimeLimit(timeConfig);
            marketplace = new Marketplace(startingCurrency);

            CreateViewControllers();

            timeLimit.Start();
        }

        private void CreateViewControllers()
        {
            notificationViewController = new NotificationViewController(notificationView);
            timeLimitViewController = new TimeLimitViewController(timeLimitView, timeLimit, notificationViewController);
            offersViewController = new OffersViewController(offersView, marketplace);

            notificationViewController?.Init();
            timeLimitViewController?.Init();
            offersViewController?.Init();
        }

        [ContextMenu("Create Offer")]
        private void CreateRandomOffer()
        {
            marketplace.Create(new Offer
            {
                ID = Guid.NewGuid().ToString(),
                Product = new Product
                {
                    Name = "Pen",
                    Description = "This is a pen.",
                    Icon = null,
                    Score = 10,
                },
                Price = 1.99f,
                HyperlinkText = "amazin.com",
                Url = "amazin.com",
                Rating = new Rating
                {
                    NumOfReviews = 20,
                    Stars = 4,
                    Reviews = new Review[]
                    {
                        new Review
                        {
                            Reviewer = "Little Timmy",
                            Text = "Me Likey",
                        },
                    },
                },
                Delivery = DateTime.Now.AddDays(7),
                Type = OfferType.Legit,
                ImageHeader1 = "Limited Time Offer!",
                ImageHeader2 = "Great Deal!",
            });
        }

        private void OnDestroy()
        {
            offersViewController?.Dispose();
            timeLimitViewController?.Dispose();
            notificationViewController?.Dispose();

            timeLimit?.Dispose();
        }
    }
}
