using Google.Cloud.Firestore;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Repository.Firebase.Shared;

namespace TimeRecorder.Repository.Firebase.Todo.Dao;

[FirestoreData]
internal class TodoListDocument : DocumentBase
{
    public string Id { get; set; }

    [FirestoreProperty]
    public string IconKey { get; set; }

    [FirestoreProperty]
    public string Title { get; set; }

    [FirestoreProperty]
    public string Background { get; set; }


    [FirestoreProperty]
    public int DisplayOrder { get; set; }


    public static TodoListDocument FromDomainObject(TodoList todolist)
    {
        return new TodoListDocument
        {
            IconKey = todolist.IconKey,
            Title = todolist.Title,
            Background = todolist.Background,
            DisplayOrder = todolist.DisplayOrder
        };
    }

    public TodoList ConvertToDomainObject()
    {
        return new TodoList(new TodoListIdentity(Id))
        {
            IconKey = IconKey,
            Title = Title,
            Background = Background,
            DisplayOrder = DisplayOrder,
            CreatedAt = CreatedAt.ToLocalDateTime(),
            UpdatedAt = UpdatedAt.ToLocalDateTime(),
        };
    }

}
