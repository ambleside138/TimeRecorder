using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Buttons
{
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
            var editDialogVm = new WorkTaskEditDialogViewModel();

            var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                _WorkTaskRegistor.AddWorkTask(inputValue, editDialogVm.NeedStart);
            }
        }
    }
}
