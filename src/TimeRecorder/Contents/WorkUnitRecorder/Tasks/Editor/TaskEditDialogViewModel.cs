using Livet;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Contents.WorkUnitRecorder.Editor
{
    class TaskEditDialogViewModel : ViewModel
    {
        public WorkTaskViewModel TaskCardViewModel { get; }

        public ReactivePropertySlim<bool> IsEditMode = new ReactivePropertySlim<bool>(false);

        public TaskEditDialogViewModel()
            : this(WorkTask.ForNew()) { }

        public TaskEditDialogViewModel(WorkTask model)
        {
            TaskCardViewModel = new WorkTaskViewModel(model);
            IsEditMode.Value = true;
        }

        public void Regist()
        {

        }
    }
}
