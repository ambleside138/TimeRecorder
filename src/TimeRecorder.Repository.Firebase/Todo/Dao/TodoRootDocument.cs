using Google.Cloud.Firestore;
using TimeRecorder.Repository.Firebase.Shared;

namespace TimeRecorder.Repository.Firebase.Todo.Dao
{

    [FirestoreData]
    class TodoRootDocument : DocumentBase
    {
        [FirestoreProperty]
        public string UserId { get; set; }

        [FirestoreProperty]
        public int CompletedItemCount { get; set; }

        public TodoItemDocument[] TodoItems { get; set; }

        public TodoItemDocument[] CompletedTodoItems { get; set; }

    }
}
