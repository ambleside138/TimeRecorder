using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Contents.Todo
{
    public class TodoItemViewModel
    {
        public TodoItem DomainModel { get; }

        public ReactivePropertySlim<string> TemporaryTitle { get; }

        public ReactivePropertySlim<bool> IsSelected { get; } = new();

        public TodoItemViewModel(TodoItem item)
        {
            DomainModel = item;

            TemporaryTitle = new ReactivePropertySlim<string>(item.Title);
        }
    }
}
