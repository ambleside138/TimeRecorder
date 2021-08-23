using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Repository.InMemory
{
    public class InMemoryTodoRepository :  RepositoryBase<TodoItem, TodoItemIdentity>, ITodoRepository
    {
        public TodoItem[] SelectByListId(TodoListIdentity id)
        {
            throw new NotImplementedException();
        }

        public TodoItem[] SelectWithCompleted()
        {
            throw new NotImplementedException();
        }

        TodoItemIdentity ITodoRepository.Add(TodoItem item)
        {
            var clone = TodoItem.Rebuild(new TodoItemIdentity(Guid.NewGuid().ToString()), item);
            Add(clone);
            return clone.Id;
        }
    }
}
