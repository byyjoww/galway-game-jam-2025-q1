using Scamazon.Timers;
using Scamazon.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scamazon.App
{
    public class App : MonoBehaviour
    {
        [SerializeField] private TimeLimit.Config timeConfig = default;

        [Header("Views")]
        [SerializeField] private NotificationView notificationView = default;
        [SerializeField] private TimeLimitView timeLimitView = default;

        // Models
        private TimeLimit timeLimit = default;

        // View controllers
        private NotificationViewController notificationViewController = default;
        private TimeLimitViewController timeLimitViewController = default;

        private void Start()
        {
            timeLimit = new TimeLimit(timeConfig);

            CreateViewControllers();

            timeLimit.Start();
        }

        private void CreateViewControllers()
        {
            notificationViewController = new NotificationViewController(notificationView);
            timeLimitViewController = new TimeLimitViewController(timeLimitView, timeLimit, notificationViewController);

            notificationViewController?.Init();
            timeLimitViewController?.Init();
        }

        private void OnDestroy()
        {
            timeLimitViewController?.Dispose();
            notificationViewController?.Dispose();

            timeLimit?.Dispose();
        }
    }
}
