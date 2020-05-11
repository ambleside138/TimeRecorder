using Livet;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.NavigationRail.ViewModels;
using Reactive.Bindings.Helpers;
using Reactive.Bindings.Extensions;
using TimeRecorder.Contents.WorkUnitRecorder.Timeline;
using System.Reactive.Linq;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkUnitRecorderViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "入力", IconKey = "CalendarClock" };

        private readonly WorkUnitRecorderModel _Model = new WorkUnitRecorderModel();

        public ReadOnlyReactiveCollection<TaskCardViewModel> PlanedTaskCards { get; }

        public ReadOnlyReactiveCollection<WorkingTimeCardViewModel> WorkingTimes { get; }

        public ReactiveProperty<WorkingTimeCardViewModel> DoingTask { get; }


        public WorkUnitRecorderViewModel()
        {
            PlanedTaskCards = _Model.PlanedTaskModels
                                    .ToReadOnlyReactiveCollection(t => new TaskCardViewModel(t))
                                    .AddTo(CompositeDisposable);

            WorkingTimes = _Model.WorkingTimes
                                 .ToReadOnlyReactiveCollection(w => new WorkingTimeCardViewModel(w))
                                 .AddTo(CompositeDisposable);

            DoingTask = _Model.DoingTask
                              .Select(t => new WorkingTimeCardViewModel(t))
                              .ToReactiveProperty()
                              .AddTo(CompositeDisposable);

            Initialize();          
        }

        public void Initialize()
        {
            _Model.Load();
        }

        public async void ExecuteNewTaskDialog()
        {
            var editDialogVm = new TaskEditDialogViewModel();

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new TaskEditDialog
            {
                DataContext = editDialogVm
            };

            //show the dialog
            var result = (bool?)await DialogHost.Show(view, ClosingEventHandler);

            if(result.HasValue && result.Value)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                _Model.AddWorkTask(inputValue);
            }
        }

        public async void EditWorkTask(TaskCardViewModel taskCardViewModel)
        {
            var targetData = _Model.SelectWorkTask(taskCardViewModel.DomainModel.Id);

            var editDialogVm = new TaskEditDialogViewModel(targetData);

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new TaskEditDialog
            {
                DataContext = editDialogVm
            };

            //show the dialog
            var result = (bool?)await DialogHost.Show(view, ClosingEventHandler);

            if (result.HasValue && result.Value)
            {
                var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                _Model.EditWorkTask(inputValue);
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        public void StartWorkTask(TaskCardViewModel taskCardViewModel)
        {
            var workingTime = taskCardViewModel.DomainModel.StartTask();
            _Model.AddWorkingTime(workingTime);
        }

    }
}
