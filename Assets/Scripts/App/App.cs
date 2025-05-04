using Scamazon.Offers;
using Scamazon.Timers;
using Scamazon.UI;
using SLS.Core;
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

        [Header("Data")]
        [SerializeField] private Database<ProductSO> products = default;

        [Header("Views")]
        [SerializeField] private NotificationView notificationView = default;
        [SerializeField] private TimeLimitView timeLimitView = default;
        [SerializeField] private OffersView offersView = default;
        [SerializeField] private CurrencyView currencyView = default;
        [SerializeField] private AntivirusView antivirusView = default;

        // Models
        private TimeLimit timeLimit = default;
        private OfferFactory offerFactory = default;
        private Marketplace marketplace = default;
        private Antivirus antivirus = default;

        // View controllers
        private NotificationViewController notificationViewController = default;
        private TimeLimitViewController timeLimitViewController = default;
        private OffersViewController offersViewController = default;
        private CurrencyViewController currencyViewController = default;
        private AntivirusViewController antivirusViewController = default;

        private void Start()
        {
            timeLimit = new TimeLimit(timeConfig);
            offerFactory = new OfferFactory(products.Elements);
            marketplace = new Marketplace(offerFactory, startingCurrency);
            antivirus = new Antivirus();

            CreateViewControllers();

            timeLimit.Start();
            marketplace.StartShowingOffers();
        }

        private void CreateViewControllers()
        {
            notificationViewController = new NotificationViewController(notificationView);
            timeLimitViewController = new TimeLimitViewController(timeLimitView, timeLimit, notificationViewController);
            offersViewController = new OffersViewController(offersView, marketplace);
            currencyViewController = new CurrencyViewController(currencyView, marketplace);
            antivirusViewController = new AntivirusViewController(antivirusView, antivirus);

            notificationViewController?.Init();
            timeLimitViewController?.Init();
            offersViewController?.Init();
            currencyViewController?.Init();
            antivirusViewController?.Init();
        }

        private void OnDestroy()
        {
            antivirusViewController?.Dispose();
            currencyViewController?.Dispose();
            offersViewController?.Dispose();
            timeLimitViewController?.Dispose();
            notificationViewController?.Dispose();

            timeLimit?.Dispose();
            marketplace?.Dispose();
        }

        private void OnValidate()
        {
            products?.Refresh();
        }
    }
}
