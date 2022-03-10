using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Todo;

public static class TodoListFactory
{
    public static IEnumerable<TodoList> CreateDefaultCollections()
    {
        yield return new TodayTodoList();
        yield return new ImportantTodoList();
        yield return new FutureTodoList();
        yield return DefaultList;
    }

    public static TodoList DefaultList { get; } = new(TodoListIdentity.None) { Title = "タスク", IconKey = "Home" };

}

public class TodayTodoList : TodoList
{
    public TodayTodoList()
        : base(TodoListIdentity.Today)
    {
        Title = "今日の予定";
        IconKey = "WhiteBalanceSunny";
    }

    public override bool MatchTodoItem(TodoItem item) => item.IsTodayTask;

    public override void SetDefaultItemProperties(TodoItem todoItem)
    {
        base.SetDefaultItemProperties(todoItem);
        todoItem.AddAsTodayTask();

    }
}

public class ImportantTodoList : TodoList
{
    public ImportantTodoList()
        : base(TodoListIdentity.Important)
    {
        Title = "重要";
        IconKey = "StarOutline";
        DoneItemVisilble = false;
    }

    public override bool MatchTodoItem(TodoItem item) => item.IsImportant;

    public override void SetDefaultItemProperties(TodoItem todoItem)
    {
        base.SetDefaultItemProperties(todoItem);
        todoItem.IsImportant = true;
    }
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
        return item.PlanDate.IsEmpty == false
                 && item.IsCompleted == false;
    }

    public override void SetDefaultItemProperties(TodoItem todoItem)
    {
        base.SetDefaultItemProperties(todoItem);
        todoItem.PlanDate = new YmdString(Utility.SystemClocks.SystemClockServiceLocator.Current.Now);

    }
}
