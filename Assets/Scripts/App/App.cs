using Scamazon.Cursor;
using Scamazon.Offers;
using Scamazon.Timers;
using Scamazon.UI;
using Scamazon.Virus;
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
        [SerializeField] private Texture2D originalCursor = default;
        [SerializeField] private Texture2D frozenCursor = default;
        [SerializeField] private Database<ProductSO> products = default;

        [Header("Views")]
        [SerializeField] private NotificationView notificationView = default;
        [SerializeField] private TimeLimitView timeLimitView = default;
        [SerializeField] private OffersView offersView = default;
        [SerializeField] private CurrencyView currencyView = default;
        [SerializeField] private AntivirusView antivirusView = default;
        [SerializeField] private DesktopIconsView desktopIconsView = default;
        [SerializeField] private ScoreView scoreView = default;

        // Models
        private TimeLimit timeLimit = default;
        private OfferFactory offerFactory = default;
        private Marketplace marketplace = default;
        private Antivirus antivirus = default;
        private PlayerCursor cursor = default;

        // View controllers
        private NotificationViewController notificationViewController = default;
        private TimeLimitViewController timeLimitViewController = default;
        private OffersViewController offersViewController = default;
        private CurrencyViewController currencyViewController = default;
        private AntivirusViewController antivirusViewController = default;
        private DesktopIconsViewController desktopIconsViewController = default;
        private ScoreViewController scoreViewController = default;

        private void Start()
        {
            timeLimit = new TimeLimit(timeConfig);
            offerFactory = new OfferFactory(products.Elements);
            cursor = new PlayerCursor(originalCursor, frozenCursor);
            antivirus = new Antivirus(cursor);
            marketplace = new Marketplace(offerFactory, antivirus, startingCurrency);            

            CreateViewControllers();

            cursor.SetOriginalCursor();
            timeLimit.Start();
            marketplace.StartShowingOffers();
        }

        private void CreateViewControllers()
        {
            notificationViewController = new NotificationViewController(notificationView);
            timeLimitViewController = new TimeLimitViewController(timeLimitView, timeLimit, notificationViewController);
            offersViewController = new OffersViewController(offersView, marketplace, notificationViewController);
            currencyViewController = new CurrencyViewController(currencyView, marketplace);
            antivirusViewController = new AntivirusViewController(antivirusView, antivirus);
            desktopIconsViewController = new DesktopIconsViewController(desktopIconsView);
            scoreViewController = new ScoreViewController(scoreView, marketplace);

            notificationViewController?.Init();
            timeLimitViewController?.Init();
            offersViewController?.Init();
            currencyViewController?.Init();
            antivirusViewController?.Init();
            desktopIconsViewController?.Init();
            scoreViewController?.Init();
        }

        private void OnDestroy()
        {
            scoreViewController?.Dispose();
            desktopIconsViewController?.Dispose();
            antivirusViewController?.Dispose();
            currencyViewController?.Dispose();
            offersViewController?.Dispose();
            timeLimitViewController?.Dispose();
            notificationViewController?.Dispose();

            timeLimit?.Dispose();
            marketplace?.Dispose();
            antivirus?.Dispose();
        }

        private void OnValidate()
        {
            products?.Refresh();
        }
    }
}
