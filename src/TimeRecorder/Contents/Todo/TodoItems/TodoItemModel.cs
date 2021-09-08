using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
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
        public TodoItem DomainModel { get; }


        private readonly TodoUseCase _TodoUseCase;

        private readonly IPublisher<TodoItemChangedEventArgs> _Publisher;


        public TodoItemModel(TodoItem todoItem, IPublisher<TodoItemChangedEventArgs> publisher)
        {
            DomainModel = todoItem;
            _Publisher = publisher;
            _TodoUseCase = ContainerHelper.GetRequiredService<TodoUseCase>();
        }

        public async Task ToggleCompletedAsync(bool completed)
        {
            if(completed)
            {
                DomainModel.Complete();
            }
            else
            {
                DomainModel.RevertComplete();
            }

            await UpdateAsync();
        }


        public async Task ToggleImportantAsync()
        {
            DomainModel.IsImportant = !DomainModel.IsImportant;
            await UpdateAsync();
        }

        public async Task UpdateAsync()
        {
            await _TodoUseCase.EditAsync(DomainModel);

            _Publisher.Publish(new TodoItemChangedEventArgs(DomainModel.Id));
        }

        public async void DeleteAsync()
        {
        // メッセージ機構でTodoModelに通知したい
        // https://github.com/Cysharp/MessagePipe
        }
    }
}
