using Livet;
using Livet.Messaging;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks;
using TimeRecorder.Contents.WorkUnitRecorder.Tracking;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Helpers;
using TimeRecorder.Host;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    /// <summary>
    /// 作業時間の情報を含むタスクを表すViewModelです
    /// </summary>
    public class WorkTaskWithTimesCardViewModel : ViewModel
    {
        private readonly WorkTaskModel _Model = new WorkTaskModel();

        public ReactivePropertySlim<bool> IsIndeterminate { get; } = new ReactivePropertySlim<bool>(false);


        public ReactivePropertySlim<bool> IsCompleted { get; }

        public ReactivePropertySlim<string> PlayTooltip { get; set; } = new ReactivePropertySlim<string>();

        public ReactivePropertySlim<string> PlayIconKind { get; set; } = new ReactivePropertySlim<string>();


        public WorkTaskWithTimesCardViewModel(WorkTaskWithTimesDto dto)
        {
            Dto = dto;
            UpdateStatus();

            IsCompleted = new ReactivePropertySlim<bool>(false);

            IsCompleted.Subscribe(b =>
                            {
                                if(b)
                                {
                                    CompleteWorkTask();
                                }
                            })
                        .AddTo(CompositeDisposable);

        }

        public void UpdateStatus()
        {
            IsIndeterminate.Value = Dto.WorkingTimes.Any(t => t.IsDoing);

            if (IsIndeterminate.Value)
            {
                PlayTooltip.Value = "現在の作業を停止";
                PlayIconKind.Value = "Pause";
            }
            else
            {
                PlayTooltip.Value = "タスクを開始";
                PlayIconKind.Value = "Play";
            }
        }

        public void StartOrStopWorkTask()
        {
            var doingTask = Dto.WorkingTimes.LastOrDefault(t => t.IsDoing);
            if(doingTask != null)
            {
                _Model.StopWorking(doingTask.Id);
            }
            else
            {
                _Model.StartWorking(Dto.TaskId);
            }
        }


        public void EditWorkTask()
        {
            var targetData = _Model.SelectWorkTask(Dto.TaskId);

            var editDialogVm = new WorkTaskEditDialogViewModel(targetData);

            var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                if(editDialogVm.NeedDelete)
                {
                    _Model.DeleteWorkTask(Dto.TaskId);
                }
                else
                {
                    var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                    _Model.EditWorkTask(inputValue);
                }

            }
        }



        public async void CompleteWorkTask()
        {
            try
            {
                _Model.CompleteWorkTask(Dto.TaskId);
                SnackbarService.Current.ShowMessage("[タスク完了] おつかれさまでした！");
            }
            catch (TimeRecorder.Domain.Utility.Exceptions.SpecificationCheckException ex)
            {
                SnackbarService.Current.ShowMessage(ex.Message);

                await Task.Delay(TimeSpan.FromSeconds(2));
                IsCompleted.Value = false;
            } 
        }

        public void DeleteWorkTask()
        {
            _Model.DeleteWorkTask(Dto.TaskId);
        }

        public async void AddWorkingTime()
        {
            var workingTimeRange = WorkingTimeRange.ForEdit(Dto.TaskId);

            var editDialogVm = new WorkingTimeRangeEditDialogViewModel(workingTimeRange);

            var view = new WorkingTimeRangeEditDialog
            {
                DataContext = editDialogVm
            };

            //show the dialog
            var result = (bool?)await DialogHost.Show(view);

            if (result.HasValue && result.Value)
            {
                var editObj = editDialogVm.WorkingTimeViewModel.DomainModel;
                _Model.AddWorkingTime(editObj);
            }
        }

        public async void EditWorkingTime(WorkingTimeRange workingTimeRange)
        {
            var editDialogVm = new WorkingTimeRangeEditDialogViewModel(workingTimeRange);

            var view = new WorkingTimeRangeEditDialog
            {
                DataContext = editDialogVm
            };

            //show the dialog
            var result = (bool?)await DialogHost.Show(view);

            if (result.HasValue && result.Value)
            {
                var editObj = editDialogVm.WorkingTimeViewModel.DomainModel;
                _Model.EditWorkingTime(editObj);
            }
        }

        public void DeleteWorkingTime(WorkingTimeRange workingTimeRange)
        {
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith((t, _) => IsSample4DialogOpen = false, null,
            //        TaskScheduler.FromCurrentSynchronizationContext());

            _Model.DeleteWorkingTime(workingTimeRange);
        }

        public WorkTaskWithTimesDto Dto { get; }

    }
}
