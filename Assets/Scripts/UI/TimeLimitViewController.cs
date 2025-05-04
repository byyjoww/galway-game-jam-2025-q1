using Scamazon.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scamazon.UI
{
    public class TimeLimitViewController : ViewController<TimeLimitView, TimeLimit>
    {
        private NotificationViewController notificationViewController = default;

        private Dictionary<double, string> alerts = new Dictionary<double, string>()
        {
            // { 178, "Hello World!" },
        };

        private float progress => (float)(model.ElapsedTime.TotalSeconds / model.TotalTime.TotalSeconds);

        // Progress
        private float countdownFill => 1f - progress;
        private float countupFill => progress;
        private float currentFill => countdown
            ? countdownFill
            : countupFill;

        // Text
        private string countdownTime => model.RemainingTime.ToString("mm':'ss");
        private string countupTime => model.ElapsedTime.ToString("mm':'ss");
        private string timerDisabledTime => "\u221E"; //"00:00";
        private bool isTimerDisabled => false;
        private string timeRemaining => isTimerDisabled
            ? timerDisabledTime
            : countdown
                ? countdownTime
                : countupTime;

        private bool countdown = true;
        private int nextAlertIndex = default;

        public TimeLimitViewController(TimeLimitView view, TimeLimit model, NotificationViewController notificationViewController) : base(view, model)
        {
            this.notificationViewController = notificationViewController;
            alerts = alerts.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
            nextAlertIndex = 0;
        }

        public override void Init()
        {
            view.SetActive(true);
            model.OnTimeRemainingChanged += UpdateView;
            UpdateViewWithCurrentTime();
        }

        private void UpdateView(TimeSpan prev, TimeSpan remaining)
        {
            if (TryGetNextAlertWithinTimeRange(prev, remaining, out KeyValuePair<double, string> next))
            {
                IncrementAlertIndex();
                SendNotification(next.Value);
            }

            UpdateViewWithCurrentTime();
        }

        private void UpdateViewWithCurrentTime()
        {
            view.Setup(new TimeLimitView.PresenterModel
            {
                TimeText = "Time",
                TimeRemaining = timeRemaining,
            });
        }

        private void SendNotification(string notificationKey)
        {
            notificationViewController.Enqueue(new NotificationView.PresenterModel
            {
                NotificationText = notificationKey,
                IsPopupText = false,
                CreateScreenNotificationTweenFunc = (go) =>
                {
                    return LeanTween.delayedCall(2f, () =>
                    {
                        go.SetActive(false);
                    });
                },
            });
        }

        private bool TryGetNextAlertWithinTimeRange(TimeSpan prev, TimeSpan remaining, out KeyValuePair<double, string> next)
        {
            if (alerts.Count == 0 || nextAlertIndex == -1)
            {
                next = default;
                return false;
            }

            next = alerts.ElementAt(nextAlertIndex);
            return prev.TotalSeconds > next.Key && remaining.TotalSeconds <= next.Key;
        }

        private void IncrementAlertIndex()
        {
            if (nextAlertIndex + 1 >= alerts.Count)
            {
                nextAlertIndex = -1;
                return;
            }

            nextAlertIndex++;
        }

        public override void Dispose()
        {
            model.OnTimeRemainingChanged -= UpdateView;
        }
    }
}