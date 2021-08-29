using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Domain.UseCase.Todo
{
    public class TodoUseCase
    {
        private readonly ITodoRepository _TodoRepository;

        public TodoUseCase(ITodoRepository todoRepository)
        {
            _TodoRepository = todoRepository;
        }

        public async Task<TodoItemIdentity> AddAsync(TodoItem workTask)
        {
            return await _TodoRepository.AddAsync(workTask);
        }

        public async Task EditAsync(TodoItem workTask)
        {
            await _TodoRepository.EditAsync(workTask);
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
}
