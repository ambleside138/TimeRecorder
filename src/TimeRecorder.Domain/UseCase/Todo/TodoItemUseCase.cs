using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Domain.UseCase.Todo;

public class TodoItemUseCase
{
    private readonly ITodoItemRepository _TodoRepository;

    public TodoItemUseCase(ITodoItemRepository todoRepository)
    {
        _TodoRepository = todoRepository;
    }

    public async Task<TodoItemIdentity> AddAsync(TodoItem todoItem)
    {
        return await _TodoRepository.AddAsync(todoItem);
    }

    public async Task EditAsync(TodoItem todoItem)
    {
        await _TodoRepository.EditAsync(todoItem);
    }

    public async Task DeleteAsync(TodoItemIdentity todoId)
    {
        await _TodoRepository.DeleteAsync(todoId);
    }

    public async Task<TodoItem[]> SelectAsync()
    {
        return await _TodoRepository.SelectAsync();
    }

}
