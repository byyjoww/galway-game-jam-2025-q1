using System.Collections.Generic;

namespace Scamazon.UI
{
    public class NotificationViewController : ViewController<NotificationView>
    {
        private Queue<NotificationView.PresenterModel> notifications = default;

        public NotificationViewController(NotificationView view) : base(view)
        {
            notifications = new Queue<NotificationView.PresenterModel>();
        }

        public override void Init()
        {
            view?.Init();
        }

        public void Enqueue(NotificationView.PresenterModel notification)
        {
            notifications.Enqueue(notification);
            Dequeue();
        }

        private void Dequeue()
        {
            var notification = notifications.Dequeue();
            view.SetActive(true);
            view.Setup(notification);
        }

        public override void Dispose()
        {

        }
    }
}