using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Contents.Todo
{
    class TodoItemDoneFilterViewModel : TodoItemViewModel
    {
        public TodoItemDoneFilterViewModel(TodoItem item) : base(item)
        {
            IsDoneFilter = true;
        }
    }
}
