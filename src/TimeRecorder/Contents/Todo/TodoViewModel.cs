using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Todo
{
    public class TodoViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new() { Title = "Todo", IconKey = "CheckAll" };

        public ReadOnlyReactiveCollection<TodoListNavigationItemViewModel> NavigationItems { get; }

        //public List<INavigationItem> SideMenuItems { get; } = new() { new NavigationIconButtonViewModel { Title = "ほんじつ" }, new DividerNavigationItemViewModel(), new NavigationIconButtonViewModel { Title = "重要" }, };

        private readonly TodoModel _Model = new();


        public ReactivePropertySlim<string> NewTodoTitle { get; } = new();

        public ReadOnlyReactiveCollection<TodoItemViewModel> TodoItems { get; }

        public TodoList CurrentTodoList 
            => NavigationItems.FirstOrDefault(i => i.IsSelected)?.TodoList;

        public ReactivePropertySlim<LoginStatus> LoginStatus { get; }

        public ReactivePropertySlim<bool> IsProcessing { get; } = new();

        public ReactivePropertySlim<bool> IsSelected { get; } = new();


        public TodoViewModel()
        {
            NavigationItems = _Model.TodoListCollection
                                    .ToReadOnlyReactiveCollection(i => new TodoListNavigationItemViewModel(i))
                                    .AddTo(CompositeDisposable);

            TodoItems = _Model.FilteredTodoItems
                              .ToReadOnlyReactiveCollection(i => {
                                  if (i.Id == TodoItemIdentity.DoneFilter)
                                  {
                                      return new TodoItemDoneFilterViewModel(i);
                                  }
                                  else
                                      return new TodoItemViewModel(i);
                              })
                              .AddTo(CompositeDisposable);

            NavigationItems.First().IsSelected = true;

            LoginStatus = _Model.ToReactivePropertySlimAsSynchronized(m => m.LoginStatus)
                                .AddTo(CompositeDisposable);

            IsSelected.Subscribe(i => Handler(i)).AddTo(CompositeDisposable);
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
                //await ActionAsync(() =>
                //{
                //    _Model.LoadTodoItemsAsync(CurrentTodoList.Id);
                //});
            }
        }


        private async void ActionAsync(Func<Task> action)
        {
            try
            {
                IsProcessing.Value = true;
                await action();
            }
            finally
            {
                IsProcessing.Value = false;
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

                await _Model.AddTodoItemAsync(CurrentTodoList.Id, item);
            }
            finally
            {
                IsProcessing.Value = false;
            }

        }

        public void CloseDetailView()
        {
            ClearTodoItemSelection();   
        }

        public void Delete()
        {
            var selectedItem = TodoItems.FirstOrDefault(i => i.IsSelected.Value);
            if (selectedItem == null)
                return;

            _Model.DeleteTodoItem(CurrentTodoList.Id, selectedItem.DomainModel.Id);
        }

        private void ClearTodoItemSelection()
        {
            foreach(var item in TodoItems)
            {
                item.IsSelected.Value = false;
            }
        }


    }
}
