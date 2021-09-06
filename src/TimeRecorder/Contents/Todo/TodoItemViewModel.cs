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

namespace TimeRecorder.Contents.Todo
{
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

        public TodoItemViewModel(TodoItem item)
        {
            _TodoItemModel = new TodoItemModel(item, ContainerHelper.Provider.GetRequiredService<IPublisher<TodoItemChangedEventArgs>>());

            TemporaryTitle = new ReactivePropertySlim<string>(item.Title);

            IsCompleted = item.ObserveProperty(i => i.IsCompleted)
                              .ToReactiveProperty(initialValue: item.IsCompleted, mode: ReactivePropertyMode.DistinctUntilChanged)
                              .AddTo(CompositeDisposable);

            IsCompleted.Subscribe(_ => ToggleImportantAsync(IsCompleted.Value) )
                       .AddTo(CompositeDisposable);

            IsImportant = item.ObserveProperty(i => i.IsImportant)
                              .ToReadOnlyReactivePropertySlim()
                              .AddTo(CompositeDisposable);

            IsImportant.Subscribe(important =>
            {
                ImportantToggleDescription.Value = important ? "重要度の削除" : "重要としてマークする";
                ImportantToggleIcon.Value = important ? PackIconKind.Star : PackIconKind.StarOutline;
            }).AddTo(CompositeDisposable);
        }

        public async void ToggleCompletedAsync() => await _TodoItemModel.ToggleImportantAsync();

        public async void ToggleImportantAsync(bool completed) => await _TodoItemModel.ToggleCompletedAsync(completed);
    }
}
