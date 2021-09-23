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
    public class FirestoreTodoItemRepository : ITodoItemRepository
    {
        public async Task<TodoItemIdentity> AddAsync(TodoItem item)
        {
            FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();
            TodoItemDocument doc = TodoItemDocument.FromDomainObject(item);

            return await new TodoItemDao(db, FirebaseAuthenticator.Current.UserId).SetAsync(item.Id, doc);
        }

        public async Task DeleteAsync(TodoItemIdentity id)
        {
            FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();
            await new TodoItemDao(db, FirebaseAuthenticator.Current.UserId).DeleteAsync(id);
        }

        public async Task EditAsync(TodoItem item) =>  await AddAsync(item);

        public async Task<TodoItem[]> SelectAsync()
        {
            FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();

            TodoRootDocument doc = await new TodoItemDao(db, FirebaseAuthenticator.Current.UserId).SelectAsync();

            return doc?.TodoItems
                      .Select(i => i.ConvertToDomainObject())
                      .ToArray() ?? Array.Empty<TodoItem>();
        }

    }
}
