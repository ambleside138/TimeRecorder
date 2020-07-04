﻿using Google.Apis.Util;
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
using TimeRecorder.Contents.Configuration.TaskConfigEditor;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Domain.Domain.Calendar;
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
        public ObservableCollection<ScheduleTitleMap> ScheduleTitleMaps { get; }

        public ReactivePropertySlim<bool> ShowFavoriteDescription { get; }
        public ReactivePropertySlim<bool> ShowScheduleDescription { get; }

        public ConfigurationViewModel()
        {
            // ComboBoxの初期値を設定するにはItemsSourceで利用しているインスタンスの中から指定する必要がある
            SelectedSwatch = new ReactivePropertySlim<Swatch>(Swatches.FirstOrDefault(s => s.Name == ThemeService.CurrentTheme.Name));
            SelectedSwatch.Subscribe(s =>ChangeTheme(s)).AddTo(CompositeDisposable);

            var backupPath = UserConfigurationManager.Instance.GetConfiguration<BackupPathConfig>(ConfigKey.BackupPath);
            BackupPath = new ReactivePropertySlim<string>(backupPath?.DirectoryPath);

            var favorites = UserConfigurationManager.Instance.GetConfiguration<FavoriteWorkTasksConfig>(ConfigKey.FavoriteWorkTask);
            FavoriteWorkTasks = new ObservableCollection<FavoriteWorkTask>(favorites?.FavoriteWorkTasks ?? new FavoriteWorkTask[0]);

            var maps = UserConfigurationManager.Instance.GetConfiguration<ScheduleTitleMapConfig>(ConfigKey.ScheduleTitleMap)?.ScheduleTitleMaps;
            if(maps == null || maps.Count() == 0)
            {
                // 新設定が未登録なら旧設定をとる
                var config = JsonFileIO.Deserialize<TimeRecorderConfiguration>("TimeRecorderConfiguration.json") ?? new TimeRecorderConfiguration();
                maps = config.WorkTaskBuilderConfig.TitleMappers?.Select(t => t.ConvertToScheduleTitleMap()).ToArray();
            }
            ScheduleTitleMaps = new ObservableCollection<ScheduleTitleMap>(maps ?? new ScheduleTitleMap[0]);

            ShowFavoriteDescription = new ReactivePropertySlim<bool>(FavoriteWorkTasks.Count == 0);
            ShowScheduleDescription = new ReactivePropertySlim<bool>(ScheduleTitleMaps.Count == 0);
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


        #region favorite task

        public void AddFavoriteTask()
        {
            var editDialogVm = new TaskConfigEditDialogViewModel("ボタンタイトル", "");
            editDialogVm.ShowQuickStartButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskConfigEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                var favTask = FavoriteWorkTask.FromDomainObject(inputValue);
                favTask.ButtonTitle = editDialogVm.ConfigTitle.Value;
                FavoriteWorkTasks.Add(favTask);

                RegistFavoriteWorkTasksConfig();
            }
        }

        public void EditFavoriteTask(FavoriteWorkTask favoriteWorkTask)
        {
            var editDialogVm = new TaskConfigEditDialogViewModel("ボタンタイトル", favoriteWorkTask.ButtonTitle, favoriteWorkTask.ConvertToDomainModel());
            editDialogVm.ShowQuickStartButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskConfigEditDialog>(editDialogVm);

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

                    var favTask = FavoriteWorkTask.FromDomainObject(inputValue);
                    favTask.ButtonTitle = editDialogVm.ConfigTitle.Value;
                    FavoriteWorkTasks.Insert(targetIndex, favTask);

                    RegistFavoriteWorkTasksConfig();
                }

            }
        }

        private void RegistFavoriteWorkTasksConfig()
        {
            ShowFavoriteDescription.Value = FavoriteWorkTasks.Count == 0;
            UserConfigurationManager.Instance.SetConfiguration(new FavoriteWorkTasksConfig { FavoriteWorkTasks = FavoriteWorkTasks.ToArray() });
        }

        #endregion

        #region schedule title map

        public void AddScheduleTitleMap()
        {
            var editDialogVm = new TaskConfigEditDialogViewModel("取込時のタイトル", "");
            editDialogVm.ShowQuickStartButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskConfigEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                var favTask = ScheduleTitleMap.FromDomainObject(inputValue);
                favTask.MapTitle = editDialogVm.ConfigTitle.Value;
                ScheduleTitleMaps.Add(favTask);

                RegistScheduleTitleMapsConfig();
            }
        }

        public void EditScheduleTitleMap(ScheduleTitleMap favoriteWorkTask)
        {
            var editDialogVm = new TaskConfigEditDialogViewModel("取込時のタイトル", favoriteWorkTask.MapTitle, favoriteWorkTask.ConvertToDomainModel());
            editDialogVm.ShowQuickStartButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskConfigEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                if (editDialogVm.NeedDelete)
                {
                    ScheduleTitleMaps.Remove(favoriteWorkTask);
                    RegistScheduleTitleMapsConfig();
                }
                else
                {
                    var inputValue = editDialogVm.TaskCardViewModel.DomainModel;

                    // 変更通知機能がないのでdelete-insert方式で無理やりViewに変更通知する
                    var targetIndex = ScheduleTitleMaps.IndexOf(favoriteWorkTask);
                    ScheduleTitleMaps.RemoveAt(targetIndex);

                    var favTask = ScheduleTitleMap.FromDomainObject(inputValue);
                    favTask.MapTitle = editDialogVm.ConfigTitle.Value;
                    ScheduleTitleMaps.Insert(targetIndex, favTask);

                    RegistScheduleTitleMapsConfig();
                }

            }
        }

        private void RegistScheduleTitleMapsConfig()
        {
            ShowScheduleDescription.Value = ScheduleTitleMaps.Count == 0;
            UserConfigurationManager.Instance.SetConfiguration(new ScheduleTitleMapConfig { ScheduleTitleMaps = ScheduleTitleMaps.ToArray() });
        } 
        #endregion

        public void ShutDown()
        {
            ApplicationService.Instance.Shutdown();
        }
    }
}
