using Livet;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Editor;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks;
using TimeRecorder.Contents.WorkUnitRecorder.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Helpers;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Archive
{
    public class ArchiveManagerViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new() { Title = "アーカイブ", IconKey = "Archive" };

        private readonly ArchiveManagerModel _Model = new();
        private readonly WorkTaskModel _WorkTaskModel = new();

        public ReactiveProperty<DateTime> TargetDateTime { get; }

        public ReadOnlyReactiveCollection<WorkingTimeRecordForReport> WorkingTimeRecords { get; }

        // もっと良い書き方があるはずだが...
        public ReadOnlyReactivePropertySlim<bool> NoResults { get; set; }
        public ReactivePropertySlim<bool> IsSelected { get; } = new();

        public ArchiveManagerViewModel()
        {
            TargetDateTime = _Model.TargetDate
                                   .ToReactivePropertyAsSynchronized(d => d.Value)
                                   .AddTo(CompositeDisposable);

            WorkingTimeRecords = _Model.DailyWorkRecordHeaders
                                        .ToReadOnlyReactiveCollection()
                                        .AddTo(CompositeDisposable);

            NoResults = _Model.NoResults.ToReadOnlyReactivePropertySlim();
        }

        public void EditWorkTask(WorkingTimeRecordForReport record)
        {
            var targetData = _WorkTaskModel.SelectWorkTask(record.WorkTaskId);

            var editDialogVm = new WorkTaskEditDialogViewModel(targetData);
            editDialogVm.ShowDeleteButton.Value = false;

            var result = TransitionHelper.Current.TransitionModal<TaskEditDialog>(editDialogVm);

            if (result == ModalTransitionResponse.Yes)
            {
                if (editDialogVm.NeedDelete)
                {
                    _WorkTaskModel.DeleteWorkTask(record.WorkTaskId);
                }
                else
                {
                    var inputValue = editDialogVm.TaskCardViewModel.DomainModel;
                    _WorkTaskModel.EditWorkTask(inputValue);
                }

                _Model.Load();
            }
        }

        public async void EditWorkTaskTime(WorkingTimeRecordForReport record)
        {
            var editDialogVm = new WorkingTimeRangeEditDialogViewModel(record.ConvertToWorkingTimeRange());

            var view = new WorkingTimeRangeEditDialog
            {
                DataContext = editDialogVm
            };

            //show the dialog
            var result = (bool?)await DialogHost.Show(view);

            if (result.HasValue && result.Value)
            {
                var editObj = editDialogVm.WorkingTimeViewModel.DomainModel;
                _WorkTaskModel.EditWorkingTime(editObj);

                _Model.Load();
            }
        }
    }
}
