using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkTaskViewModel : ViewModel
    {
        public ReactiveProperty<string> Title { get; }

        public ReactiveProperty<TaskCategory> TaskCategory { get; }

        public ReactiveProperty<Product> Product { get; }

        public ReactiveProperty<Client> Client { get; }

        public ReactiveProperty<WorkProcess> WorkProcess { get; }

        //public ReactiveProperty<string> Remarks { get; }

        public WorkTask DomainModel { get; }

        private readonly WorkProcess[] _Processes;

        private readonly Client[] _Clients;

        private readonly Product[] _Products;

        public WorkTaskViewModel(WorkTask task, WorkProcess[] processes, Client[] clients, Product[] products)
        {
            DomainModel = task;
            _Processes = processes;
            _Clients = clients;
            _Products = products;

            Title = DomainModel.ToReactivePropertyWithIgnoreInitialValidationError(x => x.Title)
                                .SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? "タイトルは入力必須です" : null)
                                .AddTo(CompositeDisposable);

            TaskCategory = DomainModel.ToReactivePropertyAsSynchronized(x => x.TaskCategory)
                                      .AddTo(CompositeDisposable);

            Product = DomainModel.ToReactivePropertyAsSynchronized(
                                        x => x.ProductId,
                                        m => _Products.FirstOrDefault(p => p.Id == m),
                                        vm => vm?.Id ?? Identity<Product>.Empty)
                                     .AddTo(CompositeDisposable);

            WorkProcess = DomainModel.ToReactivePropertyAsSynchronized(
                                        x => x.ProcessId,
                                        m => _Processes.FirstOrDefault(p => p.Id == m),
                                        vm => vm?.Id ?? Identity<WorkProcess>.Empty)
                                     .AddTo(CompositeDisposable);

            Client = DomainModel.ToReactivePropertyAsSynchronized(
                                        x => x.ClientId,
                                        m => _Clients.FirstOrDefault(p => p.Id == m),
                                        vm => vm?.Id ?? Identity<Client>.Empty)
                                     .AddTo(CompositeDisposable);

            //Remarks = DomainModel.ToReactivePropertyWithIgnoreInitialValidationError(x => x.Remarks)
            //                      .SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? "備考は入力必須です" : null)
            //                      .AddTo(CompositeDisposable);

        }
    }
}
 