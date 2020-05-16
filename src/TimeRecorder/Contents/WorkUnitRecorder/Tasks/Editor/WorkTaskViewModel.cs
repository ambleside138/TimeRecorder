using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkTaskViewModel : ViewModel
    {
        public ReactiveProperty<string> Title { get; }

        public ReactiveProperty<TaskCategory> TaskCategory { get; }

        public ReactiveProperty<Product> Product { get; }

        public ReactiveProperty<Client> User { get; }

        public ReactiveProperty<Process> Process { get; }

        public ReactiveProperty<string> Remarks { get; }

        public WorkTask DomainModel { get; }

        public WorkTaskViewModel(WorkTask task)
        {
            DomainModel = task;

            Title = DomainModel.ToReactivePropertyWithIgnoreInitialValidationError(x => x.Title)
                                .SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? "タイトルは入力必須です" : null)
                                .AddTo(CompositeDisposable);

            TaskCategory = DomainModel.ToReactivePropertyAsSynchronized(x => x.TaskCategory)
                                      .AddTo(CompositeDisposable);

            Product = DomainModel.ToReactivePropertyAsSynchronized(x => x.Product)
                                 .AddTo(CompositeDisposable);

            Remarks = DomainModel.ToReactivePropertyWithIgnoreInitialValidationError(x => x.Remarks)
                                  .SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? "備考は入力必須です" : null)
                                  .AddTo(CompositeDisposable);

        }
    }
}
 