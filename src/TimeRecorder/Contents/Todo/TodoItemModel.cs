using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Domain.UseCase.Todo;

namespace TimeRecorder.Contents.Todo
{
    class TodoItemModel
    {
        private readonly TodoItem _DomainModel;

        private readonly TodoUseCase _TodoUseCase;


        public TodoItemModel(TodoItem todoItem)
        {
            _DomainModel = todoItem;
            _TodoUseCase = new TodoUseCase(ContainerHelper.Resolver.Resolve<ITodoRepository>());
        }

        public async void UpdateAsync()
        {

        }

        public async void DeleteAsync()
        {
        // メッセージ機構でTodoModelに通知したい
        // https://github.com/Cysharp/MessagePipe
        }
    }
}
