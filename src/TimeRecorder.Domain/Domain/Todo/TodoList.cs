using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Todo
{
    public class TodayTodoList : TodoList
    {
        public TodayTodoList()
            : base(TodoListIdentity.Today) 
        {
            Title = "今日の予定";
            IconKey = "WhiteBalanceSunny";
        }

        public override bool MatchTodoItem(TodoItem item) => item.IsTodayTask;
    }

    public class ImportantTodoList : TodoList
    {
        public ImportantTodoList()
            :base(TodoListIdentity.Important) 
        {
            Title = "重要";
            IconKey = "StarOutline";
            DoneItemVisilble = false;
        }

        public override bool MatchTodoItem(TodoItem item) => item.IsImportant;
    }

    public class FutureTodoList : TodoList
    {
        public FutureTodoList()
            : base(TodoListIdentity.Future) 
        {
            Title = "今後の予定";
            IconKey = "Calendar";
        }

        public override bool MatchTodoItem(TodoItem item)
        {
            return item.PlanDate.CompareTo(YmdString.Today) > 0;
        }
    }



    public class TodoList : Entity<TodoList>
    {
        public TodoListIdentity Id { get; private set; }



        #region IconCharacter変更通知プロパティ
        private string _IconCharacter;

        public string IconCharacter
        {
            get => _IconCharacter;
            set => RaisePropertyChangedIfSet(ref _IconCharacter, value);
        }
        #endregion


        #region IconKey変更通知プロパティ
        private string _IconKey;

        public string IconKey
        {
            get => _IconKey;
            set => RaisePropertyChangedIfSet(ref _IconKey, value);
        }
        #endregion


        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get => _Title;
            set => RaisePropertyChangedIfSet(ref _Title, value);
        }
        #endregion


        #region Background変更通知プロパティ
        private string _Background;

        public string Background
        {
            get => _Background;
            set => RaisePropertyChangedIfSet(ref _Background, value);
        }
        #endregion


        #region FilteredCount変更通知プロパティ
        private int _FilteredCount;

        public int FilteredCount
        {
            get => _FilteredCount;
            set => RaisePropertyChangedIfSet(ref _FilteredCount, value);
        }
        #endregion


        #region DoneItemVisilble変更通知プロパティ
        private bool _DoneItemVisilble = true;

        public bool DoneItemVisilble
        {
            get => _DoneItemVisilble;
            protected set => RaisePropertyChangedIfSet(ref _DoneItemVisilble, value);
        }
        #endregion


        #region DisplayOrder変更通知プロパティ
        private int _DisplayOrder;

        public int DisplayOrder
        {
            get => _DisplayOrder;
            set => RaisePropertyChangedIfSet(ref _DisplayOrder, value);
        }
        #endregion



        public TodoList(TodoListIdentity id)
        {
            Id = id;
        }


        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }

        public virtual bool MatchTodoItem(TodoItem item)
        {
            return item.TodoListId == Id;
        }
    }
}
