using Livet;
using MaterialDesignThemes.Wpf;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Data;
using System.Windows.Input;
using TimeRecorder.Contents.Shared;
using TimeRecorder.Contents.Todo.TodoItems;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Todo;

public class TodoViewModel : ViewModel, IContentViewModel
{
    public NavigationIconButtonViewModel NavigationIcon => new() { Title = "Todo", IconKey = "CheckAll" };

    public ReadOnlyReactiveCollection<NavigationIconButtonViewModel> NavigationItems { get; }


    private readonly TodoModel _Model = new(ContainerHelper.Provider.GetRequiredService<ISubscriber<TodoItemChangedEventArgs>>());


    public ListCollectionView SortedTodoItems { get; }

    public ReactivePropertySlim<string> NewTodoTitle { get; } = new();

    public ReadOnlyReactiveCollection<TodoItemViewModel> TodoItems { get; }

    public TodoList CurrentTodoList
        => CurrentTodoListViewModel?.TodoList;

    public TodoListNavigationItemViewModel CurrentTodoListViewModel
        => NavigationItems.OfType<TodoListNavigationItemViewModel>().FirstOrDefault(i => i.IsSelected);

    public ReadOnlyReactivePropertySlim<LoginStatus> LoginStatus { get; }

    public ReactivePropertySlim<bool> IsProcessing { get; } = new();

    public ReactivePropertySlim<bool> IsSelected { get; } = new();

    public ReactivePropertySlim<int> SelectedListIndex { get; } = new();

    private bool _IsInitialized = false;

    public ReactiveCommand DeleteTaskListCommand { get; } = new();
    public ReactiveCommand EditTaskListTitleCommand { get; } = new();
    public ReactiveCommand LogoutCommand { get; } = new();

    public ReadOnlyReactivePropertySlim<bool> IsSignined { get; }

    public TodoViewModel()
    {
        NavigationItems = _Model.TodoListCollection
                                .ToReadOnlyReactiveCollection(i =>
                                {
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
                          .ToReadOnlyReactiveCollection(i =>
                          {
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

        LoginStatus = _Model.ObserveProperty(m => m.LoginStatus)
                            .ToReadOnlyReactivePropertySlim()
                            .AddTo(CompositeDisposable);

        IsSelected.Subscribe(i => Handler(i)).AddTo(CompositeDisposable);
        SelectedListIndex.Subscribe(i => Handler(IsSelected.Value)).AddTo(CompositeDisposable);

        DeleteTaskListCommand.Subscribe(() => DeleteTodoListAsync())
                             .AddTo(CompositeDisposable);

        EditTaskListTitleCommand.Subscribe(() => CurrentTodoListViewModel.FocusToTitleTextbox())
                                .AddTo(CompositeDisposable);

        LogoutCommand.Subscribe(() => _Model.Logout())
                     .AddTo(CompositeDisposable);

        IsSignined = _Model.ObserveProperty(m => m.LoginStatus)
                           .Select(s => s != null)
                           .ToReadOnlyReactivePropertySlim()
                           .AddTo(CompositeDisposable);
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
        if (isselected)
        {

            try
            {
                IsProcessing.Value = true;

                if (_IsInitialized == false)
                {
                    await _Model.InitializeIfNeededAsync();

                    _IsInitialized = true;
                }

                if (CurrentTodoList == null)
                {
                    NavigationItems.First(i => i.IsSelectable).IsSelected = true;
                }
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
        CurrentTodoList.SetDefaultItemProperties(item);

        if (string.IsNullOrEmpty(item.Title))
            return;

        try
        {
            IsProcessing.Value = true;

            NewTodoTitle.Value = "";

            var id = await _Model.AddTodoItemAsync(CurrentTodoList.Id, item);

            ClearTodoItemSelection();
            NavigationItems.OfType<TodoListNavigationItemViewModel>().FirstOrDefault(i => i.IsSelected)?.FocusToAdditionTextbox();
        }
        finally
        {
            IsProcessing.Value = false;
        }

    }

    public void AddTodoList()
    {
        _ = _Model.AddTodoListAsync();
    }

    private void ClearTodoItemSelection()
    {
        foreach (var item in TodoItems)
        {
            item.ClearSelection();
        }
    }

    public async void SetTodoListTitleAsync()
    {
        var current = NavigationItems.OfType<TodoListNavigationItemViewModel>().FirstOrDefault(i => i.IsSelected);

        if (current.TodoList.Id.IsFixed)
            return;

        current.TodoList.Title = current.Title;

        await _Model.SetTodoListTitleAsync(current.TodoList);

        Keyboard.ClearFocus();
        current.FocusToAdditionTextbox();
    }

    private async void DeleteTodoListAsync()
    {
        var current = CurrentTodoList;
        if (current.Id.IsFixed)
            return;

        // 確認してから削除する
        var view = new ConfirmDialog
        {
            DataContext = new ConfirmDialogViewModel("リストを削除", $"{current.Title}は完全に削除されます。", "削除", "キャンセル")
        };

        //show the dialog
        var result = await DialogHost.Show(view, "RootDialog");

        if ((bool)result)
        {
            NavigationItems.First().IsSelected = true;
            await _Model.DeleteTodoListAsync(current);
        }
    }

    public async void Login()
    {
        await _Model.InitializeAsync();
    }

}
