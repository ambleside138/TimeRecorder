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

        async Task<TodoItemIdentity> ITodoRepository.AddAsync(TodoItem item)
        {
            var clone = TodoItem.Rebuild(new TodoItemIdentity(Guid.NewGuid().ToString()), item);
            Add(clone);
            await Task.Delay(100);
            return clone.Id;
        }

        Task ITodoRepository.DeleteAsync(TodoItemIdentity id)
        {
            throw new NotImplementedException();
        }

        Task ITodoRepository.EditAsync(TodoItem item)
        {
            throw new NotImplementedException();
        }

        Task<TodoItem[]> ITodoRepository.SelectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
