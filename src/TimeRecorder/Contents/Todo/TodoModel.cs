using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Domain.UseCase.System;
using TimeRecorder.Domain.UseCase.Todo;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.Todo
{
    internal class TodoModel : NotificationDomainModel
    {
        private readonly TodoList _TodayList = new(TodoListIdentity.Today) { Title = "今日の予定", IconKey = "WhiteBalanceSunny" };
        private readonly TodoList _ImportantList = new(TodoListIdentity.Important) { Title = "重要", IconKey = "Star" };
        private readonly TodoList _FutureList = new(TodoListIdentity.Future) { Title = "今後の予定", IconKey = "Calendar" };
        private readonly TodoList _NoneList = new(TodoListIdentity.None) { Title = "タスク", IconKey = "Home" };

        private readonly TodoUseCase _TodoUseCase;
        private readonly AuthenticationUseCase _AuthenticationUseCase;

        public ObservableCollection<TodoList> TodoListCollection { get; } = new();

        public TodoItem[] TodoItems { get; private set; } = System.Array.Empty<TodoItem>();

        public ObservableCollection<TodoItem> FilteredTodoItems { get; } = new();

        #region LoginStatus変更通知プロパティ
        private LoginStatus _LoginStatus;

        public LoginStatus LoginStatus
        {
            get => _LoginStatus;
            set => RaisePropertyChangedIfSet(ref _LoginStatus, value);
        }
        #endregion


        public TodoModel()
        {
            _TodoUseCase = new TodoUseCase(ContainerHelper.Resolver.Resolve<ITodoRepository>());
            _AuthenticationUseCase = new AuthenticationUseCase(ContainerHelper.Resolver.Resolve<IAccountRepository>());

            TodoListCollection.Add(_TodayList);
            TodoListCollection.Add(_ImportantList);
            TodoListCollection.Add(_FutureList);
            TodoListCollection.Add(_NoneList);

            //Signin();
        }

        private void LoadTodoList()
        {

        }

        private void LoadTodoItems(TodoListIdentity selectedListId)
        {
            if(LoginStatus == null)
            {
                LoginStatus = _AuthenticationUseCase.TrySignin();
            }

            TodoItems = _TodoUseCase.Select();

            Filter(selectedListId);
        }

        private void Filter(TodoListIdentity selectedListId)
        {
            FilteredTodoItems.Clear();

            var ownListItems = TodoItems.Where(i => i.TodoListId == selectedListId);
            FilteredTodoItems.AddRange(ownListItems.Where(i => i.IsCompleted == false));

            var doneItems = ownListItems.Where(i => i.IsCompleted).ToArray();
            if(doneItems.Any())
            {
                FilteredTodoItems.Add(TodoItem.ForDoneFilter());
                FilteredTodoItems.AddRange(doneItems);
            }

        }

        public void AddTodoItem(TodoListIdentity selectedListId, TodoItem item)
        {
            item.TodoListId = selectedListId;
            _TodoUseCase.Add(item);
            LoadTodoItems(selectedListId);
        }

        public void DeleteTodoItem(TodoListIdentity selectedListId, TodoItemIdentity itemIdentity)
        {
            _TodoUseCase.Delete(itemIdentity);
            LoadTodoItems(selectedListId);
        }

    }
}
