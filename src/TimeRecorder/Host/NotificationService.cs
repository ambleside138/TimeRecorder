using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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
            GetMainWindow().Notify(ToolTipIconKind.Info, title, content);
        }

        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;
    }
}
