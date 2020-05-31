using Livet;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Contents;
using TimeRecorder.NavigationRail.ViewModels;
using TimeRecorder.Repository.SQLite;

namespace TimeRecorder.Contents.Configuration
{
    public class ConfigurationViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "設定", IconKey = "Cog" };

        public Swatch[] Swatches { get; } = new SwatchesProvider().Swatches.ToArray();

        public ConfigurationViewModel()
        {
            
        }

        public void ShutDown()
        {
            App.Current.Shutdown();
        }
    }
}
