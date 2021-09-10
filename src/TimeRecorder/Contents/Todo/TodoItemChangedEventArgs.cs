using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Contents.Todo
{
    enum ChangeType
    {
        Updated,
        Deleted,
    }

    class TodoItemChangedEventArgs
    {
        public TodoItemIdentity TodoItemIdentity { get; }

        public ChangeType ChangeType { get; }


        public TodoItemChangedEventArgs(ChangeType changeType, TodoItemIdentity todoItemIdentity)
        {
            ChangeType = changeType;
            TodoItemIdentity = todoItemIdentity;
        }
    }
}
