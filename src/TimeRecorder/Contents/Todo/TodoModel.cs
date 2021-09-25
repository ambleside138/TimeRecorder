using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Domain.UseCase.System;
using TimeRecorder.Domain.UseCase.Todo;
using TimeRecorder.Domain.Utility.SystemClocks;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.Todo
{

    internal class TodoModel : NotificationDomainModel
    {

        // FilterClassとListクラスにわける
        // ListクラスはFilterClassを保持
        private readonly TodoList _TodayList = new TodayTodoList();
        private readonly TodoList _ImportantList = new ImportantTodoList();
        private readonly TodoList _FutureList = new FutureTodoList();
        public TodoList DefaultList { get; } = new(TodoListIdentity.None) { Title = "タスク", IconKey = "Home" };

        private readonly TodoItemUseCase _TodoUseCase;
        private readonly TodoListUseCase _TodoListUseCase;
        private readonly AuthenticationUseCase _AuthenticationUseCase;

        public ObservableCollection<TodoList> TodoListCollection { get; } = new();


        private List<TodoItem> _AllTodoItems;

        public ObservableCollection<TodoItem> FilteredTodoItems { get; } = new();

        private TodoListIdentity _CurrentTodoList;

        #region LoginStatus変更通知プロパティ
        private LoginStatus _LoginStatus;

        public LoginStatus LoginStatus
        {
            get => _LoginStatus;
            set => RaisePropertyChangedIfSet(ref _LoginStatus, value);
        }
        #endregion

        private readonly ISubscriber<TodoItemChangedEventArgs> _Subscriber;

        private readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;

        public TodoModel(ISubscriber<TodoItemChangedEventArgs> subscriber)
        {
            _TodoUseCase = ContainerHelper.Provider.GetRequiredService<TodoItemUseCase>();
            _TodoListUseCase = ContainerHelper.Provider.GetRequiredService<TodoListUseCase>();

            _AuthenticationUseCase = ContainerHelper.Provider.GetRequiredService<AuthenticationUseCase>();

            _Subscriber = subscriber;
            LoadFixedTodoList();

            // IDisposableの管理が必要
            _Subscriber.Subscribe(s => Subscribe(s));
        }

        private void Subscribe(TodoItemChangedEventArgs args)
        {
            switch (args.ChangeType)
            {
                case ChangeType.Updated:
                    Filter(_AllTodoItems, _CurrentTodoList);
                    break;

                case ChangeType.Deleted:
                    var target = FilteredTodoItems.FirstOrDefault(i => i.Id == args.TodoItemIdentity);
                    if(target != null)
                    {
                        _ = FilteredTodoItems.Remove(target);
                        _ = _AllTodoItems.Remove(target);
                    }
                    Filter(_AllTodoItems, _CurrentTodoList);

                    break;
                default:
                    break;
            }
        }

        private void LoadFixedTodoList()
        {
            TodoListCollection.Add(_TodayList);
            TodoListCollection.Add(_ImportantList);
            TodoListCollection.Add(_FutureList);
            TodoListCollection.Add(DefaultList);
        }

        private async Task LoadTodoListAsync()
        {
            for (int i = TodoListCollection.Count - 1; i >= 0; i--)
            {
                if(TodoListCollection[i].Id.IsFixed == false
                    || TodoListCollection[i].Id == TodoListIdentity.Divider)
                {
                    TodoListCollection.RemoveAt(i);
                }
            }

            var list = await _TodoListUseCase.SelectAsync();
            if(list?.Length > 0)
            {
                TodoListCollection.Add(new TodoList(TodoListIdentity.Divider));
                TodoListCollection.AddRange(list);
            }
        }

        public async Task InitializeAsync()
        {
            LoginStatus = _AuthenticationUseCase.TrySignin();
            await LoadTodoListAsync();
        }


        public async Task LoadTodoItemsAsync(TodoListIdentity selectedListId)
        {
            _CurrentTodoList = selectedListId;

            var todoItems = await _TodoUseCase.SelectAsync();
            _AllTodoItems = new List<TodoItem>(todoItems);

            Filter(todoItems, selectedListId);
        }

        private void Filter(IEnumerable<TodoItem> todoItems, TodoListIdentity selectedListId)
        {
            foreach (var todolist in TodoListCollection)
            {

                TodoItem[] ownListItems = todoItems.Where(i => todolist.MatchTodoItem(i))
                                                   .ToArray();


                if (todolist.Id == selectedListId)
                {
                    var tmpTodoItems = new List<TodoItem>(ownListItems.Where(i => i.IsCompleted == false));

                    var doneItems = ownListItems.Where(i => i.IsCompleted).ToArray();
                    if (doneItems.Any()
                        && todolist.DoneItemVisilble)
                    {
                        tmpTodoItems.Add(TodoItem.ForDoneFilter());
                        tmpTodoItems.AddRange(doneItems);
                    }
                    todolist.FilteredCount = tmpTodoItems.Count(i => i.IsDoneFilter == false);

                    if (tmpTodoItems.SequenceEqual(FilteredTodoItems) == false)
                    {
                        FilteredTodoItems.Clear();
                        FilteredTodoItems.AddRange(tmpTodoItems);
                    }

                }

            }

        }

        public async Task<TodoItemIdentity> AddTodoItemAsync(TodoListIdentity selectedListId, TodoItem item)
        {
            item.TodoListId = selectedListId;
            TodoItemIdentity id = await _TodoUseCase.AddAsync(item);
            await LoadTodoItemsAsync(selectedListId);
            return id;
        }

        public async Task<TodoListIdentity> AddTodoListAsync()
        {
            var list = TodoList.ForNew(TodoListCollection.Count(i => i.Id.IsFixed == false) + 1);

            TodoListIdentity id = await _TodoListUseCase.AddAsync(list);

            await LoadTodoListAsync();

            return id;
        }

        public async Task SetTodoListTitleAsync(TodoList todoList)
        {
            await _TodoListUseCase.EditAsync(todoList);
        }

        public async Task DeleteTodoListAsync(TodoList todoList)
        {
            TodoListCollection.Remove(todoList);
            await _TodoListUseCase.DeleteAsync(todoList.Id);
        }
    }
}
