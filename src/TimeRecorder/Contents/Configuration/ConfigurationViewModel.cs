using Livet;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Contents;
using TimeRecorder.NavigationRail.ViewModels;
using TimeRecorder.Repository.SQLite;

namespace TimeRecorder.Contents.Configuration
{
    public class ConfigurationViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "設定", IconKey = "Cog" };

        public void CreateDbFile()
        {
            Setup.CreateDatabaseFile();
        }
    }
}
