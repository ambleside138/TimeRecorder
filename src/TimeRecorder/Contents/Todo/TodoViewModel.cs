﻿using Livet;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using TimeRecorder.Contents.Todo.TodoItems;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Todo
{
    public class TodoViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new() { Title = "Todo", IconKey = "CheckAll" };

        public ReadOnlyReactiveCollection<NavigationIconButtonViewModel> NavigationItems { get; }


        private readonly TodoModel _Model = new(ContainerHelper.Provider.GetRequiredService<ISubscriber<TodoItemChangedEventArgs>>());


        public ListCollectionView SortedTodoItems { get; }

        public ReactivePropertySlim<string> NewTodoTitle { get; } = new();

        public ReadOnlyReactiveCollection<TodoItemViewModel> TodoItems { get; }

        public TodoList CurrentTodoList 
            => NavigationItems.OfType<TodoListNavigationItemViewModel>().FirstOrDefault(i => i.IsSelected)?.TodoList;

        public ReactivePropertySlim<LoginStatus> LoginStatus { get; }

        public ReactivePropertySlim<bool> IsProcessing { get; } = new();

        public ReactivePropertySlim<bool> IsSelected { get; } = new();

        public ReactivePropertySlim<int> SelectedListIndex { get; } = new();


        public TodoViewModel()
        {
            NavigationItems = _Model.TodoListCollection
                                    .ToReadOnlyReactiveCollection(i => {
                                        if (i.Id == TodoListIdentity.Divider)
                                        {
                                            return (NavigationIconButtonViewModel)new DividerNavigationItemViewModel();
                                        }
                                        else
                                        {
                                            return (NavigationIconButtonViewModel)new TodoListNavigationItemViewModel(i);
                                        }
                                      })
                                    .AddTo(CompositeDisposable);

            TodoItems = _Model.FilteredTodoItems
                              .ToReadOnlyReactiveCollection(i => {
                                  if (i.Id == TodoItemIdentity.DoneFilter)
                                  {
                                      return new TodoItemDoneFilterViewModel(i, ContainerHelper.Provider.GetRequiredService<IPublisher<TodoItemFilterEventArgs>>());
                                  }
                                  else
                                      return new TodoItemViewModel(i, ContainerHelper.Provider.GetRequiredService<ISubscriber<TodoItemFilterEventArgs>>());
                              })
                              .AddTo(CompositeDisposable);

            SetLiveSorting();

            NavigationItems.First().IsSelected = true;

            LoginStatus = _Model.ToReactivePropertySlimAsSynchronized(m => m.LoginStatus)
                                .AddTo(CompositeDisposable);

            IsSelected.Subscribe(i => Handler(i)).AddTo(CompositeDisposable);
            SelectedListIndex.Subscribe(i => Handler(IsSelected.Value)).AddTo(CompositeDisposable);
        }

        private void SetLiveSorting()
        {
            // Salaryプロパティでソート
            var view = CollectionViewSource.GetDefaultView(TodoItems);
            view.SortDescriptions.Add(new SortDescription(nameof(TodoItemViewModel.SortValue), ListSortDirection.Ascending));

            // Salaryのソートをリアルタイムソートに設定する
            if (view is not ICollectionViewLiveShaping liveShaping)
            {
                // ICollectionViewLiveShapingを実装していない場合は何もしない
                return;
            }

            // リアルタイムソートをサポートしているか確認する
            if (liveShaping.CanChangeLiveSorting)
            {
                // リアルタイムソートをサポートしている場合は対象のプロパティにSalaryを追加して
                // リアルタイムソートを有効にする。
                liveShaping.LiveSortingProperties.Add(nameof(TodoItemViewModel.SortValue));
                liveShaping.IsLiveSorting = true;
            }
        }


        private async void Handler(bool isselected)
        {
            if(isselected)
            {

                try
                {
                    IsProcessing.Value = true;
                    await _Model.LoadTodoItemsAsync(CurrentTodoList.Id);
                }
                finally
                {
                    IsProcessing.Value = false;
                }
            }
        }




        // ViewからのEnterキー押下で呼び出し
        public async void AddTodoItemAsync()
        {
            var item = TodoItem.ForNew();
            item.Title = NewTodoTitle.Value;

            if (string.IsNullOrEmpty(item.Title))
                return;

            try
            {
                IsProcessing.Value = true;
                
                NewTodoTitle.Value = "";

                var id = await _Model.AddTodoItemAsync(CurrentTodoList.Id, item);

                ClearTodoItemSelection();
                TodoItems.FirstOrDefault(i => i.Identity == id)?.Select();
            }
            finally
            {
                IsProcessing.Value = false;
            }

        }

        public void AddTodoList()
        {

        }

        private void ClearTodoItemSelection()
        {
            foreach(var item in TodoItems)
            {
                item.ClearSelection();
            }
        }


    }
}
