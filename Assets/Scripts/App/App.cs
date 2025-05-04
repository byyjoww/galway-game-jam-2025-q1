using Scamazon.Audio;
using Scamazon.Cursor;
using Scamazon.Offers;
using Scamazon.Timers;
using Scamazon.UI;
using Scamazon.Virus;
using SLS.Core;
using SLS.Core.Extensions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace Scamazon.App
{
    public class App : MonoBehaviour
    {
        [SerializeField] private TimeLimit.Config timeConfig = default;
        [SerializeField] private float startingCurrency = default;
        [SerializeField] private OfferFactory.Config offerConfig = default;

        [Header("Data")]
        [SerializeField] private Texture2D originalCursor = default;
        [SerializeField] private Texture2D frozenCursor = default;        
        [SerializeField] private Database<ProductSO> products = default;

        [Header("Audio")]
        [SerializeField] private TracklistSO bgm = default;
        [SerializeField] private SoundlistSO buttonClick = default;
        [SerializeField] private AudioSourceWrapper bgmSource = default;
        [SerializeField] private AudioSourceWrapper sfxSource = default;
        [SerializeField] private AudioMixer mixer = default;

        [Header("Views")]
        [SerializeField] private NotificationView notificationView = default;
        [SerializeField] private TimeLimitView timeLimitView = default;
        [SerializeField] private OffersView offersView = default;
        [SerializeField] private CurrencyView currencyView = default;
        [SerializeField] private AntivirusView antivirusView = default;
        [SerializeField] private DesktopIconsView desktopIconsView = default;
        [SerializeField] private ScoreView scoreView = default;
        [SerializeField] private StartGameView startGameView = default;
        [SerializeField] private EndGameView endGameView = default;
        [SerializeField] private View[] views = default;

        // Models
        private TimeLimit timeLimit = default;
        private OfferFactory offerFactory = default;
        private Marketplace marketplace = default;
        private Antivirus antivirus = default;
        private PlayerCursor cursor = default;
        private AudioSystem audioSystem = default;

        // View controllers
        private NotificationViewController notificationViewController = default;
        private TimeLimitViewController timeLimitViewController = default;
        private OffersViewController offersViewController = default;
        private CurrencyViewController currencyViewController = default;
        private AntivirusViewController antivirusViewController = default;
        private DesktopIconsViewController desktopIconsViewController = default;
        private ScoreViewController scoreViewController = default;
        private StartGameViewController startGameViewController = default;
        private EndGameViewController endGameViewController = default;

        private bool isRunning = false;

        public event UnityAction OnGameEnded;

        private void Start()
        {
            audioSystem = new AudioSystem(mixer, bgmSource, sfxSource, 0.5f, 0.25f, bgm, null);
            views.ForEach(x => x.SetAudioPlayer(audioSystem.SFXPlayer));
            views.ForEach(x => x.SetButtonAudio(buttonClick));

            timeLimit = new TimeLimit(timeConfig);
            offerFactory = new OfferFactory(products.Elements, offerConfig);
            cursor = new PlayerCursor(originalCursor, frozenCursor);
            antivirus = new Antivirus(cursor);
            marketplace = new Marketplace(offerFactory, antivirus, startingCurrency);

            CreateViewControllers();
            cursor.SetOriginalCursor();
            audioSystem.Init();
        }

        [ContextMenu("Reset")]
        public void ResetGame()
        {
            timeLimit.Reset();
            marketplace.Reset();
            antivirus.Reset();
            startGameViewController.ShowView();
        }

        [ContextMenu("Start")]
        public void StartGame()
        {
            if (isRunning) { return; }

            isRunning = true;
            timeLimit.Start();
            marketplace.StartShowingOffers();
        }

        [ContextMenu("End")]
        public void EndGame()
        {
            if (!isRunning) { return; }

            isRunning = false;
            timeLimit.Stop();
            marketplace.StopShowingOffers();
            antivirus.Reset();
            OnGameEnded?.Invoke();
        }

        private void CreateViewControllers()
        {
            startGameViewController = new StartGameViewController(startGameView, this);
            endGameViewController = new EndGameViewController(endGameView, this, timeLimit, marketplace);
            notificationViewController = new NotificationViewController(notificationView);
            timeLimitViewController = new TimeLimitViewController(timeLimitView, timeLimit, notificationViewController);
            offersViewController = new OffersViewController(offersView, marketplace, notificationViewController);
            currencyViewController = new CurrencyViewController(currencyView, marketplace);
            antivirusViewController = new AntivirusViewController(antivirusView, antivirus);
            desktopIconsViewController = new DesktopIconsViewController(desktopIconsView);
            scoreViewController = new ScoreViewController(scoreView, marketplace);

            startGameViewController?.Init();
            endGameViewController?.Init();
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
            startGameViewController?.Dispose();
            endGameViewController?.Dispose();

            timeLimit?.Dispose();
            marketplace?.Dispose();
            antivirus?.Dispose();
            audioSystem?.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            products?.Refresh();

            if (PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject) && PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                views = FindObjectsOfType<View>(true);
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}
