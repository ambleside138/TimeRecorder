using Microsoft.Toolkit.Uwp.Notifications;

namespace TimeRecorder.Host
{
    public class NotificationService
    {
        public static NotificationService Current => new NotificationService();

        private NotificationService()
        {

        }

        public void Info(string title, string content)
        {
            new ToastContentBuilder()
                 .AddText(title)
                 .AddText(content)
                 .AddAttributionText("TimeRecorder ⏰ 工数管理")
                 .Show();
        }
    }
}
