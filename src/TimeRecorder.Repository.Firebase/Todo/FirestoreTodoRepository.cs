using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Repository.Firebase.Shared;
using TimeRecorder.Repository.Firebase.Todo.Dao;

namespace TimeRecorder.Repository.Firebase.Todo
{
    public class FirestoreTodoRepository : ITodoRepository
    {
        public async Task<TodoItemIdentity> AddAsync(TodoItem item)
        {
            FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();
            TodoItemDocument doc = TodoItemDocument.FromDomainObject(item);

            return await new TodoDao(db, FirebaseAuthenticator.Current.UserId).SetAsync(item.Id, doc);
        }

        public Task DeleteAsync(TodoItemIdentity id)
        {
            throw new NotImplementedException();
        }

        public async Task EditAsync(TodoItem item) =>  await AddAsync(item);

        public async Task<TodoItem[]> SelectAsync()
        {
            FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();

            TodoRootDocument doc = await new TodoDao(db, FirebaseAuthenticator.Current.UserId).SelectAsync();

            return doc?.TodoItems
                      .Select(i => i.ConvertToDomainObject())
                      .ToArray() ?? Array.Empty<TodoItem>();
        }

        public TodoItem[] SelectByListId(TodoListIdentity id)
        {
            throw new NotImplementedException();
        }

        public TodoItem[] SelectWithCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
