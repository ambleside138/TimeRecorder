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
        private readonly TodoList _NoneList = new(TodoListIdentity.None) { Title = "タスク", IconKey = "Home" };

        private readonly TodoUseCase _TodoUseCase;
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
            _TodoUseCase = ContainerHelper.Provider.GetRequiredService<TodoUseCase>();
            _AuthenticationUseCase = ContainerHelper.Provider.GetRequiredService<AuthenticationUseCase>();

            TodoListCollection.Add(_TodayList);
            TodoListCollection.Add(_ImportantList);
            TodoListCollection.Add(_FutureList);
            TodoListCollection.Add(_NoneList);
            _Subscriber = subscriber;

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



        public async Task LoadTodoItemsAsync(TodoListIdentity selectedListId)
        {
            if(LoginStatus == null)
            {
                LoginStatus = _AuthenticationUseCase.TrySignin();
            }

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



    }
}
