using Livet;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.NavigationRail.ViewModels;
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

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkUnitRecorderViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "入力", IconKey = "CalendarClock" };

        private readonly WorkUnitRecorderModel _Model = new WorkUnitRecorderModel();

        public ReadOnlyReactiveCollection<WorkTaskWithTimesCardViewModel> PlanedTaskCards { get; }

        public ReadOnlyReactiveCollection<TimelineWorkingTimeCardViewModel> WorkingTimes { get; }

        public ReactiveProperty<TimelineWorkingTimeCardViewModel> DoingTask { get; }

        public ReactiveProperty<DateTime> TargetDateTime { get; }

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

            TargetDateTime = _Model.TargetDate
                                   .ToReactivePropertyAsSynchronized(d => d.Value)
                                   .AddTo(CompositeDisposable);

            CompositeDisposable.Add(_Model);

            Initialize();

            // 1sスパンで更新する
            var timer = new ReactiveTimer(TimeSpan.FromSeconds(1), new SynchronizationContextScheduler(SynchronizationContext.Current));
            timer.Subscribe(_ => {
                    _Model.UpdateDoingTask();
                    foreach(var obj in PlanedTaskCards)
                    {
                        obj.UpdateStatus();
                    }
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



        public async void ExecuteNewTaskDialog()
        {
            var editDialogVm = new WorkTaskEditDialogViewModel();

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new TaskEditDialog
            {
                DataContext = editDialogVm
            };

            //show the dialog
            var result = (bool?)await DialogHost.Show(view);

            if (result.HasValue && result.Value)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                _Model.AddWorkTask(inputValue);
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
