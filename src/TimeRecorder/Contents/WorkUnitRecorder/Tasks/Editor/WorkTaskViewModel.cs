using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain;
using TimeRecorder.Helpers;
using TimeRecorder.Domain.Domain.Segments;

namespace TimeRecorder.Contents.WorkUnitRecorder;

/// <summary>
/// タスク（編集機能有）のViewModelを表します
/// </summary>
public class WorkTaskViewModel : ViewModel
{
    public ReactiveProperty<string> Title { get; }

    public ReactiveProperty<TaskCategory> TaskCategory { get; }

    public ReactiveProperty<Product> Product { get; }

    public ReactiveProperty<Client> Client { get; }

    public ReactiveProperty<WorkProcess> WorkProcess { get; }

    public ReactiveProperty<Segment> Segment { get; set; }

    public WorkTask DomainModel { get; }

    private readonly WorkProcess[] _Processes;

    private readonly Client[] _Clients;

    private readonly Product[] _Products;

    private readonly Segment[] _Segments;

    public WorkTaskViewModel(WorkTask task, WorkProcess[] processes, Client[] clients, Product[] products, Segment[] segments)
    {
        DomainModel = task;
        _Processes = processes;
        _Clients = clients;
        _Products = products;
        _Segments = segments;

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
                                    vm => vm?.Id ?? Identity<WorkProcess>.Empty,
                                    ReactivePropertyMode.IgnoreInitialValidationError)
                                 .SetValidateNotifyError(x => (x == null || x.Id == Identity<WorkProcess>.Empty) ? "工程は選択必須です" : null)
                                 .AddTo(CompositeDisposable);

        Client = DomainModel.ToReactivePropertyAsSynchronized(
                                    x => x.ClientId,
                                    m => _Clients.FirstOrDefault(p => p.Id == m),
                                    vm => vm?.Id ?? Identity<Client>.Empty)
                                 .AddTo(CompositeDisposable);

        Segment = DomainModel.ToReactivePropertyAsSynchronized(
                                    x => x.SegmentId,
                                    m => _Segments.FirstOrDefault(p => p.Id == m),
                                    vm => vm?.Id ?? Identity<Segment>.Empty,
                                    ReactivePropertyMode.IgnoreInitialValidationError)
                                 .SetValidateNotifyError(x => (x == null || x.Id == Identity<Segment>.Empty) ? "セグメントは選択必須です" : null)
                                 .AddTo(CompositeDisposable);

        if (task.Id.IsTemporary
            && DomainModel.TaskCategory == Domain.Domain.Tasks.TaskCategory.UnKnown)
        {
            DomainModel.TaskCategory = Domain.Domain.Tasks.TaskCategory.Develop;
        }

        if (task.Id.IsTemporary
            && DomainModel.SegmentId.IsEmpty)
        {
            DomainModel.SegmentId = _Segments.FirstOrDefault()?.Id ?? Identity<Segment>.Empty;
        }
    }

    public bool TryValidate()
    {
        var result = true;

        // TODO: 検証対象をListでまとめてぐるぐる処理したいが...どう実装していいのかわからない
        Title.ForceValidate();
        result &= Title.HasErrors == false;

        WorkProcess.ForceValidate();
        result &= WorkProcess.HasErrors == false;

        return result;
    }


}
 