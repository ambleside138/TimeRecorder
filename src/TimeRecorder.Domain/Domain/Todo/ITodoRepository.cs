using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Todo
{
    public interface ITodoRepository : IRepository
    {
        TodoItem[] Select();

        TodoItem[] SelectWithCompleted();

        TodoItemIdentity Add(TodoItem item);

        void Edit(TodoItem item);

        void Delete(TodoItemIdentity id);

        TodoItem[] SelectByListId(TodoListIdentity id);
    }
}
