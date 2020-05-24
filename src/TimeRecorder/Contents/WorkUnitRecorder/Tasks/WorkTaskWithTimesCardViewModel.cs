using Livet;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks;
using TimeRecorder.Contents.WorkUnitRecorder.Tracking;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    /// <summary>
    /// 作業時間の情報を含むタスクを表すViewModelです
    /// </summary>
    public class WorkTaskWithTimesCardViewModel : ViewModel
    {
        private readonly WorkTaskModel _Model = new WorkTaskModel();

        public ReactivePropertySlim<bool> IsIndeterminate { get; } = new ReactivePropertySlim<bool>(false);

        public ReactivePropertySlim<int> ProgressValue { get; } = new ReactivePropertySlim<int>(0);


        public WorkTaskWithTimesCardViewModel(WorkTaskWithTimesDto dto)
        {
            Dto = dto;

            if(dto.WorkingTimes.Any(t => t.IsDoing))
            {
                IsIndeterminate.Value = true;
                ProgressValue.Value = 20;
            }
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
