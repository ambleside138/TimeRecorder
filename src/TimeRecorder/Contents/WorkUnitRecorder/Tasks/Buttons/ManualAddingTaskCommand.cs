﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Helpers;
using TimeRecorder.Host;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Buttons;

/// <summary>
/// 編集画面を表示してからタスク追加するコマンドを表します
/// </summary>
class ManualAddingTaskCommand : IAddingTaskCommand
{
    private readonly WorkTaskRegistor _WorkTaskRegistor = new();

    public void Invoke()
    {
        ExecuteNewTaskDialog();
    }

    private void ExecuteNewTaskDialog()
    {
        // 汚いのでなんとかしたい...
        var linkUrl = MainWindowViewModel.Instance.Contents.OfType<WorkUnitRecorderViewModel>().First().TimeCardLinkURL;

        var editDialogVm = new WorkTaskEditDialogViewModel(linkUrl);

        var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

        if (result == ModalTransitionResponse.Yes)
        {
            var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
            _WorkTaskRegistor.AddWorkTask(inputValue, editDialogVm.NeedStart);
        }
    }
}
