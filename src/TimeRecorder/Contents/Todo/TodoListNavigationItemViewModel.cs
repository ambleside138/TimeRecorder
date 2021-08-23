using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Todo
{
    /// <summary>
    /// TodoリストをあらわすViewModelです
    /// </summary>
    public class TodoListNavigationItemViewModel : NavigationIconButtonViewModel
    {

        public ReactivePropertySlim<int> TaskCount { get; } = new();

        public TodoListNavigationItemViewModel(TodoList todoList)
        {
            TodoList = todoList;

            IconKey = todoList.IconKey;
            Title = todoList.Title;
        }

        public TodoList TodoList { get; }
    }
}
