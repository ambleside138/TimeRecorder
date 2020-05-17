using Livet;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks.Editor;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.Contents.WorkUnitRecorder.Editor
{
    class WorkTaskEditDialogViewModel : ViewModel
    {
        public WorkTaskViewModel TaskCardViewModel { get; }

        public ReactivePropertySlim<bool> IsEditMode = new ReactivePropertySlim<bool>(false);

        private readonly WorkTaskEditDialogModel _WorkTaskEditDialogModel = new WorkTaskEditDialogModel();

        public WorkProcess[] Processes { get; }

        public Client[] Clients { get; }

        public WorkTaskEditDialogViewModel()
            : this(WorkTask.ForNew()) { }

        public WorkTaskEditDialogViewModel(WorkTask model)
        {
            IsEditMode.Value = true;

            Processes = _WorkTaskEditDialogModel.GetProcesses();
            Clients = _WorkTaskEditDialogModel.GetClients();

            TaskCardViewModel = new WorkTaskViewModel(model, Processes, Clients);
        }
    }
}
