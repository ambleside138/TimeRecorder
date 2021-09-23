using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Repository.InMemory
{
    public class InMemoryTodoRepository :  RepositoryBase<TodoItem, TodoItemIdentity>, ITodoItemRepository
    {
        public TodoItem[] SelectByListId(TodoListIdentity id)
        {
            throw new NotImplementedException();
        }

        public TodoItem[] SelectWithCompleted()
        {
            throw new NotImplementedException();
        }

        async Task<TodoItemIdentity> ITodoItemRepository.AddAsync(TodoItem item)
        {
            var clone = TodoItem.Rebuild(new TodoItemIdentity(Guid.NewGuid().ToString()), item);
            Add(clone);
            await Task.Delay(100);
            return clone.Id;
        }

        Task ITodoItemRepository.DeleteAsync(TodoItemIdentity id)
        {
            throw new NotImplementedException();
        }

        Task ITodoItemRepository.EditAsync(TodoItem item)
        {
            throw new NotImplementedException();
        }

        Task<TodoItem[]> ITodoItemRepository.SelectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
