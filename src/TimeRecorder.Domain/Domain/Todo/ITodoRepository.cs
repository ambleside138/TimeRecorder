using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Todo
{
    public interface ITodoRepository : IRepository
    {
        Task<TodoItem[]> SelectAsync();

        //TodoItem[] SelectWithCompleted();

        Task<TodoItemIdentity> AddAsync(TodoItem item);

        Task EditAsync(TodoItem item);

        Task DeleteAsync(TodoItemIdentity id);

        //TodoItem[] SelectByListId(TodoListIdentity id);
    }
}
