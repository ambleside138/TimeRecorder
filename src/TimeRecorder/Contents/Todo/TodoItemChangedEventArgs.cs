using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Contents.Todo
{
    class TodoItemChangedEventArgs
    {
        public TodoItemIdentity TodoItemIdentity { get; }

        public TodoItemChangedEventArgs(TodoItemIdentity todoItemIdentity)
        {
            TodoItemIdentity = todoItemIdentity;
        }
    }
}
