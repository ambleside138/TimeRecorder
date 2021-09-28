using MessagePipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Contents.Todo.TodoItems
{
    class TodoItemDoneFilterViewModel : TodoItemViewModel
    {
        // リストごとに記憶しておく必要あり
        #region IsExpanded変更通知プロパティ
        private bool _IsExpanded;
        private readonly IPublisher<TodoItemFilterEventArgs> _Publisher;

        public bool IsExpanded
        {
            get => _IsExpanded;
            set
            {
                _Publisher.Publish(new TodoItemFilterEventArgs { NeedShowDoneItem = value });
                RaisePropertyChangedIfSet(ref _IsExpanded, value);
            }
        }
        #endregion




        public TodoItemDoneFilterViewModel(TodoItem item, IPublisher<TodoItemFilterEventArgs> publisher)
            : base(item, null)
        {
            IsDoneFilter = true;
            _Publisher = publisher;
        }
    }
}
