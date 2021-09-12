using Livet;
using MaterialDesignThemes.Wpf;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Contents.Shared;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Contents.Todo.TodoItems
{
    /// <summary>
    /// タスク項目のViewModelを表します
    /// </summary>
    public class TodoItemViewModel : ViewModel, IEquatable<TodoItemViewModel>
    {
        private readonly TodoItemModel _TodoItemModel;

        private readonly ISubscriber<TodoItemFilterEventArgs> _Subscriber;

        private readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;

        public TodoItemIdentity Identity => _TodoItemModel.DomainModel.Id;

        public bool IsDoneFilter { get; init; }

        public ReactivePropertySlim<string> TemporaryTitle { get; }

        public ReactivePropertySlim<bool> IsSelected { get; } = new();

        public ReactiveProperty<bool> IsCompleted { get; }

        public ReadOnlyReactivePropertySlim<bool> IsImportant { get; }

        public ReactivePropertySlim<string> ImportantToggleDescription { get; } = new();

        public ReactivePropertySlim<PackIconKind> ImportantToggleIcon { get; } = new();

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

        public ReadOnlyReactivePropertySlim<string> CreateTimeText { get; }

        public TodoItemViewModel(TodoItem item, ISubscriber<TodoItemFilterEventArgs> subscriber)
        {
            _TodoItemModel = new TodoItemModel(item, ContainerHelper.Provider.GetRequiredService<IPublisher<TodoItemChangedEventArgs>>());

            TemporaryTitle = new ReactivePropertySlim<string>(item.Title);
            TemporaryTitle.Subscribe(t =>
            {
                if (t == _TodoItemModel.DomainModel.Title)
                    return;

                _TodoItemModel.DomainModel.Title = TemporaryTitle.Value;
                UpdateAsync();
            }).AddTo(CompositeDisposable);


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


            // ★DateTimeの変更を検知してstring に変換する
            CreateTimeText = item.ObserveProperty(i => i.IsCompleted)
                                 .ToReactiveProperty()
                                 .Select(i => i ? "に完了済み" : "作成: ")
                                 .ToReadOnlyReactivePropertySlim()
                                 .AddTo(CompositeDisposable);

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

        public async void UpdateAsync() => await _TodoItemModel.UpdateAsync();

        public async void Delete()
        {
            // 確認してから削除する
            var view = new ConfirmDialog
            {
                DataContext = new ConfirmDialogViewModel("タスクの削除", $"{_TodoItemModel.DomainModel.Title}は完全に削除されます。", "削除", "キャンセル")
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog");

            if((bool)result)
            {
                IsSelected.Value = false;
                await _TodoItemModel.DeleteAsync();
            }
        }

        private string ConvertToText()
        {
            TimeSpan diff = _SystemClock.Now - _TodoItemModel.DomainModel.UpdatedAt;

            if(diff.TotalMinutes < 5)
            {
                return "数分前";
            }
            else
            {
                return _TodoItemModel.DomainModel.UpdatedAt.ToString("M月d日（ddd）");
            }
        }

        private void Filter(TodoItemFilterEventArgs args)
        {
            if (args == null)
                return;

            _CurrentFilterCondition = args;
            IsVisible.Value = args.NeedShowDoneItem ? true : IsCompleted.Value == false;
        }

        public void Select() => IsSelected.Value = true;

        public void ClearSelection() => IsSelected.Value = false;


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

        public override bool Equals(object obj) => obj is TodoItemViewModel model && EqualityComparer<TodoItemIdentity>.Default.Equals(Identity, model.Identity);
        public override int GetHashCode() => HashCode.Combine(Identity);
        bool IEquatable<TodoItemViewModel>.Equals(TodoItemViewModel other) => Equals(other);
    }
}
