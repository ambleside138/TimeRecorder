using Google.Apis.Util;
using Livet;
using MaterialDesignColors;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using TimeRecorder.Configurations;
using TimeRecorder.Configurations.Items;
using TimeRecorder.Contents;
using TimeRecorder.Host;
using TimeRecorder.NavigationRail.ViewModels;
using TimeRecorder.Repository.SQLite;

namespace TimeRecorder.Contents.Configuration
{
    public class ConfigurationViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "設定", IconKey = "Cog" };

        public Swatch[] Swatches { get; } = new SwatchesProvider().Swatches.ToArray();

        public ReactivePropertySlim<Swatch> SelectedSwatch { get; }

        public ConfigurationViewModel()
        {
            // ComboBoxの初期値を設定するにはItemsSourceで利用しているインスタンスの中から指定する必要がある
            SelectedSwatch = new ReactivePropertySlim<Swatch>(Swatches.FirstOrDefault(s => s.Name == ThemeService.CurrentTheme.Name));
            SelectedSwatch.Subscribe(s =>
            {
                ThemeService.ApplyPrimary(s);
                UserConfigurationManager.Instance.SetConfiguration(ConfigKey.Theme, new ThemeConfig(s));
            }).AddTo(CompositeDisposable);
        }

        public void ShutDown()
        {
            App.Current.Shutdown();
        }
    }
}
