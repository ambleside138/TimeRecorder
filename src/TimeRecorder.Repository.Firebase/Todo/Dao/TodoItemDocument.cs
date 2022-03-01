using System;
using System.Linq;
using Google.Cloud.Firestore;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Repository.Firebase.Shared;

namespace TimeRecorder.Repository.Firebase.Todo.Dao;

[FirestoreData]
internal class TodoItemDocument : DocumentBase
{
    public string Id { get; set; }

    [FirestoreProperty]
    public string Title { get; set; }

    [FirestoreProperty]
    public bool IsImportant { get; set; }

    [FirestoreProperty]
    public Timestamp? CompletedDateTime { get; set; }

    [FirestoreProperty]
    public string Memo { get; set; }

    [FirestoreProperty]
    public string TodoListId { get; set; }

    [FirestoreProperty]
    public string[] TodayTaskDates { get; set; }

    [FirestoreProperty]
    public string PlanDate { get; set; }

    public static TodoItemDocument FromDomainObject(TodoItem todoItem)
    {
        return new TodoItemDocument
        {
            Title = todoItem.Title,
            IsImportant = todoItem.IsImportant,
            CompletedDateTime = todoItem.CompletedDateTime.ToTimestamp(),
            Memo = todoItem.Memo,
            TodoListId = todoItem.TodoListId.Value,
            CreatedAt = todoItem.CreatedAt.ToTimestamp(),
            TodayTaskDates = todoItem.TodayTaskDates.Select(d => d.Value).ToArray(),
            PlanDate = todoItem.PlanDate.Value,
        };
    }

    public TodoItem ConvertToDomainObject()
    {
        return TodoItem.FromRepository(
                         Id,
                         Title,
                         IsImportant,
                         CompletedDateTime?.ToLocalDateTime(),
                         Memo,
                         TodoListId,
                         CreatedAt.ToLocalDateTime(),
                         UpdatedAt.ToLocalDateTime(),
                         TodayTaskDates ?? Array.Empty<string>(),
                         PlanDate);
    }
}
