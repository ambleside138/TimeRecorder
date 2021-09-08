using Livet;
using MaterialDesignThemes.Wpf;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Contents.Todo.TodoItems
{
    /// <summary>
    /// タスク項目のViewModelを表します
    /// </summary>
    public class TodoItemViewModel : ViewModel
    {
        public bool IsDoneFilter { get; init; }

        public ReactivePropertySlim<string> TemporaryTitle { get; }

        public ReactivePropertySlim<bool> IsSelected { get; } = new();

        private readonly TodoItemModel _TodoItemModel;

        public ReactiveProperty<bool> IsCompleted { get; }

        public ReadOnlyReactivePropertySlim<bool> IsImportant { get; }

        public ReactivePropertySlim<string> ImportantToggleDescription { get; } = new();

        public ReactivePropertySlim<PackIconKind> ImportantToggleIcon { get; } = new();

        private readonly ISubscriber<TodoItemFilterEventArgs> _Subscriber;


        public ReactivePropertySlim<bool> IsVisible { get; } = new ReactivePropertySlim<bool>(true);

        private TodoItemFilterEventArgs _CurrentFilterCondition;

        #region SortValue変更通知プロパティ
        private string _SortValue;

        public string SortValue
        {
            get => _SortValue;
            set => RaisePropertyChangedIfSet(ref _SortValue, value);
        }
        #endregion


        public TodoItemViewModel(TodoItem item, ISubscriber<TodoItemFilterEventArgs> subscriber)
        {
            _TodoItemModel = new TodoItemModel(item, ContainerHelper.Provider.GetRequiredService<IPublisher<TodoItemChangedEventArgs>>());

            TemporaryTitle = new ReactivePropertySlim<string>(item.Title);

            IsCompleted = item.ObserveProperty(i => i.IsCompleted)
                              .ToReactiveProperty(initialValue: item.IsCompleted, mode: ReactivePropertyMode.DistinctUntilChanged)
                              .AddTo(CompositeDisposable);

            IsCompleted.Subscribe(_ => ToggleCompletedAsync(IsCompleted.Value) )
                       .AddTo(CompositeDisposable);

            IsImportant = item.ObserveProperty(i => i.IsImportant)
                              .ToReadOnlyReactivePropertySlim()
                              .AddTo(CompositeDisposable);

            IsImportant.Subscribe(important =>
            {
                ImportantToggleDescription.Value = important ? "重要度の削除" : "重要としてマークする";
                ImportantToggleIcon.Value = important ? PackIconKind.Star : PackIconKind.StarOutline;
            }).AddTo(CompositeDisposable);

            _Subscriber = subscriber;
            _Subscriber?.Subscribe(args => Filter(args))
                       .AddTo(CompositeDisposable);

            SetSortValue();
        }

        public async void ToggleImportantAsync() => await _TodoItemModel.ToggleImportantAsync();

        public async void ToggleCompletedAsync(bool completed)
        {
            SetSortValue();
            Filter(_CurrentFilterCondition);
            await _TodoItemModel.ToggleCompletedAsync(completed);
        }

        private void Filter(TodoItemFilterEventArgs args)
        {
            if (args == null)
                return;

            _CurrentFilterCondition = args;
            IsVisible.Value = args.NeedShowDoneItem ? true : IsCompleted.Value == false;
        }

        private void SetSortValue()
        {
            // 未完了
            // 完了フィルタ
            // 完了済み
            // の順になるように調整

            var ymd = "";

            // type_yyyyMMddHHmmss
            string type;

            if (_TodoItemModel.DomainModel.IsDoneFilter)
            {
                type = "02";
            }
            else
            {
                if (IsCompleted.Value)
                {
                    type = "03";
                }
                else
                {
                    type = "01";
                }
            }

            SortValue = $"{type}_{ymd}";
        }
    }
}
