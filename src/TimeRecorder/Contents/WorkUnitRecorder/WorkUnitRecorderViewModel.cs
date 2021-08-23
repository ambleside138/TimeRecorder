using Livet;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.NavigationRail;
using Reactive.Bindings.Helpers;
using Reactive.Bindings.Extensions;
using TimeRecorder.Contents.WorkUnitRecorder.Timeline;
using System.Reactive.Linq;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.Utility;
using System.Reactive.Concurrency;
using System.Threading;
using System.Linq;
using TimeRecorder.Host;
using System.Threading.Tasks;
using TimeRecorder.Helpers;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks.Buttons;
using TimeRecorder.Configurations;
using TimeRecorder.Configurations.Items;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    /// <summary>
    /// 入力タブ に対応するViewModelを表します
    /// </summary>
    public class WorkUnitRecorderViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new() { Title = "入力", IconKey = "CalendarClock" };

        private readonly WorkUnitRecorderModel _Model = new WorkUnitRecorderModel();

        public ReadOnlyReactiveCollection<WorkTaskWithTimesCardViewModel> PlanedTaskCards { get; }

        public ReadOnlyReactiveCollection<TimelineWorkingTimeCardViewModel> WorkingTimes { get; }

        public ReactiveProperty<TimelineWorkingTimeCardViewModel> DoingTask { get; }

        public ReactiveProperty<DateTime> TargetDateTime { get; }

        public ReactiveCollection<AddingTaskButtonViewModel> AddingTaskButtons { get; }

        public ReactiveProperty<bool> ContainsCompleted { get; }

        private int _NoValueCount = 0;

        // 通知間隔[sec]
        private const int _AlertCount = 60 * 5;

        public WorkUnitRecorderViewModel()
        {
            PlanedTaskCards = _Model.PlanedTaskModels
                                    .ToReadOnlyReactiveCollection(t => new WorkTaskWithTimesCardViewModel(t))
                                    .AddTo(CompositeDisposable);

            WorkingTimes = _Model.WorkingTimes
                                 .ToReadOnlyReactiveCollection(w => new TimelineWorkingTimeCardViewModel(w))
                                 .AddTo(CompositeDisposable);

            DoingTask = _Model.DoingTask
                              .Select(t => new TimelineWorkingTimeCardViewModel(t))
                              .ToReactiveProperty()
                              .AddTo(CompositeDisposable);

            ContainsCompleted = _Model.ContainsCompleted
                                      .ToReactivePropertyAsSynchronized(d => d.Value)
                                      .AddTo(CompositeDisposable);

            // 変更前のタイマー解除
            DoingTask.Pairwise().Subscribe(pair => pair.OldItem?.Dispose());

            TargetDateTime = _Model.TargetDate
                                   .ToReactivePropertyAsSynchronized(d => d.Value)
                                   .AddTo(CompositeDisposable);

            AddingTaskButtons = new ReactiveCollection<AddingTaskButtonViewModel>();
            InitializeAddingTaskButtons();

            CompositeDisposable.Add(_Model);

            Initialize();

            // 1sスパンで更新する
            var timer = new ReactiveTimer(TimeSpan.FromSeconds(1), new SynchronizationContextScheduler(SynchronizationContext.Current));
            timer.Subscribe(_ => {
                    _Model.BackupIfNeeded();
                    _Model.UpdateDoingTask();
                    foreach(var obj in PlanedTaskCards)
                    {
                        obj.UpdateStatus();
                    }

                    UpdateDurationTime();
                    _Model.CheckLunchTime();
                }
            );
            timer.AddTo(CompositeDisposable);
            timer.Start();
        }



        public async void Initialize()
        {
            // 初回の変更通知でよばれるようになったので不要
            //  _Model.Load();
            await ImportTaskFromCalendarCore(needMessage:false);

        }

        public void UpdateDurationTime()
        {
            if (DoingTask?.Value?.DomainModel != null)
            {
                _NoValueCount = 0;
                return;
            }

            // 一定時間タスク開始していないなら通知する

            if(_NoValueCount >= 0)
            {
                var config = UserConfigurationManager.Instance.GetConfiguration<LunchTimeConfig>(ConfigKey.LunchTime);
                if (config != null
                    && string.IsNullOrEmpty(config.StartHHmm) == false
                    && string.IsNullOrEmpty(config.EndHHmm) == false)
                {
                    // 休憩中はカウントしない
                    if (config.TimePeriod.WithinRangeAtCurrentTime)
                    {
                        _NoValueCount = 0;
                        return;
                    }
                }

                if (_NoValueCount > _AlertCount)
                {
                    if (PlanedTaskCards.Any())
                    {
                        var selectableTasks = PlanedTaskCards.Where(c => c.IsScheduled == false).Select(c => c.Dto);
                        NotificationService.Current.ShowTaskStarterInteractor(selectableTasks, "作業タスクが設定されていません");
                    }
                    else
                    {
                        NotificationService.Current.Info("お知らせ", "作業タスクが設定されていません");
                    }

                    // snooze機能はToast側の実装にまかせる
                    _NoValueCount = -1;
                }
                else
                {
                    _NoValueCount++;
                }
            }
            else
            {
                // 通知済み
            }
        }

        private void InitializeAddingTaskButtons()
        {
            var favorites = UserConfigurationManager.Instance.GetConfiguration<FavoriteWorkTasksConfig>(ConfigKey.FavoriteWorkTask);
            if(favorites?.FavoriteWorkTasks?.Any() == true)
            {
                var buttons = favorites.FavoriteWorkTasks.Select(t => new AddingTaskButtonViewModel(new ShortcutAddingTaskCommand(t))
                                                                        {
                                                                            ButtonTitle = string.IsNullOrEmpty(t.ButtonTitle) ? t.Title.Substring(0,1) : t.ButtonTitle,
                                                                            ToolTipDescription = t.Title,
                                                                        });
                AddingTaskButtons.AddRange(buttons);
            }
            AddingTaskButtons.Add(new AddingTaskButtonViewModel(new ManualAddingTaskCommand()) 
                                    { 
                                        ButtonTitle = "New", 
                                        ToolTipDescription = "編集画面から追加します",
                                        UseAccentColor = true,
                                    });
        }


        public void ExecuteNewTaskDialog()
        {
            var editDialogVm = new WorkTaskEditDialogViewModel();

            var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                _Model.AddWorkTask(inputValue, editDialogVm.NeedStart);
            }
        }

        public async void ImportTaskFromCalendar()
        {
            await ImportTaskFromCalendarCore(true);
        }

        private async Task ImportTaskFromCalendarCore(bool needMessage)
        {
            var imported = await _Model.ImportFromCalendarAsync();

            if (needMessage || imported.Any())
            {
                if(imported.Any())
                {
                    SnackbarService.Current.ShowMessage($"{imported.Length}件の予定を取り込みました");
                }
                else
                {
                    SnackbarService.Current.ShowMessage($"取込対象の予定は見つかりませんでした");
                }
            }
        }


        public void StopCurrentTask()
        {
            _Model.StopCurrentTask();
        }

    }
}
