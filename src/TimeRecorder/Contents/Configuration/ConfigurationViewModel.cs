using Google.Apis.Util;
using Livet;
using MaterialDesignColors;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using TimeRecorder.Configurations;
using TimeRecorder.Configurations.Items;
using TimeRecorder.Contents;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Helpers;
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

        public ObservableCollection<FavoriteWorkTask> FavoriteWorkTasks { get; }

        public ReactivePropertySlim<bool> ShowFavoriteDescription { get; }

        public ConfigurationViewModel()
        {
            // ComboBoxの初期値を設定するにはItemsSourceで利用しているインスタンスの中から指定する必要がある
            SelectedSwatch = new ReactivePropertySlim<Swatch>(Swatches.FirstOrDefault(s => s.Name == ThemeService.CurrentTheme.Name));
            SelectedSwatch.Subscribe(s =>ChangeTheme(s)).AddTo(CompositeDisposable);

            var backupPath = UserConfigurationManager.Instance.GetConfiguration<BackupPathConfig>(ConfigKey.BackupPath);
            BackupPath = new ReactivePropertySlim<string>(backupPath?.DirectoryPath);

            var favorites = UserConfigurationManager.Instance.GetConfiguration<FavoriteWorkTasksConfig>(ConfigKey.FavoriteWorkTask);
            FavoriteWorkTasks = new ObservableCollection<FavoriteWorkTask>(favorites?.FavoriteWorkTasks ?? new FavoriteWorkTask[0]);

            ShowFavoriteDescription = new ReactivePropertySlim<bool>(FavoriteWorkTasks.Count == 0);
        }

        private void ChangeTheme(Swatch swatch)
        {
            ThemeService.ApplyTheme(swatch);
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

        public void AddFavoriteTask()
        {
            var editDialogVm = new WorkTaskEditDialogViewModel();
            editDialogVm.ShowQuickStartButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                FavoriteWorkTasks.Add(FavoriteWorkTask.FromDomainObject(inputValue));
                RegistFavoriteWorkTasksConfig();
            }
        }

        public void EditFavoriteTask(FavoriteWorkTask favoriteWorkTask)
        {
            var editDialogVm = new WorkTaskEditDialogViewModel(favoriteWorkTask.ConvertToDomainModel());
            editDialogVm.ShowQuickStartButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                if (editDialogVm.NeedDelete)
                {
                    FavoriteWorkTasks.Remove(favoriteWorkTask);
                    RegistFavoriteWorkTasksConfig();
                }
                else
                {
                    var inputValue = editDialogVm.TaskCardViewModel.DomainModel;

                    // 変更通知機能がないのでdelete-insert方式で無理やりViewに変更通知する
                    var targetIndex = FavoriteWorkTasks.IndexOf(favoriteWorkTask);
                    FavoriteWorkTasks.RemoveAt(targetIndex);
                    FavoriteWorkTasks.Insert(targetIndex, FavoriteWorkTask.FromDomainObject(inputValue));

                    RegistFavoriteWorkTasksConfig();
                }

            }
        }

        private void RegistFavoriteWorkTasksConfig()
        {
            ShowFavoriteDescription.Value = FavoriteWorkTasks.Count == 0;
            UserConfigurationManager.Instance.SetConfiguration(new FavoriteWorkTasksConfig { FavoriteWorkTasks = FavoriteWorkTasks.ToArray() });
        }

        public void ShutDown()
        {
            App.Current.Shutdown();
        }
    }
}
