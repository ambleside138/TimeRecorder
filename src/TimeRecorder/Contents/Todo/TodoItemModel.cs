﻿using MessagePipe;
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
    interface ITodoItemModel { }


    class TodoItemModel : ITodoItemModel
    {
        private readonly TodoItem _DomainModel;

        private readonly TodoUseCase _TodoUseCase;

        private readonly IPublisher<TodoItemChangedEventArgs> _Publisher;


        public TodoItemModel(TodoItem todoItem, IPublisher<TodoItemChangedEventArgs> publisher)
        {
            _DomainModel = todoItem;
            _Publisher = publisher;
            _TodoUseCase = ContainerHelper.GetRequiredService<TodoUseCase>();
        }


        public async Task ToggleImportantAsync()
        {
            _DomainModel.IsImportant = !_DomainModel.IsImportant;
            await UpdateAsync();
        }

        public async Task UpdateAsync()
        {

            _Publisher.Publish(new TodoItemChangedEventArgs(_DomainModel.Id));
        }

        public async void DeleteAsync()
        {
        // メッセージ機構でTodoModelに通知したい
        // https://github.com/Cysharp/MessagePipe
        }
    }
}
