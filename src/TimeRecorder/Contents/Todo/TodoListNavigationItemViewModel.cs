using Reactive.Bindings;
using Reactive.Bindings.Extensions;
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

        public ReadOnlyReactivePropertySlim<int> TaskCount { get; }

        public TodoListNavigationItemViewModel(TodoList todoList)
        {
            TodoList = todoList;

            IconKey = todoList.IconKey;
            Title = todoList.Title;

            TaskCount = todoList.ObserveProperty(i => i.FilteredCount)
                                .ToReadOnlyReactivePropertySlim(initialValue:todoList.FilteredCount)
                                .AddTo(CompositeDisposable);
        }

        public TodoList TodoList { get; }
    }
}
