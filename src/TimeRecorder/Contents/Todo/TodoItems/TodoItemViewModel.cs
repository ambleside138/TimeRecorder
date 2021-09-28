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
using TimeRecorder.Domain.Domain;
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

        public ReactivePropertySlim<bool> IsVisible { get; }

        private TodoItemFilterEventArgs _CurrentFilterCondition;

        #region SortValue変更通知プロパティ
        private string _SortValue;

        public string SortValue
        {
            get => _SortValue;
            set => RaisePropertyChangedIfSet(ref _SortValue, value);
        }
        #endregion

        public ReactiveCommand<string> PlanDateCommand { get; } = new();

        public ReactiveCommand ManualPlanDateCommand { get; } = new();

        public ReadOnlyReactivePropertySlim<string> CreateTimeText { get; }

        public string CreateTime { get; }

        public ReadOnlyReactivePropertySlim<bool> IsTodayTask { get; }

        public ReadOnlyReactivePropertySlim<string> PlanDateText { get; }

        public ReadOnlyReactivePropertySlim<bool> IsPastPlanDate { get; }


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

            IsVisible = new ReactivePropertySlim<bool>(item.IsCompleted == false);

            IsImportant = item.ObserveProperty(i => i.IsImportant)
                              .ToReadOnlyReactivePropertySlim()
                              .AddTo(CompositeDisposable);

            IsImportant.Subscribe(important =>
            {
                ImportantToggleDescription.Value = important ? "重要度の削除" : "重要としてマークする";
                ImportantToggleIcon.Value = important ? PackIconKind.Star : PackIconKind.StarOutline;
            }).AddTo(CompositeDisposable);


            // ★IsCompletedの変更を検知してstring に変換する
            CreateTimeText = item.ObserveProperty(i => i.IsCompleted)
                                 .ToReactiveProperty()
                                 .Select(i => IsCompleted.Value ? $"{ConvertToString(_TodoItemModel.DomainModel.CompletedDateTime.Value)} に完了済み" : $"作成: {ConvertToString(_TodoItemModel.DomainModel.CreatedAt)}")
                                 .ToReadOnlyReactivePropertySlim()
                                 .AddTo(CompositeDisposable);

            CreateTime = item.CreatedAt.ToString("作成: yyyy/MM/dd(ddd) HH:mm");

            IsTodayTask = item.TodayTaskDates
                              .CollectionChangedAsObservable()
                              .Select(i => item.IsTodayTask)
                              .ToReadOnlyReactivePropertySlim(initialValue:item.IsTodayTask)
                              .AddTo(CompositeDisposable);

            PlanDateText = item.ObserveProperty(i => i.PlanDate)
                               .ToReactiveProperty()
                               .Select(i => ConvertToPlanDateString(i))
                               .ToReadOnlyReactivePropertySlim(initialValue: ConvertToPlanDateString(item.PlanDate))
                               .AddTo(CompositeDisposable);

            PlanDateCommand.Subscribe(day => PlanToDateAsync(int.Parse(day)))
                           .AddTo(CompositeDisposable);

            ManualPlanDateCommand.Subscribe(() => ManualPlanToDateAsync())
                                 .AddTo(CompositeDisposable);

            IsPastPlanDate = item.ObserveProperty(i => i.PlanDate)
                                 .ToReactiveProperty()
                                 .Select(i => i.IsPastDate)
                                 .ToReadOnlyReactivePropertySlim(initialValue: item.PlanDate.IsPastDate)
                                 .AddTo(CompositeDisposable);

            _Subscriber = subscriber;
            _Subscriber?.Subscribe(args => Filter(args))
                       .AddTo(CompositeDisposable);

            SetSortValue();
        }

        private async void PlanToDateAsync(int day)
        {
            var targetDate = _SystemClock.Now.Date.AddDays(day);

            _TodoItemModel.DomainModel.PlanDate = new YmdString(targetDate);
            await _TodoItemModel.UpdateAsync();
        }

        private async void ManualPlanToDateAsync()
        {
            var vm = new DateTimePickerViewModel(_TodoItemModel.DomainModel.PlanDate.ToDateTime());
            // 確認してから削除する
            var view = new DateTimePickerView
            {
                DataContext = vm
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog");

            if (result?.ToString() == "1")
            {
                _TodoItemModel.DomainModel.PlanDate = new YmdString(vm.SelectedDate.Value);
                await _TodoItemModel.UpdateAsync();
            }
        }

        private string ConvertToString(DateTime dt)
        {
            var diff = _SystemClock.Now - dt;
            if (diff.TotalMinutes < 5)
            {
                return "数分前";
            }
            else if (diff.TotalMinutes < 60)
            {
                return (int)diff.TotalMinutes + "分前";
            }
            else if(diff.TotalHours < 24)
            {
                return (int)diff.TotalHours + "時間前";
            }
            else if(diff.TotalDays < 4)
            {
                return (int)diff.TotalDays + "日前";
            }
            else
            {
                return $"{dt:MM月dd日(ddd)}";
            }
        }

        private string ConvertToPlanDateString(YmdString planYmd)
        {
            DateTime? planDate = planYmd.ToDateTime();
            if (planDate.HasValue == false)
            {
                return "期限日の追加";
            }

            var diffDays = (int)(planDate.Value.Date - _SystemClock.Now.Date).TotalDays;

            return diffDays switch
            {
                -1 => "期限 昨日",
                 0 => "期限 今日",
                 1 => "期限 明日",
                 _ => planDate.Value.ToString("期限 yyyy/MM/dd (ddd)"),
            };
        }

        public async void ToggleImportantAsync() => await _TodoItemModel.ToggleImportantAsync();

        public async void ToggleCompletedAsync(bool completed)
        {
            await _TodoItemModel.ToggleCompletedAsync(completed);
            Filter(_CurrentFilterCondition);
            SetSortValue();
        }

        public async void UpdateAsync() => await _TodoItemModel.UpdateAsync();

        public async void ToggleTodayTaskAsync() => await _TodoItemModel.ToggleTodayTask();

        public async void ClearPlandateAsync()
        {
            _TodoItemModel.DomainModel.PlanDate = YmdString.Empty;
            await _TodoItemModel.UpdateAsync();
        }

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

        public void CloseDetailView()
        {
            IsSelected.Value = false;
        }

        private void Filter(TodoItemFilterEventArgs args)
        {
            if (args == null)
                return;

            _CurrentFilterCondition = args;
            IsVisible.Value = args.NeedShowDoneItem || IsCompleted.Value == false;
        }

        public void Select() => IsSelected.Value = true;

        public void ClearSelection() => IsSelected.Value = false;


        private void SetSortValue()
        {
            // 未完了
            // 完了フィルタ
            // 完了済み
            // の順になるように調整

            // type_yyyyMMddHHmmss

            string type;

            if (_TodoItemModel.DomainModel.IsDoneFilter)
            {
                type = $"02_yyyyMMddHHmmss";
            }
            else
            {
                if (_TodoItemModel.DomainModel.IsCompleted)
                {
                    type = $"03_{_TodoItemModel.DomainModel.CompletedDateTime.Value:yyyyMMddHHmmss}";
                }
                else
                {
                    type = $"01_{_TodoItemModel.DomainModel.CreatedAt:yyyyMMddHHmmss}";
                }
            }

            SortValue = type;
        }

        public override bool Equals(object obj) => obj is TodoItemViewModel model && EqualityComparer<TodoItemIdentity>.Default.Equals(Identity, model.Identity);
        public override int GetHashCode() => HashCode.Combine(Identity);
        bool IEquatable<TodoItemViewModel>.Equals(TodoItemViewModel other) => Equals(other);
    }
}
