using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Shared;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.Domain.Todo;

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
        private set => RaisePropertyChangedIfSet(ref _CompletedDateTime, value);
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

    #region PlanDate変更通知プロパティ
    private YmdString _PlanDate = YmdString.Empty;

    public YmdString PlanDate
    {
        get => _PlanDate;
        set => RaisePropertyChangedIfSet(ref _PlanDate, value);
    }
    #endregion

    #region CreatedAt変更通知プロパティ
    private DateTime _CreatedAt;

    public DateTime CreatedAt
    {
        get => _CreatedAt;
        set => RaisePropertyChangedIfSet(ref _CreatedAt, value);
    }
    #endregion

    #region UpdatedAt変更通知プロパティ
    private DateTime _UpdatedAt = DateTime.Now;

    public DateTime UpdatedAt
    {
        get => _UpdatedAt;
        set => RaisePropertyChangedIfSet(ref _UpdatedAt, value);
    }
    #endregion


    public ObservableCollection<YmdString> TodayTaskDates { get; init; } = new();

    //public ReadOnlyObservableCollection<YmdString> TodayTaskDates => _TodayTaskDates;

    public bool IsTodayTask => TodayTaskDates.Contains(YmdString.Today);


    public TodoListIdentity TodoListId { get; set; } = TodoListIdentity.None;



    public static TodoItem ForNew()
    {
        return new TodoItem { Id = IdentityHelper.CreateTempId<TodoItemIdentity>() };
    }

    public static TodoItem ForDoneFilter()
    {
        return new TodoItem { Id = TodoItemIdentity.DoneFilter };
    }

    private TodoItem()
    {
        // TodayTaskDates = new ReadOnlyObservableCollection<YmdString>(_TodayTaskDates);
    }



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
        string todoListId,
        DateTime createdAt,
        DateTime updatedAt,
        string[] todayTaskDates,
        string planDate)
    {
        return new TodoItem
        {
            Id = new TodoItemIdentity(id),
            Title = title,
            IsImportant = isImportant,
            CompletedDateTime = completedDateTime,
            Memo = memo,
            TodoListId = new TodoListIdentity(todoListId),
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            TodayTaskDates = new ObservableCollection<YmdString>(todayTaskDates?.Select(d => new YmdString(d))),
            PlanDate = new YmdString(planDate)
        };
    }


    public void AddAsTodayTask()
    {
        var today = YmdString.Today;
        if (TodayTaskDates.Contains(today))
            return;

        TodayTaskDates.Add(today);
    }

    public void ClearAsTodayTask()
    {
        TodayTaskDates.Remove(YmdString.Today);
    }

    public void Complete() => CompleteCore(true);

    public void RevertComplete() => CompleteCore(false);


    private void CompleteCore(bool isComplete)
    {
        CompletedDateTime = isComplete ? DateTime.Now : null;
        UpdatedAt = SystemClockServiceLocator.Current.Now;
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

    public bool IsDoneFilter => Id == TodoItemIdentity.DoneFilter;
}
