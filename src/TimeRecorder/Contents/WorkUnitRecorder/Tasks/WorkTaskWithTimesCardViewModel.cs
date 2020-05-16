using Livet;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks;
using TimeRecorder.Domain.UseCase.Tasks;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
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

        public WorkTaskWithTimesDto Dto { get; }
    }
}
