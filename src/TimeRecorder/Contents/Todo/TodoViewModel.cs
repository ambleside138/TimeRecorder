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
        }

        // ViewからのEnterキー押下で呼び出し
        public void AddTodoItem()
        {
            var item = TodoItem.ForNew();
            item.Title = NewTodoTitle.Value;

            if (string.IsNullOrEmpty(item.Title))
                return;

            _Model.AddTodoItem(CurrentTodoList.Id, item);

            NewTodoTitle.Value = "";
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
