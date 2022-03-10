using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Todo;

public interface ITodoListRepository
{
    Task<TodoList[]> SelectAsync();

    Task<TodoListIdentity> AddAsync(TodoList item);

    Task EditAsync(TodoList item);

    Task DeleteAsync(TodoListIdentity id);
}
