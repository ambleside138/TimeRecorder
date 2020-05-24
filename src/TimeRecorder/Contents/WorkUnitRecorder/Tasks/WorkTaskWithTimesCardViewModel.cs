using Livet;
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
                                                                        

        public WorkTaskWithTimesCardViewModel(WorkTaskWithTimesDto dto)
        {
            Dto = dto;
            IsIndeterminate.Value = dto.WorkingTimes.Any(t => t.IsDoing);

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

        public void StartWorkTask()
        {
            _Model.StartWorking(Dto.TaskId);
        }


        public async void EditWorkTask()
        {
            var targetData = _Model.SelectWorkTask(Dto.TaskId);

            var editDialogVm = new WorkTaskEditDialogViewModel(targetData);

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
                _Model.EditWorkTask(inputValue);
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
