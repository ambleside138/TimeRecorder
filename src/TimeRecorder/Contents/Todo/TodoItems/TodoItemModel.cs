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


        private readonly TodoItemUseCase _TodoUseCase;

        private readonly IPublisher<TodoItemChangedEventArgs> _Publisher;


        public TodoItemModel(TodoItem todoItem, IPublisher<TodoItemChangedEventArgs> publisher)
        {
            DomainModel = todoItem;
            _Publisher = publisher;
            _TodoUseCase = ContainerHelper.GetRequiredService<TodoItemUseCase>();
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

        public async Task ToggleTodayTask()
        {
            if(DomainModel.IsTodayTask)
            {
                DomainModel.ClearAsTodayTask();
            }
            else
            {
                DomainModel.AddAsTodayTask();
            }

            await UpdateAsync();
        }

        public async Task UpdateAsync()
        {
            _Publisher.Publish(new TodoItemChangedEventArgs(ChangeType.Updated, DomainModel.Id));
            await _TodoUseCase.EditAsync(DomainModel);
        }

        public async Task DeleteAsync()
        {
            _Publisher.Publish(new TodoItemChangedEventArgs(ChangeType.Deleted, DomainModel.Id));
            await _TodoUseCase.DeleteAsync(DomainModel.Id);
        }
    }
}
