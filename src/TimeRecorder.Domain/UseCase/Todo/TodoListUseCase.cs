using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Domain.UseCase.Todo;

public class TodoListUseCase
{
    private readonly ITodoListRepository _TodoRepository;

    public TodoListUseCase(ITodoListRepository todoRepository)
    {
        _TodoRepository = todoRepository;
    }

    public async Task<TodoListIdentity> AddAsync(TodoList todoList)
    {
        return await _TodoRepository.AddAsync(todoList);
    }

    public async Task EditAsync(TodoList todoList)
    {
        await _TodoRepository.EditAsync(todoList);
    }

    public async Task DeleteAsync(TodoListIdentity todoListId)
    {
        await _TodoRepository.DeleteAsync(todoListId);
    }

    public async Task<TodoList[]> SelectAsync()
    {
        var list = await _TodoRepository.SelectAsync();
        return list.OrderBy(i => i.DisplayOrder).ToArray();
    }
}
