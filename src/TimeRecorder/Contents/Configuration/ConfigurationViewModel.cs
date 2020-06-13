using Google.Apis.Util;
using Livet;
using MaterialDesignColors;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

        public ReactivePropertySlim<string> BackupPath { get; }

        public ConfigurationViewModel()
        {
            // ComboBoxの初期値を設定するにはItemsSourceで利用しているインスタンスの中から指定する必要がある
            SelectedSwatch = new ReactivePropertySlim<Swatch>(Swatches.FirstOrDefault(s => s.Name == ThemeService.CurrentTheme.Name));
            SelectedSwatch.Subscribe(s =>ChangeTheme(s)).AddTo(CompositeDisposable);

            var backupPath = UserConfigurationManager.Instance.GetConfiguration<BackupPathConfig>(ConfigKey.BackupPath);
            BackupPath = new ReactivePropertySlim<string>(backupPath?.DirectoryPath);
        }

        private void ChangeTheme(Swatch swatch)
        {
            ThemeService.ApplyPrimary(swatch);
            UserConfigurationManager.Instance.SetConfiguration(new ThemeConfig(swatch));
        }

        public void RegistBackupPath()
        {
            if(Directory.Exists(BackupPath.Value) == false)
            {
                SnackbarService.Current.ShowMessage("指定されたフォルダがみつかりません");
                return;
            }

            UserConfigurationManager.Instance.SetConfiguration(new BackupPathConfig { DirectoryPath = BackupPath.Value });
            SnackbarService.Current.ShowMessage("バックアップ先を変更しました");
        }

        public void ShutDown()
        {
            App.Current.Shutdown();
        }
    }
}
