using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Contents
{
    class ApplicationService
    {
        public static ApplicationService Instance { get; } = new ApplicationService();

        private ApplicationService()
        {

        }

        public bool IsShutDownProcessing { get; private set; }

        public void Shutdown()
        {
            IsShutDownProcessing = true;
            App.Current.MainWindow.Close();
        }
    }
}
