using Livet;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Buttons
{
    public class AddingTaskButtonViewModel : ViewModel
    {
        public string ButtonTitle { get; set; }

        public string ToolTipDescription { get; set; }

        public bool UseAccentColor { get; set; }

        private IAddingTaskCommand _AddingTaskCommand;

        public AddingTaskButtonViewModel(IAddingTaskCommand addingTaskCommand)
        {
            _AddingTaskCommand = addingTaskCommand;
        }

        public void Invoke()
        {
            _AddingTaskCommand.Invoke();
        }
    }
}
