﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Repository.Firebase.Shared;
using TimeRecorder.Repository.Firebase.Todo.Dao;

namespace TimeRecorder.Repository.Firebase.Todo;

public class FirestoreTodoListRepository : ITodoListRepository
{
    public async Task<TodoListIdentity> AddAsync(TodoList item)
    {
        FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();
        TodoListDocument doc = TodoListDocument.FromDomainObject(item);

        return await new TodoListDao(db, FirebaseAuthenticator.Current.UserId).SetAsync(item.Id, doc);
    }

    public async Task DeleteAsync(TodoListIdentity id)
    {
        FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();
        await new TodoListDao(db, FirebaseAuthenticator.Current.UserId).DeleteAsync(id);
    }

    public async Task EditAsync(TodoList item) => await AddAsync(item);

    public async Task<TodoList[]> SelectAsync()
    {
        FirestoreDb db = await FirestoreAccessor.CreateDbClientAsync();

        IEnumerable<TodoListDocument> todoLists = await new TodoListDao(db, FirebaseAuthenticator.Current.UserId).SelectAsync();

        return todoLists?
                  .Select(i => i.ConvertToDomainObject())
                  .ToArray() ?? Array.Empty<TodoList>();
    }
}
