using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Shared;

namespace TimeRecorder.Domain.Domain.Todo
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TodoItem : Entity<TodoItem>, IIdentifiable<TodoItemIdentity>
    {
        public TodoItemIdentity Id { get; private set; }

        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get => _Title;
            set => RaisePropertyChangedIfSet(ref _Title, value);
        }
        #endregion

        #region IsImportant変更通知プロパティ
        private bool _IsImportant;

        public bool IsImportant
        {
            get => _IsImportant;
            set => RaisePropertyChangedIfSet(ref _IsImportant, value);
        }
        #endregion


        #region CompletedDateTime変更通知プロパティ
        private DateTime? _CompletedDateTime;

        public DateTime? CompletedDateTime
        {
            get => _CompletedDateTime;
            set => RaisePropertyChangedIfSet(ref _CompletedDateTime, value);
        }
        #endregion

        public bool IsCompleted => CompletedDateTime.HasValue;

        #region Memo変更通知プロパティ
        private string _Memo;

        public string Memo
        {
            get => _Memo;
            set => RaisePropertyChangedIfSet(ref _Memo, value);
        }
        #endregion


        public TodoListIdentity TodoListId { get; set; } = TodoListIdentity.None;


        public static TodoItem ForNew()
        {
            return new TodoItem { Id = IdentityHelper.CreateTempId<TodoItemIdentity>() };
        }

        public static TodoItem ForDoneFilter()
        {
            return new TodoItem { Id = TodoItemIdentity.DoneFilter };
        }

        private TodoItem() { }



        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }

        public static TodoItem Rebuild(TodoItemIdentity id, TodoItem source)
        {
            return new TodoItem
            {
                Id = id,
                Title = source.Title,
                IsImportant = source.IsImportant,
                Memo = source.Memo,
            };
        }

        public static TodoItem FromRepository(
            string id, 
            string title, 
            bool isImportant, 
            DateTime? completedDateTime, 
            string memo, 
            string todoListId)
        {
            return new TodoItem
            {
                Id = new TodoItemIdentity(id),
                Title = title,
                IsImportant = isImportant,
                CompletedDateTime = completedDateTime,
                Memo = memo,
                TodoListId = new TodoListIdentity(todoListId),
            };
        }


        private string GetDebuggerDisplay()
        {
            return Title;
        }

        public bool IsMatch(TodoItemIdentity id)
        {
            if (id == null)
                return false;

            return id.Equals(Id);
        }

        public TodoItemIdentity GetIdentity() => Id;
    }
}
