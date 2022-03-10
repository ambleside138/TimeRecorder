using Livet;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tracking;

class WorkingTimeRangeEditDialogViewModel : ViewModel
{
    public WorkingTimeRange DomainModel { get; }

    public WorkingTimeViewModel WorkingTimeViewModel { get; }

    public WorkingTimeRangeEditDialogViewModel(WorkingTimeRange workingTimeRange)
    {
        DomainModel = workingTimeRange;
        WorkingTimeViewModel = new WorkingTimeViewModel(workingTimeRange);
    }

    public void Regist()
    {
        if (WorkingTimeViewModel.TryValidate())
        {
            Messenger.Raise(new Livet.Messaging.InteractionMessage("RegistKey"));
        }

    }
}
