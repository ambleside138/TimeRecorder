using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Editor
{
    class ProcessViewModel : ViewModel
    {
        public ReactivePropertySlim<bool> IsSelected { get; }

        public string Title { get; }

        public Identity<WorkProcess> ProcessId { get; }

        public ProcessViewModel(WorkProcess process)
        {
            IsSelected = new ReactivePropertySlim<bool>(false).AddTo(CompositeDisposable);

            Title = process.Title;
            ProcessId = process.Id;
        }
    }
}
