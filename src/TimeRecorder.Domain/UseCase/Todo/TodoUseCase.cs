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

        public TodoItemIdentity Add(TodoItem workTask)
        {
            return _TodoRepository.Add(workTask);
        }

        public void Edit(TodoItem workTask)
        {
            _TodoRepository.Edit(workTask);
        }

        public void Delete(TodoItemIdentity todoId)
        {
            _TodoRepository.Delete(todoId);
        }

        public TodoItem[] Select()
        {
            return _TodoRepository.Select();
        }

    }
}
