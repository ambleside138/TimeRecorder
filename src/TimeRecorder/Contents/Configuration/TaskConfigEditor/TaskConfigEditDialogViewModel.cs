using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Contents.Configuration.TaskConfigEditor;

class TaskConfigEditDialogViewModel : WorkTaskEditDialogViewModel
{
    public ReactivePropertySlim<string> WindowTitle { get; } = new ReactivePropertySlim<string>();

    public ReactivePropertySlim<string> ConfigTitleLabel { get; } = new ReactivePropertySlim<string>();

    public ReactivePropertySlim<string> ConfigTitle { get; } = new ReactivePropertySlim<string>();

    public TaskConfigEditDialogViewModel(string configTitle, string configTitleValue)
     : this(configTitle, configTitleValue, WorkTask.ForNew()) { }

    public TaskConfigEditDialogViewModel(string configTitle, string configTitleValue, WorkTask model)
        : base(model)
    {
        ConfigTitleLabel.Value = configTitle;
        ConfigTitle.Value = configTitleValue;
    }
}
